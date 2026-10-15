using FoodMapGIS.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.Common.Interface.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
        Task<UserProfileResponseDto> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<UserProfileResponseDto> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto request, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
        Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default);
        // Cac ham xu ly quen mat khau
        Task ForgotPasswordAsync(ForgotPasswordRequestDto request, CancellationToken cancellationToken = default);
        Task<VerifyResetOtpResponseDto> VerifyResetOtpAsync(VerifyResetOtpRequestDto request, CancellationToken cancellationToken = default);
        Task ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default);
        // Cac ham tien xac thuc email
        Task SendRegisterOtpAsync(SendRegisterOtpRequestDto request, CancellationToken cancellationToken = default);
        Task<VerifyRegisterOtpResponseDto> VerifyRegisterOtpAsync(VerifyRegisterOtpRequestDto request, CancellationToken cancellationToken = default);
    }
}
