using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Auth
{
    public record VerifyResetOtpRequestDto
    {
        public string Email { get; init; } = string.Empty;
        public string Otp { get; init; } = string.Empty;
    }

    public record VerifyResetOtpResponseDto
    {
        public string ResetToken { get; init; } = string.Empty;
    }

}
