using E_Commerce_Inern_Project.Core.Features.Categories.Commands.CreateCategoryCommad;
using E_Commerce_Inern_Project.Core.Features.Categories.Commands.DeleteCategoryCommand;
using E_Commerce_Inerno.Core.Features.Categories.Commands.UpdateCategoryCommand;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.CategoryControl.Commands
{
    [Authorize(Roles = "Admin")]
    [Route("api/Category")]
    [ApiController]
    public class CategoryCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<Result<CategoryResponse>> CreateNewCategory([FromForm] CategoryRequest Request)
        {
            return await _mediator.Send(Request);
        }

        [HttpPut("[action]/{CategoryID}")]
        public async Task<Result<bool>> DeleteCategory(Guid CategoryID)
        {
            return await _mediator.Send(new DeleteCategoryCmd(CategoryID));
        }

        [HttpPut("[action]")]
        public async Task<Result<CategoryResponse>> UpdateCategory([FromForm] UpdateCategoryRequest Request)
        {
            return await _mediator.Send(Request);
        }
    }
}
