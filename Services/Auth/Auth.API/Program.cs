using System.Reflection;
using Auth.API.Data;
using Auth.API.Exceptions;
using Auth.API.Services;
using BuildingBlocks.Auth;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddControllers();

builder.Services.AddDbContext<AuthContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICookiesService, CookiesService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUsersRepository, UserRepository>();

builder.Services.AddOutboxPattern<AuthContext>();

builder.Services.AddAuth(builder.Configuration);
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuth();

app.MapControllers();

app.Run();