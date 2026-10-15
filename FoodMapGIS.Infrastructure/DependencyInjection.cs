using FoodMapGIS.Application.Common.Interface.Infrastructure;
using FoodMapGIS.Application.Common.Interface.Repositories;
using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Infrastructure.Database.Persistence;
using FoodMapGIS.Infrastructure.Database.Repositories;
using FoodMapGIS.Infrastructure.Security;
using FoodMapGIS.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FoodMapGIS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Cau hinh DBContext + Postgres
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    // Bat ho tro du lieu ban do NetTopologySuit
                    npgsqlOptions.UseNetTopologySuite();
                })
                // Tự động chuyển tên bảng, tên cột sang dạng snake_case (food_places, created_at...)
                .UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddMemoryCache();

            // Dang ki dependency cho Security service
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            // Cau hinh option pattern
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));


            //Cau hinh Jwt Bearer
            var jwtSettings = new JwtSettings();
            configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

            return services;
        }
    }
}
