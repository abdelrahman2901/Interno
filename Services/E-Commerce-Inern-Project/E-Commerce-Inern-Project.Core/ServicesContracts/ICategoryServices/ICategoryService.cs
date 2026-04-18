using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using E_Commerce_Inern_Project.Core.Features.Category.Commands.CreateCategoryCommad;
using E_Commerce_Inern_Project.Core.Features.Category.Commands.UpdateCategoryCommand;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.ICategoryServices
{
    public interface ICategoryService
    {
        Task<Result<CategoryResponse?>> GetCategoryByID(Guid CatID);
        Task<Result<CategoryResponse>> CreateCategory(CategoryRequest Request);
        Task<Result<bool>> DeleteCategory(Guid id);
        Task<Result<CategoryResponse>> UpdateCategory(UpdateCategoryRequest UpdateReq);
        Task<Result<IEnumerable<SubCategoryResponse>>> GetCategoriesWithSubCat();

    }
}
