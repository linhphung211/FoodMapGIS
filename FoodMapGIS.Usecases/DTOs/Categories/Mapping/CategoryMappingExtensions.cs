using FoodMapGIS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Categories.Mapping
{
    public static class CategoryMappingExtensions
    {
        // ================= CHIỀU ĐỌC (Entity -> DTO) =================
        public static Expression<Func<Category, CategoryResponseDto>> EntityToDtoExpression =>
            c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IconUrl = c.IconUrl
            };
        public static CategoryResponseDto EntityToDto(this Category category) =>
            new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IconUrl = category.IconUrl
            };

        public static Category CreateCategoryRequestDtoToEntity(this CategoryRequestDto dto)
        {
            return new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                IconUrl = dto.IconUrl
            };
        }
        
        public static void UpdateCategoryRequestDtoToEntity(this CategoryRequestDto dto, Category entity)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IconUrl = dto.IconUrl;
            entity.UpdatedAt = DateTime.UtcNow;
        }

       
    }
}
