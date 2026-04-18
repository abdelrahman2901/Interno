using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.SizeDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.ISizeServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.SIze.Query.GetSizesQ
{
    public class GetSizeHandler : IRequestHandler<GetSizesQuery, Result<IEnumerable<SizeResponse>>>
    {
        private readonly ISizeService _sizeService;
        public GetSizeHandler(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        public async Task<Result<IEnumerable<SizeResponse>>> Handle(GetSizesQuery request, CancellationToken cancellationToken)
        {
            return await _sizeService.GetAllSizes();
        }
    }
}
