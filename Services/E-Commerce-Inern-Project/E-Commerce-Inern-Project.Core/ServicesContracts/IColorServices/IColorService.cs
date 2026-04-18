using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ColorDTO;
using E_Commerce_Inern_Project.Core.Features.Color.Command.CreateColorCmd;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IColorServices
{
    public interface IColorService
    {
         public Task<Result<ColorResponse>> CreateColor(ColorRequest request);
         public Task<Result<bool>> DeleteColor(Guid ColorID);
         public Task<Result<IEnumerable<ColorResponse>>> GetAllColors();
    }
}
