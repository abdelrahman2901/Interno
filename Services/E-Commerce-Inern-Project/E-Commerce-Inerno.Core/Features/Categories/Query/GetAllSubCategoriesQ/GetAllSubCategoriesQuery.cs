using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Categories.Query.GetAllSubCategoriesQ
{
    public record GetAllSubCategoriesQuery : IRequest<Result<IEnumerable<SubCategoryResponse>>>;
}
