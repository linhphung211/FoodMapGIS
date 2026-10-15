using FoodMapGIS.Application.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Auth
{
    public record VerifyRegisterOtpRequestDto
    {
        [Required(ErrorMessage = "Email không được để trống.")]
        [RegularExpression(AuthValidationRules.EmailRegex, ErrorMessage = AuthValidationRules.EmailError)]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Mã OTP không được để trống.")]
        [RegularExpression(AuthValidationRules.OtpRegex, ErrorMessage = AuthValidationRules.OtpError)]
        public string Otp { get; init; } = string.Empty;
    }
    public record VerifyRegisterOtpResponseDto
    {
        public string VerificationToken { get; init; } = string.Empty;
    }
}
