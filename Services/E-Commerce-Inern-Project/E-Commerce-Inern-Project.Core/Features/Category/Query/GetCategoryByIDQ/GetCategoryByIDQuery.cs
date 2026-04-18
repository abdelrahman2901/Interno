using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Category.Query.GetCategoryByIDQ
{
    public record GetCategoryByIDQuery(Guid CatID) : IRequest<Result<CategoryResponse>>;
}
