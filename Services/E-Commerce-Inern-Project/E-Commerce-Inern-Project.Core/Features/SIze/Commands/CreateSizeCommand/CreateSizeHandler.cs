using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.SizeDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.ISizeServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.SIze.Commands.CreateSizeCommand
{
    public class CreateSizeHandler : IRequestHandler<SizeRequest, Result<SizeResponse>>
    {
        private readonly ISizeService _sizeService;
        public CreateSizeHandler(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        public async Task<Result<SizeResponse?>> Handle(SizeRequest request, CancellationToken cancellationToken)
        {
            return await _sizeService.CreateSize(request);
        }
    }
}
