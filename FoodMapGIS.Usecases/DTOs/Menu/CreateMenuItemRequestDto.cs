using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Menu
{
    public record CreateMenuItemRequestDto
    {
        [Required(ErrorMessage = "Tên món ăn không được để trống.")]
        [MaxLength(150, ErrorMessage = "Tên món ăn không được vượt quá 150 ký tự.")]
        public string Name { get; init; } = string.Empty;

        [Range(0, 100_000_000, ErrorMessage = "Giá món ăn phải từ 0đ đến 100.000.000đ.")]
        public decimal Price { get; init; }

        public string? ImageUrl { get; init; }

        [MaxLength(50, ErrorMessage = "Tên nhóm món ăn không được vượt quá 50 ký tự.")]
        public string? Group { get; init; } // Ví dụ: "Món chính", "Đồ uống", "Tráng miệng"...

        [MaxLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string? Description { get; init; }

        public bool IsAvailable { get; init; } = true;
    }
}
