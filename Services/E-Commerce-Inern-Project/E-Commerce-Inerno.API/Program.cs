using E_Commerce_Inerno.Core.Communication.HttpClients;
using E_Commerce_Inerno.Core.DependencyInjection;
using E_Commerce_Inerno.Core.Policies;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Inerno.Infrastructure.DependencyInjection;
using E_Commerce_Inerno.Infrastructure.Repository.PaymentRepo.PaymobProviderService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Polly;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var runningInDocker =
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
builder.Services.AddControllers().AddJsonOptions(cfg =>
{
    cfg.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"RunningInDocker: {runningInDocker}");

builder.Services.AddCore(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowOrigins").Get<string[]>();

        foreach (var orign in allowedOrigins)
        {
            Console.WriteLine(orign);
        }
        policy.WithOrigins(allowedOrigins)
          .WithHeaders("Authorization", "origin", "accept", "content-type", "Accept-Language")
          .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
          .AllowCredentials();
    });
});

// HttpClients for local HTTPS dual-boot (Audit / Notification microservices)
builder.Services.AddHttpClient<CreateAuditClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuditService:BaseUrl"]!);
}).
AddPolicyHandler((serviceProvider, request) =>
{
    var policy = serviceProvider
        .GetRequiredService<IPollyPolicy>().GetPolicy();

    return policy.AsAsyncPolicy<HttpResponseMessage>();
});

builder.Services.AddHttpClient<PaymobHttpClient>()
   .AddPolicyHandler((serviceProvider, request) =>
{
    var policy = serviceProvider
        .GetRequiredService<IPollyPolicy>().GetPolicy();

    return policy.AsAsyncPolicy<HttpResponseMessage>();
});

builder.Services.AddHttpClient<CreateNotificationClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["NotificationsService:BaseUrl"]!);
})
    .AddPolicyHandler((serviceProvider, request) =>
{
    var policy = serviceProvider
        .GetRequiredService<IPollyPolicy>().GetPolicy();

    return policy.AsAsyncPolicy<HttpResponseMessage>();
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter Your JWT Token"
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
    c.EnableAnnotations();
});


//RateLimit

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Auth", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 500,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.AddFixedWindowLimiter("strict", opt =>
    {
        opt.PermitLimit = 3;
        opt.Window = TimeSpan.FromMinutes(5);
        opt.QueueLimit = 0;
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            message = "Too many requests. Please try again later."
        }, cancellationToken: token);
    };
});

//authentication

builder.Services.AddAuthentication(JwtBuilder =>
{
    JwtBuilder.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    JwtBuilder.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateLifetime = true,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
    .AddPolicy("UserOnly", policy => policy.RequireRole("User"));

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    db.Database.Migrate();
}

app.UseForwardedHeaders();

if (!runningInDocker)
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();
