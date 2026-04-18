using Audit_Microservice.API.EndPoints.AuditEndPoints;
using Microservice_Audit.Application.Features.Audit.Query.GetAllAuditQ;
using Microservice_Audit.Infrastructure.DependencyInjection;
using Microservice_Audit.Application.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterGenericHandlers = true;
    cfg.RegisterServicesFromAssemblyContaining<GetAllAuditHandler>();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowOrigins").Get<string[]>())
        .AllowAnyHeader()
        .WithHeaders(["Authorization"])
        .AllowAnyMethod();
    });
});
builder.Services.AddSwaggerGen();


//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer(options =>
//    {
//        options.Authority = "http://localhost:5099";
//        options.Audience = "https://localhost:7054";
//    });

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthentication();
app.MapControllers();
app.MapAuditEndPoint();
app.Run();
