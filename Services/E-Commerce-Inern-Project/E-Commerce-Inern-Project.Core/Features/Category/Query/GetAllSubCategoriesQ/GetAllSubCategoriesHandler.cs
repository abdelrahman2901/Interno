using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using E_Commerce_Inern_Project.Core.Features.Category.Query.GetAllSubCategoriesQ;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICategoryServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Category.Query.GetAllsUBCategoriesQ
{
    public class GetAllsUBCategoriesHandler : IRequestHandler<GetAllSubCategoriesQuery, Result<IEnumerable<SubCategoryResponse>>>
    {
        private readonly ICategoryService _CatSerice;

        public GetAllsUBCategoriesHandler(ICategoryService catSerice)
        {
            _CatSerice = catSerice;
        }
        async Task<Result<IEnumerable<SubCategoryResponse>>> IRequestHandler<GetAllSubCategoriesQuery, Result<IEnumerable<SubCategoryResponse>>>.Handle(GetAllSubCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _CatSerice.GetCategoriesWithSubCat();
        }
    }
}
