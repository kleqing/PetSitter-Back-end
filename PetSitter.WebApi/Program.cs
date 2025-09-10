using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetSitter.DataAccess;
using PetSitter.DataAccess.Repository.Implements;
using PetSitter.DataAccess.Repository.Interfaces;
using PetSitter.DataAccess.Services.Implements;
using PetSitter.DataAccess.Services.Interfaces;
using PetSitter.DataAccess.Services.Role;
using PetSitter.Models.Models;

namespace PetSitter.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddScoped<IAuthServices, AuthServices>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        
        builder.Services.AddIdentity<Users, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy =
                    JsonNamingPolicy.CamelCase; //* Use original property names
                options.JsonSerializerOptions.PropertyNameCaseInsensitive =
                    true; //* Enable case-insensitive property names
            });
        
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await RoleService.SeedRole(roleManager);
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        
        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}
