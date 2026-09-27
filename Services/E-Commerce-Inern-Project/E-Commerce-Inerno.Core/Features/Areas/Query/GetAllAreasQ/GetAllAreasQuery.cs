using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Areas.Query.GetAllAreasQ
{
    public record GetAllAreasQuery : IRequest<Result<IEnumerable<AreaResponse>>>;
}
