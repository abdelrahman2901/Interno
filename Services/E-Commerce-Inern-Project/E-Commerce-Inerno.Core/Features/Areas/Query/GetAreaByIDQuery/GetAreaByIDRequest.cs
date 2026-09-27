using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Areas.Query.GetAreaByIDQuery
{
    public record GetAreaByIDRequest(Guid AreaID) : IRequest<Result<AreaResponse>>;
}
