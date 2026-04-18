using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.SizeDTO;
using E_Commerce_Inern_Project.Core.Features.SIze.Commands.CreateSizeCommand;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.ISizeServices
{
    public interface ISizeService
    {
        public Task<Result<SizeResponse?>> CreateSize(SizeRequest size);
        public Task<Result<bool>> DeleteSize(Guid sizeID);
        public Task<Result<IEnumerable<SizeResponse>>> GetAllSizes();
    }
}
