using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IColorServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Color.Command.DeleteColorCmd
{
    public class DeleteColorHandler : IRequestHandler<DeleteColorRequest, Result<bool>>
    {
        private readonly IColorService _colorService;
        public DeleteColorHandler(IColorService colorService)
        {
            _colorService = colorService;
        }
        async Task<Result<bool>> IRequestHandler<DeleteColorRequest, Result<bool>>.Handle(DeleteColorRequest request, CancellationToken cancellationToken)
        {
            return await _colorService.DeleteColor(request.ColorID);
        }
    }
}
