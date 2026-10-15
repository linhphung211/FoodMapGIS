using FoodMapGIS.Application.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Auth
{
    public record UpdateProfileRequestDto
    {
        [Required(ErrorMessage = "Email không được để trống.")]
        [RegularExpression(AuthValidationRules.EmailRegex, ErrorMessage = AuthValidationRules.EmailError)]
        public string Email { get; init; } = string.Empty;
        public string? AvatarUrl { get; init; }
    }
}
