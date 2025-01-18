using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;
using Tripex.Core;
using Tripex.Core.Services;
using Tripex.Core.Services.Security;
using Tripex.Infrastructure.Persistence;
using Tripex.Infrastructure.Persistence.Repositories;
using Tripex.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis")!;
    return ConnectionMultiplexer.Connect(configuration);
});


builder.Services.AddHttpContextAccessor();

#region DI
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICookiesService, CookiesService>();
builder.Services.AddScoped<IUsersRepository, UserRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped(typeof(ILikesService<>), typeof(LikesService<>));
builder.Services.AddScoped<IFollowersService, FollowersService>();
builder.Services.AddScoped<IS3FileService, S3FileService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICensorService, CensorService>();
builder.Services.AddScoped<IRedisRepository, RedisRepository>();

builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));
#endregion

builder.Services.AddHttpClient<CensorService>();

var awsOptions = builder.Configuration.GetSection("AWS").Get<AwsOptions>()!;
builder.Services.AddSingleton<IAmazonS3>(sp =>
    new AmazonS3Client(awsOptions.AccessKey, awsOptions.SecretKey, Amazon.RegionEndpoint.GetBySystemName(awsOptions.Region)));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var cookiesManager = context.HttpContext.RequestServices.GetRequiredService<ICookiesService>();
                context.Token = cookiesManager.GetFromCookie(jwtOptions.TokenName);
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
