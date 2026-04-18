using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICategoryServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Category.Commands.CreateCategoryCommad
{
    public class CreateCategoryCmdHandler : IRequestHandler<CategoryRequest, Result<CategoryResponse>>
    {
        private readonly ICategoryService _CatSerice;

        public CreateCategoryCmdHandler(ICategoryService catSerice)
        {
            _CatSerice = catSerice;
        }

        public async Task<Result<CategoryResponse>> Handle(CategoryRequest request, CancellationToken cancellationToken)
        {

            return await _CatSerice.CreateCategory(request);
        }
    }
}
