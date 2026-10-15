using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FoodMapGIS.Usecases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Dki Category
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFoodPlaceService, FoodPlaceService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMenuService, MenuService>();
            return services;
        }
    }
}
