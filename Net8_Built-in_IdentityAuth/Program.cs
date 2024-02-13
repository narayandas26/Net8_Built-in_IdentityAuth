using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite("DataSource=BuiltInIdentityAuth.db"));
builder.Services.AddIdentityCore<MyIdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

var app = builder.Build();

app.MapIdentityApi<MyIdentityUser>();

app.MapGet("/", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}")
    .RequireAuthorization();

app.Run();


class MyIdentityUser : IdentityUser { }

class AppDbContext : IdentityDbContext<MyIdentityUser> 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
            
    }
}