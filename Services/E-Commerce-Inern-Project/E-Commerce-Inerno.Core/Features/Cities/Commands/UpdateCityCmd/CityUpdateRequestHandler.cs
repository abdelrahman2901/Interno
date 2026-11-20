using AutoMapper;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.ICityRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Cities.Commands.UpdateCityCmd
{
    public class CityUpdateRequestHandler : IRequestHandler<CityUpdateRequest, Result<bool>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;
        private readonly IInternoEventPublisher _Publisher;
        

        public CityUpdateRequestHandler(ICityRepository cityRepository, IDistributedCache cache, IUserRepository UserRepo, IInternoEventPublisher Publisher, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _UserRepo = UserRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(CityUpdateRequest request, CancellationToken cancellationToken)
        {
            City? city = await _cityRepository.GetCity(request.CityID, true);
            if (city == null)
            {
                return Result<bool>.NotFound("City with the given id does not exist.");
            }

            string JsonOldValues = JsonSerializer.Serialize<City>(city);

            _mapper.Map(request, city);
            if (!await _cityRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to update city.");
            }

            string JsonNewValues = JsonSerializer.Serialize<City>(city);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Update, nameof(City), JsonOldValues, JsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);

            string CityKey = $"City:{city.CityID}";
            await _cache.RemoveAsync(CityKey);
            return Result<bool>.Success(city.IsDeleted);
        }

        private async Task<Guid> GetAdminID()
        {
            var admin = await _UserRepo.GetApplicationUserByEmail("admin@gmail.com", false);
            return admin.UserID;
        }
    }
}
