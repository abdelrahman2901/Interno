using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICategoryServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Category.Query.GetCategoryByIDQ
{
    public class GetCategoryByIDHandler : IRequestHandler<GetCategoryByIDQuery, Result<CategoryResponse>>
    {
        private readonly ICategoryService _CatSerice;

        public GetCategoryByIDHandler(ICategoryService catSerice)
        {
            _CatSerice = catSerice;
        }
        public async Task<Result<CategoryResponse?>> Handle(GetCategoryByIDQuery request, CancellationToken cancellationToken)
        {
            return await _CatSerice.GetCategoryByID(request.CatID);
        }
    }
}
