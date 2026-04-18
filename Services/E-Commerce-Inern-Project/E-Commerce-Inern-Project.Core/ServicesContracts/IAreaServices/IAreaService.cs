using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;
using E_Commerce_Inern_Project.Core.Features.Area.Commands.CreateAreaCmd;
using E_Commerce_Inern_Project.Core.Features.Area.Commands.UpdateAreaCmd;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IAreaServices
{
    public interface IAreaService
    {
        public Task<Result<IEnumerable<AreaResponse>>> GetAllAreas();
        public Task<Result<AreaResponse>> GetArea(Guid id);
        public Task<Result<bool>> UpdateArea(AreaUpdateRequest AreaRequest);
        public Task<Result<AreaResponse>> AddArea(AreaRequest AreaRequest);
        public Task<Result<bool>> DeleteArea(Guid id);
    }
}
