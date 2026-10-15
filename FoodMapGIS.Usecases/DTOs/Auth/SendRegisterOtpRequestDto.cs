using FoodMapGIS.Application.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace FoodMapGIS.Application.DTOs.Auth
{
    public record SendRegisterOtpRequestDto
    {
        [Required(ErrorMessage = "Email không được để trống.")]
        [RegularExpression(AuthValidationRules.EmailRegex, ErrorMessage = AuthValidationRules.EmailError)]
        public string Email { get; init; } = string.Empty;
    }
}
