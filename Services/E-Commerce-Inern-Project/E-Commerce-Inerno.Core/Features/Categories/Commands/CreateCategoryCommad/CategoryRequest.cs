using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce_Inern_Project.Core.Features.Categories.Commands.CreateCategoryCommad
{
    public class CategoryRequest : IRequest<Result<CategoryResponse>>
    {
        public string? CategoryName { get; set; }
        [SwaggerSchema(Nullable =true)]
        public Guid? ParentCategoryID { get; set; }
        public IFormFile? CategoryImage { get; set; }
    }
}
