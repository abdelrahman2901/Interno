using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce_Inerno.Core.Features.Categories.Commands.UpdateCategoryCommand
{
    public class UpdateCategoryRequest : IRequest<Result<CategoryResponse>>
    {
        public Guid? CategoryID { get; set; }
        public string? CategoryName { get; set; }
        [SwaggerSchema(Nullable = true)]
        public Guid? ParentCategoryID { get; set; }
        [SwaggerSchema(Nullable = true)]
        public IFormFile? CategoryImage { get; set; }
    }
}
