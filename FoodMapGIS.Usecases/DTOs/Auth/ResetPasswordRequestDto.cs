using FoodMapGIS.Application.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Auth
{
    public record ResetPasswordRequestDto
    {
        [Required(ErrorMessage = "Mã đặt lại mật khẩu không được để trống.")]
        public string ResetToken { get; init; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu mới không được để trống.")]
        [RegularExpression(AuthValidationRules.PasswordRegex, ErrorMessage = AuthValidationRules.PasswordError)]
        public string NewPassword { get; init; } = string.Empty;
    }
}
