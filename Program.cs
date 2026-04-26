using DotNetEnv;
using MyApi.Bootstrap;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructure();
builder.AddSecurity();
builder.AddGameServices();

var app = builder.Build();

app.UseInfrastructure();
app.UseGameEndpoints();

app.Run();