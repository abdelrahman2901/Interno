using AutoMapper;
using Microservice_Audit.Application.Features.Audit.Command.CreateAuditCmd;
using Microservice_Audit.Core.DTO.AuditDTO;
using Microservice_Audit.Domain.Entity;

namespace Microservice_Audit.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Audit, AuditResponse>();
            CreateMap<CreateAuditRequest, Audit>()
                .ForMember(dest=>dest.AuditID,opt=>Guid.NewGuid())
                .ForMember(dest=>dest.CreatedAt,opt=>opt.MapFrom(src=>DateTime.UtcNow));
        }
    }
}
