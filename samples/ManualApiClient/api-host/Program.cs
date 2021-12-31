var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var app = builder.Build();

app.MapGet("", () => { });

app.Run();
