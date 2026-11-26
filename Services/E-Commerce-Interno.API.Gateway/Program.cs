using System.Text;
using System.Text.Json.Nodes;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var runningInDocker =
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

// ocelot.json lists both downstreams on every route:
//   Docker  -> service DNS on port 8080, http
//   HTTPS   -> localhost on the Kestrel HTTPS port
// One route can only use one scheme, so keep the host that matches this process.
var ocelotPath = Path.Combine(builder.Environment.ContentRootPath, "ocelot.json");
var ocelot = JsonNode.Parse(await File.ReadAllTextAsync(ocelotPath))!.AsObject();

foreach (var routeNode in ocelot["Routes"]!.AsArray())
{
    var route = routeNode!.AsObject();
    var kept = new JsonArray();

    foreach (var hostNode in route["DownstreamHostAndPorts"]!.AsArray())
    {
        var host = hostNode!["Host"]!.GetValue<string>();
        var isLocalHost = host.Equals("localhost", StringComparison.OrdinalIgnoreCase);

        if (runningInDocker && !isLocalHost)
        {
                kept.Add(hostNode.DeepClone());
        }
        else if (isLocalHost)
        {
            kept.Add(hostNode.DeepClone());
        }
    }

    if (kept.Count == 0)
    {
        var upstream = route["UpstreamPathTemplate"]?.GetValue<string>() ?? "(unknown route)";
        throw new InvalidOperationException(
            $"Route '{upstream}' has no downstream host for {(runningInDocker ? "Docker" : "local HTTPS")}.");
    }

    route["DownstreamHostAndPorts"] = kept;

    if (runningInDocker)
    {
        route.Remove("DownstreamScheme");
        route.Remove("DangerousAcceptAnyServerCertificateValidator");
    }
    else
    {
        route["DownstreamScheme"] = "https";
        route["DangerousAcceptAnyServerCertificateValidator"] = true;
    }
}

ocelot["GlobalConfiguration"] ??= new JsonObject();
ocelot["GlobalConfiguration"]!["BaseUrl"] = runningInDocker
    ? "http://localhost:5003"
    : "https://localhost:7110";

using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(ocelot.ToJsonString())))
{
    builder.Configuration.AddJsonStream(stream);
}

Console.WriteLine(runningInDocker
    ? "Ocelot config: Docker hosts on port 8080 (http)"
    : "Ocelot config: localhost HTTPS ports");

builder.Services.AddOcelot(builder.Configuration).AddQualityOfService();
var app = builder.Build();

await app.UseOcelot();
app.Run();
