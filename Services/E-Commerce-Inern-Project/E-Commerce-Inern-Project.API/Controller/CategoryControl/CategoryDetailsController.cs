using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using E_Commerce_Inern_Project.Core.Features.Category.Query.GetAllSubCategoriesQ;
using E_Commerce_Inern_Project.Core.Features.Category.Query.GetCategoryByIDQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.CategoryControl
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryDetailsController(IMediator mediator)
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
