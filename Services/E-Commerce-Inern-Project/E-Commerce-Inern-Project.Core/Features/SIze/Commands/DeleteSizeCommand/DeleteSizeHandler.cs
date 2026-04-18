using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.ISizeServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.SIze.Commands.DeleteSizeCommand
{
    public class DeleteSizeHandler : IRequestHandler<DeleteSizeCmd, Result<bool>>
    {
        private readonly ISizeService _sizeService;
        public DeleteSizeHandler(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        public async Task<Result<bool>> Handle(DeleteSizeCmd request, CancellationToken cancellationToken)
        {
            return await _sizeService.DeleteSize(request.SizeID);
        }
    }

}
