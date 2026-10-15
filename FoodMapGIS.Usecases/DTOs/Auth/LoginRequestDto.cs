using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Auth
{
    public record LoginRequestDto
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        public string Username { get; init; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string Password { get; init; } = string.Empty;
    }
}
