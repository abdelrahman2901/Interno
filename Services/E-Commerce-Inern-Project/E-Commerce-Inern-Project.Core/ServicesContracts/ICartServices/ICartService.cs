using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartDTO;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.ICartServices
{
    public interface ICartService
    {
        public Task<Result<CartResponse>> CreateCart(Guid UserID);
        public   Task<Result<CartDetailsResponse>> GetUserCarItemsQuery(Guid UserID);

    }
}
