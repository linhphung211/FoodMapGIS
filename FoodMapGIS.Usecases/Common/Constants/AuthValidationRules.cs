using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.Common.Constants
{
    public static class AuthValidationRules
    {
        // 1. Username: 3-30 ký tự, chữ cái, số, gạch dưới '_', gạch ngang '-'
        public const string UsernameRegex = @"^[a-zA-Z0-9_-]{3,30}$";
        public const string UsernameError = "Tên đăng nhập từ 3-30 ký tự, chỉ gồm chữ cái, chữ số, gạch dưới '_' hoặc gạch ngang '-'.";
        // 2. Email: định dạng email chuẩn
        public const string EmailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        public const string EmailError = "Địa chỉ email không đúng định dạng.";
        // 3. Password: tối thiểu 8 ký tự, gồm ít nhất 1 chữ hoa, 1 chữ thường, 1 số, 1 ký tự đặc biệt
        public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$";
        public const string PasswordError = "Mật khẩu tối thiểu 8 ký tự, phải gồm chữ hoa, chữ thường, chữ số và ít nhất 1 ký tự đặc biệt.";
        // 4. OTP: đúng 6 chữ số
        public const string OtpRegex = @"^\d{6}$";
        public const string OtpError = "Mã OTP phải gồm đúng 6 chữ số.";
    }
}
