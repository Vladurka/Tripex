using Profiles.API;
using Profiles.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

app.UseApiServices();

app.Run();