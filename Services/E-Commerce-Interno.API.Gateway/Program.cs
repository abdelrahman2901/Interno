using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.HttpOverrides;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var runningInDocker =
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

// Env vars win. Docker uses the compose service. Production reads appsettings.Production.json
// (https://internoapi.runasp.net:443). Local Development stays on the main API dev cert.
var downstreamHost = Environment.GetEnvironmentVariable("DOWNSTREAM_HOST");
var downstreamPortText = Environment.GetEnvironmentVariable("DOWNSTREAM_PORT");
var downstreamScheme = Environment.GetEnvironmentVariable("DOWNSTREAM_SCHEME");

if (runningInDocker)
{
    downstreamHost ??= "InternoMicroservice.api";
    downstreamPortText ??= "8080";
    downstreamScheme ??= "http";
}
else
{
    downstreamHost ??= builder.Configuration["Downstream:Host"] ?? "localhost";
    downstreamPortText ??= builder.Configuration["Downstream:Port"] ?? "7164";
    downstreamScheme ??= builder.Configuration["Downstream:Scheme"] ?? "https";
}

if (!int.TryParse(downstreamPortText, out var downstreamPort))
{
    throw new InvalidOperationException(
        $"DOWNSTREAM_PORT '{downstreamPortText}' is not a valid port.");
}

var useHttps = downstreamScheme.Equals("https", StringComparison.OrdinalIgnoreCase);
var trustDevCertificate = useHttps
    && downstreamHost.Equals("localhost", StringComparison.OrdinalIgnoreCase);

var ocelotPath = Path.Combine(builder.Environment.ContentRootPath, "ocelot.json");
var ocelot = JsonNode.Parse(await File.ReadAllTextAsync(ocelotPath))!.AsObject();

foreach (var routeNode in ocelot["Routes"]!.AsArray())
{
    var route = routeNode!.AsObject();

    route["DownstreamHostAndPorts"] = new JsonArray
    {
        new JsonObject
        {
            ["Host"] = downstreamHost,
            ["Port"] = downstreamPort
        }
    };

    route["DownstreamScheme"] = downstreamScheme;

    if (trustDevCertificate)
    {
        route["DangerousAcceptAnyServerCertificateValidator"] = true;
    }
    else
    {
        route.Remove("DangerousAcceptAnyServerCertificateValidator");
    }
}

var gatewayBaseUrl = Environment.GetEnvironmentVariable("GATEWAY_BASE_URL");
if (string.IsNullOrWhiteSpace(gatewayBaseUrl))
{
    gatewayBaseUrl = runningInDocker
        ? "http://localhost:5003"
        : builder.Configuration["Gateway:BaseUrl"] ?? "https://localhost:7110";
}

ocelot["GlobalConfiguration"] ??= new JsonObject();
ocelot["GlobalConfiguration"]!["BaseUrl"] = gatewayBaseUrl;

using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(ocelot.ToJsonString())))
{
    builder.Configuration.AddJsonStream(stream);
}

Console.WriteLine(
    $"Ocelot downstream: {downstreamScheme}://{downstreamHost}:{downstreamPort}");

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var allowedOrigins = builder.Configuration.GetSection("AllowOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOcelot(builder.Configuration).AddQualityOfService();
var app = builder.Build();

app.UseForwardedHeaders();

if (!runningInDocker)
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseCors();

await app.UseOcelot();
app.Run();
