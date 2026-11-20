using E_Commerce_Inerno.Core.RepositoryContracts.ISizeRepo;
using E_Commerce_Interno.Domain.Common;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace E_Commerce_Inerno.Core.Features.SIze.Commands.DeleteSizeCommand
{
    public class DeleteSizeHandler : IRequestHandler<DeleteSizeCmd, Result<bool>>
    {
        private readonly ISizeRepository _SizeRepo;
        private readonly IDistributedCache _cache;

        public DeleteSizeHandler(ISizeRepository sizeRepo, IDistributedCache cache)
        {
            _SizeRepo = sizeRepo;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(DeleteSizeCmd request, CancellationToken cancellationToken)
        {
            var size = await _SizeRepo.GetSize(request.SizeID);
            if (size == null)
            {
                return Result<bool>.NotFound("No sizes found.");
            }
            size.IsDeleted = true;

            if (!await _SizeRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to save changes.");
            }

            await _cache.RemoveAsync($"Size:{request.SizeID}");
            return Result<bool>.Success(true);
        }
    }
}
