using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICategoryRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.CreateProductCommand;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductCommand;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductRatingCommand;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using System.Security.Cryptography;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.ProductServices
{
    public class ProductService : IProductService //add rabbitmq
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductRatesRepository _ProductRatesRepo;
        private readonly ICategoryRepository _CategoryRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IMapper _Mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IAsyncPolicy _AsyncPolicy;
        private readonly ITransectionRepository _Transection;
        private readonly string _CurrentDIr = $@"{Directory.GetCurrentDirectory()}\wwwroot\ProductsImages";
        private readonly string AuditKey = "Interno.Audit";
        public ProductService(IProductRepository productRepo, ITransectionRepository Transection, IAsyncPolicy AsyncPolicy, IUserRepository UserRepo, ICategoryRepository CategoryRepo, IRabbitMQPublisher Publisher, IProductRatesRepository ProductRatesRepo, IMapper mapper, ILogger<ProductService> logger)
        {
            _productRepo = productRepo;
            _ProductRatesRepo = ProductRatesRepo;
            _CategoryRepo = CategoryRepo;
            _UserRepo = UserRepo;
            _Publisher = Publisher;
            _Mapper = mapper;
            _AsyncPolicy = AsyncPolicy;
            _logger = logger;
            _Transection = Transection;
        }

        public async Task<Result<bool>> CreateProductAsync(ProductRequest request)
        {
            var transection = await _Transection.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<bool>.InternalError("Failed To Initial Transetcion");
            }
            try
            {


                Category? Category = await _CategoryRepo.GetCategoryById(request.CategoryID.Value);
                if (Category == null)
                {
                    return Result<bool>.BadRequest("Category Doesnt Exists");
                }

                Product newProduct = _Mapper.Map<Product>(request);

                if (request.ProductImage != null)
                {

                    string? imagePath = await UploadImage(request.ProductImage, Category);
                    if (imagePath == null)
                    {
                        return Result<bool>.InternalError("Failed To Upload Image");
                    }
                    newProduct.ProductImageUrl = imagePath;
                    newProduct.HashImage = HashImage(request.ProductImage);
                }

                if (_productRepo.AddProduct(newProduct) == null)
                {
                    DeleteImage($@"{_CurrentDIr}\{Category.ParentCategory.CategoryName}\{Category.CategoryName}\{newProduct.ProductImageUrl}");

                    return Result<bool>.InternalError("Failed To Add New PRoduct");
                }



                if (!await _productRepo.SaveChanges())
                {
                    DeleteImage($@"{_CurrentDIr}\{Category.ParentCategory.CategoryName}\{Category.CategoryName}\{newProduct.ProductImageUrl}");
                    return Result<bool>.InternalError("Failed To Save Changes");
                }


                string JsonNewValues = JsonSerializer.Serialize<Product>(newProduct);
                AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Create, nameof(Product), null, JsonNewValues);
                await _AsyncPolicy.ExecuteAsync(async () =>
                {
                    await _Publisher.Publish(AuditKey, AuditRequest);
                });

                await transection.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await transection.RollbackAsync();
                return Result<bool>.InternalError($"An Error Occured While Creating Product Check logs,  Possible reasons :{ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteProductAsync(Guid Productid)
        {
            Product? Product = await _productRepo.GetProductByID_Tracking(Productid);
            Category? Category = await _CategoryRepo.GetCategoryById(Product.CategoryID);

            if (Product == null)
            {
                return Result<bool>.NotFound("Product Doesnt Exists");
            }
            string JsonOldValues = JsonSerializer.Serialize<Product>(Product);
            Product.IsDeleted = true;

            bool result = await _productRepo.SaveChanges();
            if (!result)
            {
                return Result<bool>.InternalError("Failed To Save Changes");
            }

            if (!DeleteImage($@"{_CurrentDIr}\{Category.ParentCategory.CategoryName}\{Category.CategoryName}\{Product.ProductImageUrl}"))
            {
                return Result<bool>.InternalError("Failed To Delete Image");
            }

            string JsonNewValues = JsonSerializer.Serialize<Product>(Product);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Delete, nameof(Product), JsonOldValues, JsonNewValues);
            await _AsyncPolicy.ExecuteAsync(async () =>
            {
                await _Publisher.Publish(AuditKey, AuditRequest);
            });

            return Result<bool>.Success(result);
        }

        public async Task<Result<bool>> UpdateProductAsync(ProductUpdateRequest UpdateRequest) // add transection
        {
            var transection = await _Transection.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<bool>.InternalError("Failed To Initial Transetcion");
            }
            try
            {

                Product? existing = await _productRepo.GetProductByID_Tracking(UpdateRequest.ProductID);
                if (!await _CategoryRepo.IsCategoryExistsByID(UpdateRequest.CategoryID))
                {
                    return Result<bool>.BadRequest("Category Doesnt Exists");
                }
                if (existing == null)
                {
                    return Result<bool>.NotFound("Product Doesnt Exists");
                }
                string JsonOldValues = JsonSerializer.Serialize<Product>(existing);


                _Mapper.Map(UpdateRequest, existing);

                if (UpdateRequest.ProductImage != null)
                {
                    Category? Category = await _CategoryRepo.GetCategoryById(UpdateRequest.CategoryID);
                    string? path;
                    if (string.IsNullOrEmpty(existing.ProductImageUrl))
                    {
                        path = await UploadImage(UpdateRequest.ProductImage, Category);
                        if (string.IsNullOrEmpty(path))
                        {
                            return Result<bool>.InternalError("failed to upload image");
                        }

                    }
                    else
                    {
                        path = await UpdateImage(UpdateRequest.ProductImage, existing.ProductImageUrl, Category);
                        if (string.IsNullOrEmpty(path))
                        {
                            return Result<bool>.InternalError("failed to update image");
                        }

                    }
                    existing.ProductImageUrl = path;
                    existing.HashImage = HashImage(UpdateRequest.ProductImage);

                }

                if (!await _productRepo.SaveChanges())
                {
                    return Result<bool>.InternalError("Failed To Save Changes");
                }



                string JsonNewValues = JsonSerializer.Serialize<Product>(existing);
                AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Update, nameof(Product), JsonOldValues, JsonNewValues);
                await _AsyncPolicy.ExecuteAsync(async () =>
                {
                    await _Publisher.Publish(AuditKey, AuditRequest);
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
        public async Task<Result<bool>> UpdateProductRating(UpdateProductRatingRequest request)
        {
            Product? existing = await _productRepo.GetProductByID_Tracking(request.ProductID);
            if (existing == null)
            {
                return Result<bool>.NotFound("Product Doesnt Exists");
            }
            string JsonOldValues = JsonSerializer.Serialize<Product>(existing);
            existing.Rating = existing.TotalRating == 0 ? request.Rating : CalculateRatingCls.CalcuateTheRating(existing.TotalRating, await CalculateAvg(existing.ProductID));
            existing.TotalRating += 1;

            if (!await _productRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges");
            }

            string JsonNewValues = JsonSerializer.Serialize<Product>(existing);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Update, nameof(Product), JsonOldValues, JsonNewValues);
            await _AsyncPolicy.ExecuteAsync(async () =>
            {
                await _Publisher.Publish(AuditKey, AuditRequest);
            });

            return Result<bool>.Success(existing.IsDeleted);
        }
        private async Task<string?> UploadImage(IFormFile image, Category Category)
        {
            string ImageDir = $@"{_CurrentDIr}\{Category.ParentCategory.CategoryName}\{Category.CategoryName}";
            if (!Directory.Exists(ImageDir))
            {
                Directory.CreateDirectory(ImageDir);
            }

            string imagename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            try
            {
                using (FileStream stream = new(Path.Combine(ImageDir, imagename), FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return imagename;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed To Upload Image");

                return null;
            }
        }
        private async Task<string?> UpdateImage(IFormFile image, string oldImage, Category Category)
        {
            string ImageDir = Path.Combine(_CurrentDIr, $@"{Category.ParentCategory.CategoryName}\{Category.CategoryName}\{oldImage}");
            if (!DeleteImage(ImageDir))
            {
                return null;
            }
            return await UploadImage(image, Category);
        }
        private bool DeleteImage(string Path)
        {
            if (!File.Exists(Path))
            {
                _logger.LogError("Image Doesnt Exists");
                return false;
            }
            try
            {
                File.Delete(Path);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed To Delete Old Image");

                return false;
            }
        }
        private string HashImage(IFormFile image)
        {
            using (var sha256 = SHA256.Create())
            using (Stream stream = image.OpenReadStream())
            {
                byte[] hashBytes = sha256.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private async Task<double> CalculateAvg(Guid ProductID)
        {
            var ProductRates = await _ProductRatesRepo.GetAllProductRatesForProduct(ProductID);
            return ProductRates.Any() ? ProductRates.Average(r => r.Rating) : 0;
        }

        private async Task<Guid> GetAdminID()
        {
            var admin = await _UserRepo.GetApplicationUserByEmail("admin@gmail.com");
            return admin.Id;
        }
    }
}


