using FoodMapGIS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Menu.Mapping
{
    public static class MenuMappingExtensions
    {
        // ================= CHIỀU ĐỌC (Entity -> DTO) =================
        /// Expression dùng cho IQueryable nếu cần Select trực tiếp từ Database
        public static Expression<Func<MenuItem, MenuItemDto>> EntityToDtoExpression =>
            m => new MenuItemDto
            {
                Id = m.Id,
                FoodPlaceId = m.FoodPlaceId,
                Name = m.Name,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                Group = m.Group,
                Description = m.Description,
                IsAvailable = m.IsAvailable
            };

        /// Ánh xạ 1 Entity MenuItem sang MenuItemDto
        public static MenuItemDto EntityToDto(this MenuItem m) =>
            new MenuItemDto
            {
                Id = m.Id,
                FoodPlaceId = m.FoodPlaceId,
                Name = m.Name,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                Group = m.Group,
                Description = m.Description,
                IsAvailable = m.IsAvailable
            };

        /// Gom nhóm danh sách MenuItem theo Group (ví dụ: "Món chính", "Đồ uống",...)
        public static List<MenuGroupDto> EntityListToGroupedMenuDto(this IEnumerable<MenuItem> items)
        {
            return items
                .GroupBy(m => string.IsNullOrWhiteSpace(m.Group) ? "Món khác" : m.Group.Trim())
                .Select(g => new MenuGroupDto
                {
                    GroupName = g.Key,
                    Items = g.Select(m => m.EntityToDto()).ToList()
                })
                .ToList();
        }
        // ================= CHIỀU GHI (DTO -> Entity) =================
        /// Tạo mới MenuItem Entity từ CreateMenuItemRequestDto
        public static MenuItem CreateDtoToEntity(this CreateMenuItemRequestDto dto, Guid foodPlaceId)
        {
            return new MenuItem
            {
                FoodPlaceId = foodPlaceId,
                Name = dto.Name.Trim(),
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Group = dto.Group?.Trim(),
                Description = dto.Description?.Trim(),
                IsAvailable = dto.IsAvailable
            };
        }

        /// Cập nhật dữ liệu từ UpdateMenuItemRequestDto vào Entity đã có
        public static void UpdateEntityFromDto(this UpdateMenuItemRequestDto dto, MenuItem entity)
        {
            entity.Name = dto.Name.Trim();
            entity.Price = dto.Price;
            entity.ImageUrl = dto.ImageUrl;
            entity.Group = dto.Group?.Trim();
            entity.Description = dto.Description?.Trim();
            entity.IsAvailable = dto.IsAvailable;
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
