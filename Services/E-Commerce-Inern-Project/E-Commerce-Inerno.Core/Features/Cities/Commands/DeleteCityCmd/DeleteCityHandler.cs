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

namespace E_Commerce_Inerno.Core.Features.Cities.Commands.DeleteCityCmd
{
    public class DeleteCityHandler : IRequestHandler<DeleteCityRequest, Result<bool>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;
        private readonly IInternoEventPublisher _Publisher;
        

        public DeleteCityHandler(ICityRepository cityRepository, IDistributedCache cache, IUserRepository UserRepo, IInternoEventPublisher Publisher)
        {
            _cityRepository = cityRepository;
            _UserRepo = UserRepo;
            _Publisher = Publisher;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(DeleteCityRequest request, CancellationToken cancellationToken)
        {
            City? city = await _cityRepository.GetCity(request.CityID, true);
            if (city == null)
            {
                return Result<bool>.NotFound("City with the given id does not exist.");
            }
            string JsonOldValues = JsonSerializer.Serialize<City>(city);
            city.IsDeleted = true;
            if (!await _cityRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to delete city.");
            }

            string JsonNewValues = JsonSerializer.Serialize<City>(city);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Delete, nameof(City), JsonOldValues, JsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);

            string CityKey = $"City:{city.CityID}";
            await _cache.RemoveAsync(CityKey);
            return Result<bool>.Success(true);
        }

        private async Task<Guid> GetAdminID()
        {
            var admin = await _UserRepo.GetApplicationUserByEmail("admin@gmail.com", false);
            return admin.UserID;
        }
    }
}
