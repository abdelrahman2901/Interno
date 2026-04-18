using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using E_Commerce_Inern_Project.Core.Features.Category.Commands.CreateCategoryCommad;
using E_Commerce_Inern_Project.Core.Features.Category.Commands.DeleteCategoryCommand;
using E_Commerce_Inern_Project.Core.Features.Category.Commands.UpdateCategoryCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.CategoryControl
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<Result<CategoryResponse>> CreateNewCategory([FromForm]CategoryRequest Request)
        {
            return await _mediator.Send(Request);
        }
        [HttpPut("DeleteCategory/{CategoryID}")]
        public async Task<Result<bool>> DeleteCategory(Guid CategoryID)
        {
            return await _mediator.Send(new DeleteCategoryCmd(CategoryID));
        }
        [HttpPut("UpdateCategory")]
        public async Task<Result<CategoryResponse>> UpdateCategory([FromForm]UpdateCategoryRequest Request)
        {
            return await _mediator.Send(Request);
        }
    }
}
