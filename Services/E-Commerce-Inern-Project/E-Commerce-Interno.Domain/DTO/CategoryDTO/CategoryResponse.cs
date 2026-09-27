namespace E_Commerce_Interno.Domain.DTO.CategoryDTO
{
    public record CategoryResponse(Guid CategoryID,string CategoryName, string CategoryImageUrl, string HashImage, Guid? ParentCategoryID );
}
