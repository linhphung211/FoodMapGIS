using FoodMapGIS.Application.Common.Interface.Infrastructure;
using FoodMapGIS.Application.Common.Interface.Repositories;
using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Application.DTOs.Auth;
using FoodMapGIS.Application.DTOs.Auth.Mapping;
using FoodMapGIS.Domain.Entities;
using FoodMapGIS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FoodMapGIS.Application.Services
{
    public class AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMemoryCache cache, IEmailService emailService) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
        private readonly IMemoryCache _cache = cache;
        private readonly IEmailService _emailService = emailService;

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.FindOneAsync(u => u.Username == request.Username, cancellationToken: cancellationToken);
            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Ten dang nhap hoac mat khau khong chinh xac");
            }
            if (!user.IsActive)
            {
                throw new Exception("Tai khoan cua ban bi khoa");
            }
            user.LastLogin = DateTime.UtcNow;
            var accessToken = _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.EntityToAuthResponseDto(accessToken, refreshToken);
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
        {
            // 1. Chặn người dùng tự ý đăng ký làm Admin
            if (request.Role != UserRole.User && request.Role != UserRole.Merchant)
            {
                throw new Exception("Chỉ được phép đăng ký tài khoản với vai trò User hoặc Merchant.");
            }

            // 2. Kiểm tra token xác thực email trước tiên
            var tokenCacheKey = $"verified_email_{request.VerificationToken}";
            if (!_cache.TryGetValue(tokenCacheKey, out string? verifiedEmail) || string.IsNullOrEmpty(verifiedEmail))
            {
                throw new Exception("Phiên xác thực email đã hết hạn hoặc không hợp lệ. Vui lòng xác thực email lại từ đầu.");
            }

            // 4. Kiểm tra trùng lặp trong Database
            if (await _unitOfWork.Users.ExistsAsync(u => u.Username == request.Username, cancellationToken))
            {
                throw new Exception("Tên đăng nhập đã được sử dụng trong hệ thống!");
            }

            if (await _unitOfWork.Users.ExistsAsync(u => u.Email == verifiedEmail, cancellationToken))
            {
                throw new Exception("Email này đã được sử dụng!");
            }

            // 5. Khởi tạo User và mật khẩu băm
            var user = request.RegisterRequestDtoToEntity(_passwordHasher.Hash(request.Password), verifiedEmail);
            user.LastLogin = DateTime.UtcNow;

            // 6. Cấp Access Token + Refresh Token
            var accessToken = _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            // 7. Lưu vào Database
            await _unitOfWork.Users.CreateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 8. Đăng ký thành công -> Xóa token khỏi cache để không tái sử dụng
            _cache.Remove(tokenCacheKey);

            return user.EntityToAuthResponseDto(accessToken, refreshToken);
        }
        public async Task<UserProfileResponseDto> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new Exception("Khong tim thay thong tin nguoi dung");
            }
            if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (await _unitOfWork.Users.ExistsAsync(u => u.Email == request.Email && u.Id != userId, cancellationToken))
                {
                    throw new Exception("Email nay da duoc su dung boi 1 tai khoan khac.");
                }
                user.Email = request.Email;
            }
            user.AvatarUrl = request.AvatarUrl;
            user.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.EntityToProfileResponseDto();
        }
        public async Task<UserProfileResponseDto> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.FindOneAsync(u => u.Id == userId, asNoTracking: true, cancellationToken: cancellationToken);
            if (user == null)
            {
                throw new Exception("Khong tim thay thong tin nguoi dung");
            }
            return user.EntityToProfileResponseDto();
        }
        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.FindOneAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken: cancellationToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new Exception("Refresh Token ko hop le hoac het han. Vui long dang nhap lai");
            }
            var newAccessToken = _jwtTokenGenerator.GenerateToken(user);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.EntityToAuthResponseDto(newAccessToken, newRefreshToken);
        }
        public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
        //Xu ly quen mat khau
        public async Task ForgotPasswordAsync(ForgotPasswordRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.FindOneAsync(u => u.Email == request.Email, asNoTracking: true, cancellationToken: cancellationToken);
            if (user == null)
            {
                throw new Exception("Không tìm thấy tài khoản với email này.");
            }
            // Sinh mã OTP ngẫu nhiên 6 chữ số:
            var otp = new Random().Next(100000, 999999).ToString();
            // Lưu vào Cache với thời hạn 5 phút (Key: reset_otp_<email>, Value: otp):
            var cacheKey = $"reset_otp_{request.Email.ToLower()}";
            _cache.Set(cacheKey, otp, TimeSpan.FromMinutes(5));
            // Gửi email:
            var emailBody = $"Xin chào {user.Username},\n\nMã OTP đặt lại mật khẩu của bạn là: {otp}\nMã này có hiệu lực trong vòng 5 phút.";
            await _emailService.SendEmailAsync(user.Email, "Mã OTP đặt lại mật khẩu - FoodMap GIS", emailBody, cancellationToken);
        }

        public Task<VerifyResetOtpResponseDto> VerifyResetOtpAsync(VerifyResetOtpRequestDto request, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"reset_otp_{request.Email.ToLower()}";
            // Kiểm tra mã OTP trong cache:
            if (!_cache.TryGetValue(cacheKey, out string? cachedOtp) || cachedOtp != request.Otp)
            {
                throw new Exception("Mã OTP không chính xác hoặc đã hết hạn.");
            }
            // OTP đúng -> Xóa OTP khỏi cache để tránh dùng lại:
            _cache.Remove(cacheKey);
            // Sinh reset_token tạm thời (GUID):
            var resetToken = Guid.NewGuid().ToString("N");
            // Lưu reset_token vào cache trong 10 phút (Key: reset_token_<token>, Value: email):
            var tokenCacheKey = $"reset_token_{resetToken}";
            _cache.Set(tokenCacheKey, request.Email.ToLower(), TimeSpan.FromMinutes(10));
            return Task.FromResult(new VerifyResetOtpResponseDto { ResetToken = resetToken });
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default)
        {
            var tokenCacheKey = $"reset_token_{request.ResetToken}";
            // Xác thực reset_token còn hạn không:
            if (!_cache.TryGetValue(tokenCacheKey, out string? email) || string.IsNullOrEmpty(email))
            {
                throw new Exception("Yêu cầu đặt lại mật khẩu đã hết hạn hoặc không hợp lệ. Vui lòng thử lại từ đầu.");
            }
            var user = await _unitOfWork.Users.FindOneAsync(u => u.Email == email, asNoTracking: true, cancellationToken: cancellationToken);
            if (user == null)
            {
                throw new Exception("Không tìm thấy thông tin tài khoản.");
            }
            // Băm mật khẩu mới bằng BCrypt và lưu vào DB:
            user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            // Hủy luôn RefreshToken cũ nếu có để buộc đăng nhập lại từ đầu:
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            // Xóa reset_token khỏi cache:
            _cache.Remove(tokenCacheKey);
        }

        //Xac thuc Email trong qua trinh dki tai khoan
        // 1. Gửi OTP xác thực trước khi cho phép đăng ký
        public async Task SendRegisterOtpAsync(SendRegisterOtpRequestDto request, CancellationToken cancellationToken = default)
        {
            // Kiểm tra xem email này đã có ai dùng trong DB chưa:
            if (await _unitOfWork.Users.ExistsAsync(u => u.Email == request.Email, cancellationToken))
            {
                throw new Exception("Email này đã được đăng ký trong hệ thống.");
            }

            // Sinh mã OTP 6 số:
            var otp = new Random().Next(100000, 999999).ToString();

            // Lưu vào Cache 5 phút:
            var cacheKey = $"register_otp_{request.Email.ToLower()}";
            _cache.Set(cacheKey, otp, TimeSpan.FromMinutes(5));

            // Bắn email:
            var emailBody = $"Chào bạn,\n\nMã OTP để xác thực đăng ký tài khoản FoodMap GIS của bạn là: {otp}\nMã này có hiệu lực trong 5 phút.";
            await _emailService.SendEmailAsync(request.Email, "Mã xác thực đăng ký tài khoản - FoodMap GIS", emailBody, cancellationToken);
        }

        // 2. Xác thực OTP đăng ký -> Cấp verification_token tạm trong 15 phút
        public Task<VerifyRegisterOtpResponseDto> VerifyRegisterOtpAsync(VerifyRegisterOtpRequestDto request, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"register_otp_{request.Email.ToLower()}";

            if (!_cache.TryGetValue(cacheKey, out string? cachedOtp) || cachedOtp != request.Otp)
            {
                throw new Exception("Mã OTP không chính xác hoặc đã hết hạn.");
            }

            // Xóa OTP cũ để chống dùng lại:
            _cache.Remove(cacheKey);

            // Cấp mã token chứng nhận email này đã hợp lệ:
            var verificationToken = Guid.NewGuid().ToString("N");
            var tokenCacheKey = $"verified_email_{verificationToken}";

            // Lưu email kèm token vào cache trong 15 phút để người dùng kịp điền form:
            _cache.Set(tokenCacheKey, request.Email.ToLower(), TimeSpan.FromMinutes(15));

            return Task.FromResult(new VerifyRegisterOtpResponseDto { VerificationToken = verificationToken });
        }
    }
}
