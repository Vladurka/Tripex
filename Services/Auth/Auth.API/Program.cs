using System.Reflection;
using Auth.API.Data;
using Auth.API.Services;
using BuildingBlocks.Auth;
using BuildingBlocks.Behaviors;
using BuildingBlocks.CORS;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddDbContext<AuthContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICookiesService, CookiesService>();
builder.Services.AddScoped<IUsersRepository, UserRepository>();

builder.Services.AddOutboxPattern<AuthContext>();

builder.Services.AddCarter();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());

builder.Services.AddAuth(builder.Configuration);

builder.Services.AllowFrontend();

var app = builder.Build();

app.UseAuth();

app.MapCarter();
app.UseExceptionHandler(opts => { });
app.UseHealthChecks("/health");

app.UseCors("AllowFrontend");

app.Run();