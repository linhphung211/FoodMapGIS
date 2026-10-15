using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Auth
{
    public record RefreshTokenRequestDto
    {
        public string RefreshToken { get; init; } = string.Empty;
    }
}
