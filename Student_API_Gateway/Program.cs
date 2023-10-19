using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

builder.Configuration.AddJsonFile("config.json", optional : false, reloadOnChange : true);
builder.Services.AddOcelot(builder.Configuration);
app.MapControllers();
await app.UseOcelot();

app.Run();
