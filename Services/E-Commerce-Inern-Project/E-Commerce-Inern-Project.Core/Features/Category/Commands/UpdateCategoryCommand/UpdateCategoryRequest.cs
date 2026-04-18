using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce_Inern_Project.Core.Features.Category.Commands.UpdateCategoryCommand
{
    public class UpdateCategoryRequest : IRequest<Result<CategoryResponse>>
    {
       public Guid? CategoryID { get; set; }
         public string? CategoryName { get; set; }
        public Guid? ParentCategoryID { get; set; }
        public IFormFile? CategoryImage{ get; set; }
    }
}
