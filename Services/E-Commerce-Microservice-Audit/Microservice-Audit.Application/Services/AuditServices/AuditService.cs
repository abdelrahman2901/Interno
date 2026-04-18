using AutoMapper;
using Microservice_Audit.Application.Features.Audit.Command.CreateAuditCmd;
using Microservice_Audit.Core.Common;
using Microservice_Audit.Core.DTO.AuditDTO;
using Microservice_Audit.Core.ServicesContracts.IAuditsServices;
using Microservice_Audit.Domain.Entity;
using Microservice_Audit.Domain.RepositoryContracts.IAuditsRepo;

namespace Microservice_Audit.Application.Services.AuditServices
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _AuditsRepo;
        private readonly IMapper _Mapper;
        public AuditService(IAuditRepository AuditsRepo, IMapper mapper)
        {
            _AuditsRepo = AuditsRepo;
            _Mapper = mapper;
        }

        public async Task<Result<bool>> CreateAudit(CreateAuditRequest NewAudit)
        {
            var NewNotifeResult = await _AuditsRepo.CreateAudit(_Mapper.Map<Audit>(NewAudit));
            if (!NewNotifeResult)
            {
                return Result<bool>.InternalError("Failed To Add Audit");
            }
            if (!await _AuditsRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChnages");
            }
            return Result<bool>.Success(NewNotifeResult);
        }

        public async Task<Result<IEnumerable<AuditResponse>>> GetAudits()
        {
            var NotifesResult = await _AuditsRepo.GetAudits();
            if (!NotifesResult.Any())
            {
                return Result<IEnumerable<AuditResponse>>.InternalError("No Audits Was Found");
            }

            return Result<IEnumerable<AuditResponse>>.Success(NotifesResult.Select(s => _Mapper.Map<AuditResponse>(s)));
        }



    }
}
