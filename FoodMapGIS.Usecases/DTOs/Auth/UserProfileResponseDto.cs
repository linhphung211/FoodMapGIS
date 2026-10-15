using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Auth
{
    public record UserProfileResponseDto
    {
        public Guid Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public string? AvatarUrl { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
