using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;
using Tripex.Core.Services;
using Tripex.Core.Services.Security;
using Tripex.Infrastructure.Persistence;
using Tripex.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUsersRepository, UserRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<ILikesService, LikesService>();
builder.Services.AddScoped<IFollowersService, FollowersService>();
builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));

var app = builder.Build();

app.MapControllers();

app.Run();
