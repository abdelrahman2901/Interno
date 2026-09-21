using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using E_Commerce_Inern_Project.Core.Features.Category.Query.GetAllSubCategoriesQ;
using E_Commerce_Inern_Project.Core.Features.Category.Query.GetCategoryByIDQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.CategoryControl.Queries
{
    [AllowAnonymous]
    [Route("api/CategoryDetails")]
    [ApiController]
    public class CategoryDetailsQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryDetailsQueriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetCategoriesWithSubCat")]
        public async Task<Result<IEnumerable<SubCategoryResponse>>> GetCategoriesWithSubCat()
        {
            return await _mediator.Send(new GetAllSubCategoriesQuery());
        }

        [HttpGet("{CategoryID}")]
        public async Task<Result<CategoryResponse>> GetCategoryByID(Guid CategoryID)
        {
            return await _mediator.Send(new GetCategoryByIDQuery(CategoryID));
        }
    }
}
