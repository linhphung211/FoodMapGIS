using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodMapGIS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Bước 1 Đăng ký: Gửi mã OTP xác thực email trước khi tạo tài khoản
    /// </summary>
    [HttpPost("send-register-otp")]
    public async Task<IActionResult> SendRegisterOtp([FromBody] SendRegisterOtpRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.SendRegisterOtpAsync(request, cancellationToken);
            return Ok(new { message = "Mã OTP xác thực đăng ký đã được gửi đến email của bạn." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Bước 2 Đăng ký: Xác thực mã OTP - Trả về verification_token để tiếp tục đăng ký
    /// </summary>
    [HttpPost("verify-register-otp")]
    public async Task<ActionResult<VerifyRegisterOtpResponseDto>> VerifyRegisterOtp([FromBody] VerifyRegisterOtpRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _authService.VerifyRegisterOtpAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Dang ki tai khoan moi
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.RegisterAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    //Dang nhap tai khoan; trar ve JWT Token
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.LoginAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Lay thong tin ca nhan cua nguoi dung dang dang nhap
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserProfileResponseDto>> GetMyProfile(CancellationToken cancellationToken)
    {
        try
        {
            // Trich xuat UserId tu Claim cua token:
            var userId = GetCurrentUserId();
            var profile = await _authService.GetProfileAsync(userId, cancellationToken);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Cap nhat thong tin ca nhan
    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult<UserProfileResponseDto>> UpdateMyProfile([FromBody] UpdateProfileRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            // Trich xuat UserId tu Claim cua token:
            var userId = GetCurrentUserId();
            var updatedProfile = await _authService.UpdateProfileAsync(userId, request, cancellationToken);
            return Ok(updatedProfile);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new Exception("Token không hợp lệ hoặc không xác định được danh tính");
        }

        return userId;
    }

    // Lam moi access token khi token cu het han
    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.RefreshTokenAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    //Dang xuat - Huy Refresh token
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();
            await _authService.LogoutAsync(userId, cancellationToken);
            return Ok(new { message = "Đăng xuất thành công!" });
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// Bước 1: Quên mật khẩu - Nhận email và gửi mã OTP 6 số (FR-AUTH-08)
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.ForgotPasswordAsync(request, cancellationToken);
            return Ok(new { message = "Mã OTP đặt lại mật khẩu đã được gửi đến email của bạn." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// Bước 2: Xác thực mã OTP - Trả về reset_token tạm thời (FR-AUTH-08)
    [HttpPost("verify-reset-otp")]
    public async Task<ActionResult<VerifyResetOtpResponseDto>> VerifyResetOtp([FromBody] VerifyResetOtpRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _authService.VerifyResetOtpAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// Bước 3: Đặt mật khẩu mới bằng reset_token (FR-AUTH-08)
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.ResetPasswordAsync(request, cancellationToken);
            return Ok(new { message = "Đặt lại mật khẩu thành công. Bạn có thể đăng nhập bằng mật khẩu mới." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


}
