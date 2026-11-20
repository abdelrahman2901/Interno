using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.ISizeRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.SizeDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inerno.Core.Features.SIze.Commands.CreateSizeCommand
{
    public class CreateSizeHandler : IRequestHandler<SizeRequest, Result<bool>>
    {
        private readonly ISizeRepository _SizeRepo;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public CreateSizeHandler(ISizeRepository sizeRepo, IDistributedCache cache, IMapper mapper)
        {
            _SizeRepo = sizeRepo;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(SizeRequest request, CancellationToken cancellationToken)
        {
            Size NewSize = _mapper.Map<Size>(request);
            bool result = await _SizeRepo.CreateSize(NewSize);
            if (!result)
            {
                return Result<bool>.InternalError("Failed to create size.");
            }
            if (!await _SizeRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to save changes.");
            }

            //Inserting value to Caching
            string SizeKey = $"Size:{NewSize.SizeID}";
            var json = JsonSerializer.Serialize(_mapper.Map<SizeResponse>(NewSize));
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(SizeKey, json, options);

            return Result<bool>.Success(true);
        }
    }
}
