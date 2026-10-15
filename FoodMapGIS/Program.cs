
using FoodMapGIS.Infrastructure;
using FoodMapGIS.Infrastructure.Database.Persistence;
using FoodMapGIS.Usecases;



namespace FoodMapGIS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            //Cau hinh xac thuc JWT
            var jwtSecret = builder.Configuration["JwtSettings:Secret"]!;
            

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization(); 

            app.MapControllers();

            // 👇 THÊM ĐOẠN NÀY ĐỂ KÍCH HOẠT SEEDER:
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await ApplicationDbContextSeeder.SeedAsync(context);
            }

            await app.RunAsync();
        }
    }
}
