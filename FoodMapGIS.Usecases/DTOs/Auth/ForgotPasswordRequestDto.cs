using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Auth
{
    public record ForgotPasswordRequestDto
    {
        public string Email { get; init; } = string.Empty;
    }
}
