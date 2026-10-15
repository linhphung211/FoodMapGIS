using FoodMapGIS.Domain.Entities;

namespace FoodMapGIS.Application.DTOs.Auth.Mapping
{
    public static class AuthMappingExtensions
    {
        // 1. Map User sang UserProfileResponseDto (dùng cho GetProfileAsync và UpdateProfileAsync)
        public static UserProfileResponseDto EntityToProfileResponseDto(this User user)
        {
            return new UserProfileResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString(),
                AvatarUrl = user.AvatarUrl,
                CreatedAt = user.CreatedAt
            };
        }
        // 2. Map User + Token sang AuthResponseDto (dùng cho LoginAsync, RegisterAsync, RefreshTokenAsync)
        public static AuthResponseDto EntityToAuthResponseDto(this User user, string token, string refreshToken)
        {
            return new AuthResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token,
                RefreshToken = refreshToken
            };
        }
        // 3. Map RegisterRequestDto sang Entity User mới (dùng cho RegisterAsync)
        public static User RegisterRequestDtoToEntity(this RegisterRequestDto request, string passwordHash, string email)
        {
            return new User
            {
                Username = request.Username,
                Email = email,
                PasswordHash = passwordHash,
                Role = request.Role,
                IsActive = true
            };
        }
    }
}
