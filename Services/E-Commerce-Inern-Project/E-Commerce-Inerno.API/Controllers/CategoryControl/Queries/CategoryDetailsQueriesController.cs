using E_Commerce_Inerno.Core.Features.Categories.Query.GetAllSubCategoriesQ;
using E_Commerce_Inerno.Core.Features.Categories.Query.GetCategoryByIDQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.CategoryControl.Queries
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
