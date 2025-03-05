using Auth.API;
using Auth.API.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AuthContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>(opts =>
    {
        opts.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AuthContext>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.MapIdentityApi<AppUser>();
    
app.Run();