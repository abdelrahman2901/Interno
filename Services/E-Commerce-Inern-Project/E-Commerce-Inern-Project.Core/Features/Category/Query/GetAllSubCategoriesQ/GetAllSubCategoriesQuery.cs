using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Category.Query.GetAllSubCategoriesQ
{
 public   record GetAllSubCategoriesQuery : IRequest<Result<IEnumerable<SubCategoryResponse>>>;
}
