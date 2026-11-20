using AutoMapper;
using E_Commerce_Inerno.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inerno.Core.Policies;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.ICategoryRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IColorRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Inerno.Core.Features.Products.Commands.UpdateProductCommand;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Products.Commands.UpdateProductCommand
{
    public class UpdateProductHandler : IRequestHandler<ProductUpdateRequest, Result<bool>>
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _CategoryRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IColorRepository _ColorRepo;
        private readonly IMapper _Mapper;
        private readonly ProductImageStorage _imageStorage;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IAsyncPolicy _policy;
        private readonly ITransectionRepository _Transection;
        private readonly string AuditKey = "Interno.Audit";

        public UpdateProductHandler(IProductRepository productRepo, IColorRepository ColorRepo, ITransectionRepository Transection, IPollyPolicy AsyncPolicy, IUserRepository UserRepo, ICategoryRepository CategoryRepo, IInternoEventPublisher Publisher, ProductImageStorage imageStorage, IMapper mapper)
        {
            _productRepo = productRepo;
            _CategoryRepo = CategoryRepo;
            _UserRepo = UserRepo;
            _ColorRepo = ColorRepo;
            _Publisher = Publisher;
            _Mapper = mapper;
            _imageStorage = imageStorage;
            _Transection = Transection;
            _policy = AsyncPolicy.GetPolicy();
        }

        public async Task<Result<bool>> Handle(ProductUpdateRequest request, CancellationToken cancellationToken)
        {
            var transection = await _Transection.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<bool>.InternalError("Failed To Initial Transetcion");
            }
            try
            {
                
                Product? existing = await _productRepo.GetProductByID(request.ProductID, true);
                if (!await _CategoryRepo.IsCategoryExistsByID(request.CategoryID))
                {
                    return Result<bool>.BadRequest("Category Doesnt Exists");
                }
                if (existing == null)
                {
                    return Result<bool>.NotFound("Product Doesnt Exists");
                }
                if(request.ColorID == null)
                {
                    Colors color = new Colors()
                    {
                        ColorID = Guid.NewGuid(),
                        ColorHexCode = request.ColorHexCode,
                        IsDeleted = false,
                        ColorName = null,
                    };
                    await _ColorRepo.CreateColors(color);
                    await _ColorRepo.SaveChanges();
                    request.ColorID = color.ColorID;
                }
                string JsonOldValues = GetProductJson(existing);

                _Mapper.Map(request, existing);

                if (request.ProductImage != null)
                {
                    Category? Category = await _CategoryRepo.GetCategoryById(request.CategoryID);
                    string? path;
                    if (string.IsNullOrEmpty(existing.ProductImageUrl))
                    {
                        path = await _imageStorage.UploadImage(request.ProductImage, Category);
                        if (string.IsNullOrEmpty(path))
                        {
                            return Result<bool>.InternalError("failed to upload image");
                        }
                    }
                    else
                    {
                        path = await _imageStorage.UpdateImage(request.ProductImage, existing.ProductImageUrl, Category);
                        if (string.IsNullOrEmpty(path))
                        {
                            return Result<bool>.InternalError("failed to update image");
                        }
                    }
                    existing.ProductImageUrl = path;
                    existing.HashImage = _imageStorage.HashImage(request.ProductImage);
                }

                if (!await _productRepo.SaveChanges())
                {
                    return Result<bool>.InternalError("Failed To Save Changes");
                }

                string JsonNewValues = GetProductJson(existing);
                AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Update, nameof(Product), JsonOldValues, JsonNewValues);

                await _policy.ExecuteAsync(async () =>
                {
                    await _Publisher.PublishAuditAsync(AuditRequest);
                });

                await transection.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await transection.RollbackAsync();
                return Result<bool>.InternalError($"An Error Occured While Updating Product Check logs,  Possible reasons :{ex.Message}");
            }
        }
        public string GetProductJson(Product product)
        {
            string metadata = JsonSerializer.Serialize(new
            {
                product.ProductID,
                product.ProductName,
                product.Price,
                product.SalePrice,
                product.Stock,
                product.CategoryID,
                product.ProductImageUrl,
                product.HashImage
            });
            return metadata;
        }
    }
}
