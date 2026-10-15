using FoodMapGIS.Domain.Entities;
using FoodMapGIS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace FoodMapGIS.Infrastructure.Database.Persistence;

public static class ApplicationDbContextSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {

        // 2. Nạp dữ liệu Danh mục món ăn (FR-CAT-01)
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new() { Name = "Phở", Description = "Phở truyền thống, phở tái, phở gà" },
                new() { Name = "Bún bò Huế", Description = "Bún bò Huế cay nồng, đậm vị" },
                new() { Name = "Cơm tấm", Description = "Cơm tấm sườn bì chả truyền thống Sài Gòn" },
                new() { Name = "Bánh mì", Description = "Bánh mì kẹp thịt, pate, chả cá" },
                new() { Name = "Bún chả", Description = "Bún chả nướng than hoa Hà Nội" },
                new() { Name = "Lẩu & Nướng", Description = "Lẩu thái, lẩu riêu cua, bò nướng" },
                new() { Name = "Cà phê", Description = "Cà phê sữa đá, bạc xỉu, cà phê muối" },
                new() { Name = "Trà sữa & Đồ uống", Description = "Trà sữa, trà trái cây nhiệt đới" },
                new() { Name = "Hải sản & Ốc", Description = "Hải sản tươi sống, các món ốc bình dân" },
                new() { Name = "Chè & Tráng miệng", Description = "Chè ba màu, chè bưởi, bánh ngọt" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        // 3. Nạp tài khoản Admin mặc định
        User? adminUser = await context.Users.FirstOrDefaultAsync(u => u.Role == UserRole.Admin);
        if (adminUser == null)
        {
            adminUser = new User
            {
                Username = "admin",
                Email = "admin@foodmap.vn",
                // Mật khẩu mặc định: Admin@123 (mã hóa chuẩn BCrypt)
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = UserRole.Admin,
                IsActive = true,
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }

        // 4. (Tùy chọn) Nạp 2 quán ăn mẫu có tọa độ GPS thật để test GIS
        if (!await context.FoodPlaces.AnyAsync())
        {
            var phoCategory = await context.Categories.FirstAsync(c => c.Name == "Phở");
            var comTamCategory = await context.Categories.FirstAsync(c => c.Name == "Cơm tấm");

            // Lưu ý tọa độ SRID 4326: X là Kinh độ (Longitude), Y là Vĩ độ (Latitude)
            var samplePlaces = new List<FoodPlace>
            {
                new()
                {
                    Name = "Phở Thìn Lò Đúc",
                    Address = "13 Lò Đúc, Ngô Thì Nhậm, Hai Bà Trưng, Hà Nội",
                    Geom = new Point(105.857351, 21.018783) { SRID = 4326 },
                    OwnerId = adminUser.Id,
                    CategoryId = phoCategory.Id,
                    Status = FoodPlaceStatus.Active,
                    Phone = "02439712345",
                    OpeningTime = new TimeOnly(6, 0),
                    ClosingTime = new TimeOnly(20, 30),
                    MinPrice = 65000,
                    MaxPrice = 90000,
                    AvgRating = 4.8,
                    TotalReviews = 1,
                    Description = "Quán phở bò xào lăn gia truyền nổi tiếng Hà Nội"
                },
                new()
                {
                    Name = "Cơm Tấm Ba Ghiền",
                    Address = "84 Đặng Văn Ngữ, Phường 10, Phú Nhuận, TP.HCM",
                    Geom = new Point(106.671389, 10.793333) { SRID = 4326 },
                    OwnerId = adminUser.Id,
                    CategoryId = comTamCategory.Id,
                    Status = FoodPlaceStatus.Active,
                    Phone = "02838461073",
                    OpeningTime = new TimeOnly(7, 0),
                    ClosingTime = new TimeOnly(21, 0),
                    MinPrice = 70000,
                    MaxPrice = 120000,
                    AvgRating = 4.6,
                    TotalReviews = 1,
                    Description = "Cơm tấm sườn nướng khổng lồ ngon trứ danh Sài Gòn"
                }
            };

            await context.FoodPlaces.AddRangeAsync(samplePlaces);
            await context.SaveChangesAsync();
        }
    }
}
