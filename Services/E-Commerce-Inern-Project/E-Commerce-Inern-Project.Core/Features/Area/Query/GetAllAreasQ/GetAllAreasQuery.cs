using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Area.Query.GetAllAreasQ
{
    public record GetAllAreasQuery : IRequest<Result<IEnumerable<AreaResponse>>>;
}
