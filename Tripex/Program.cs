using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Security;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Services;
using Tripex.Core.Services.Security;
using Tripex.Infrastructure.Persistence;
using Tripex.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));

var app = builder.Build();

app.MapControllers();

app.Run();
