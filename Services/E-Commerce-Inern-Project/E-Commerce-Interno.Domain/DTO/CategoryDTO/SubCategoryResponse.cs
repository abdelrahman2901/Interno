namespace E_Commerce_Interno.Domain.DTO.CategoryDTO
{
    public record SubCategoryResponse(Guid CategoryID, string CategoryName, string CategoryImageUrl, ICollection<CategoryResponse> SubCategories);
}
