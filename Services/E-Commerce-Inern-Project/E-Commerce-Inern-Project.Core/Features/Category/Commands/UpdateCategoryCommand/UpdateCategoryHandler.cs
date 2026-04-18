using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICategoryServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Category.Commands.UpdateCategoryCommand
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryRequest, Result<CategoryResponse>>
    {
        private readonly ICategoryService _CatService;
        public UpdateCategoryHandler(ICategoryService CatService)
        {
            _CatService = CatService;
        }
        public async Task<Result<CategoryResponse>> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            return await _CatService.UpdateCategory(request);
        }
    }
}
