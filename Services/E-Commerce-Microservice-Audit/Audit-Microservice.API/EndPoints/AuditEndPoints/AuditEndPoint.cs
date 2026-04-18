using MediatR;
using Microservice_Audit.Application.Features.Audit.Query.GetAllAuditQ;

namespace Audit_Microservice.API.EndPoints.AuditEndPoints
{
    public static class AuditEndPoint 
    {

        public static void MapAuditEndPoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/Audit/GetAllAudit", async (IMediator _Medaitor) =>
            {
                return await _Medaitor.Send(new GetAllAuditQuery());

            });

            //app.MapPost("/api/CreateAudit", async (IMediator _Medaitor, CreateAuditRequest NewNotification) =>
            //{
            //    return await _Medaitor.Send(NewNotification);
            //});
        }
    }
}
