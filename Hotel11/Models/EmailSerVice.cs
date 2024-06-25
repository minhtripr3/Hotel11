using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Hotel11.Models
{
    public class EmailService
    {
        public void SendPasswordResetEmail(string email, string resetToken)
        {
            var fromEmail = "phamminhtripr3@gmail.com"; // Điền email của bạn
            var toEmail = email;
            var subject = "Khôi phục mật khẩu";
            var body = $"Nhấp vào liên kết sau để khôi phục mật khẩu: https://yourwebsite.com/resetpassword?token={resetToken}";

            using (var message = new MailMessage(fromEmail, toEmail, subject, body))
            {
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("phamminhtripr3@gmail.com", "tri17032004");

                    try
                    {
                        smtpClient.Send(message);
                        // Gửi email thành công
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi khi gửi email
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
    }
}
