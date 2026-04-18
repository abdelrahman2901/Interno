using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.DTO.CategoryDTO
{
    public record SubCategoryResponse(Guid CategoryID, string CategoryName, string CategoryImageUrl, ICollection<CategoryResponse> SubCategories);
}
