
namespace E_Commerce_Inern_Project.Core.DTO.CategoryDTO
{
    public record CategoryResponse(Guid CategoryID,string CategoryName, string CategoryImageUrl, string HashImage, Guid? ParentCategoryID );
}
