using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var runningInDocker =
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

// Docker uses ocelot.json (container DNS names). Local HTTPS uses ocelot.Development.json (localhost).
var ocelotFile = runningInDocker ? "ocelot.json" : "ocelot.Development.json";
builder.Configuration.AddJsonFile(ocelotFile, optional: false, reloadOnChange: true);
Console.WriteLine($"Ocelot config: {ocelotFile}");

builder.Services.AddOcelot().AddQualityOfService();
var app = builder.Build();

await app.UseOcelot();
app.Run();
