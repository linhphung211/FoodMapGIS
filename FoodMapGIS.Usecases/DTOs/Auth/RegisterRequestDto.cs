using FoodMapGIS.Application.Common.Constants;
using FoodMapGIS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Auth
{
    public record RegisterRequestDto
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [RegularExpression(AuthValidationRules.UsernameRegex, ErrorMessage = AuthValidationRules.UsernameError)]
        public string Username { get; init; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [RegularExpression(AuthValidationRules.PasswordRegex, ErrorMessage = AuthValidationRules.PasswordError)]
        public string Password { get; init; } = string.Empty;
        public UserRole Role { get; init; } = UserRole.User;

        [Required(ErrorMessage = "Mã xác thực email không được để trống.")]
        public string VerificationToken { get; init; } = string.Empty;
    }
}
