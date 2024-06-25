using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel11.Models
{
    public class TokenGenerator
    {
        public static string GenerateResetPasswordToken()
        {
            // Sử dụng GUID để tạo một chuỗi ngẫu nhiên
            var token = Guid.NewGuid().ToString();
            return token;
        }
    }
}