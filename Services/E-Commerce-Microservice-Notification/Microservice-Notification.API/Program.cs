using Microservice_Notification.API.Endpoints;
using Microservice_Notification.Infrastructure.ApplicationContext;
using Microservice_Notification.Infrastructure.DependencyInjection;
using Microservice_Notifications.Application.DependencyInjection;
using Microservice_Notifications.Application.Features.Notification.Query.GetAllNotificationsQ;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterGenericHandlers = true;
    cfg.RegisterServicesFromAssemblyContaining<GetAllNotificationsHandler>();
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


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthentication();
app.MapControllers();
app.MapNotificationEndPoint();
app.Run();
