using E_Commerce_Inern_Project.Core.DependencyInjection;
using E_Commerce_Inern_Project.Core.Identity;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using E_Commerce_Inern_Project.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(cfg =>
{
    cfg.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
 Console.WriteLine(builder.Environment.EnvironmentName);

builder.Services.AddCore(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);



//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowOrigins").Get<string[]>();
        foreach(var origin in allowedOrigins)
        {
            Console.WriteLine(origin);
        }
        policy.WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
{
    option.Password.RequiredLength = 5;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireDigit = true;
    option.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();


//RateLimit

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Auth", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });

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

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseHttpsRedirection();
app.UseHsts();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();
