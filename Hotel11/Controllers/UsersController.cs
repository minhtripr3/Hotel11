using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel11.Models;



namespace Hotel11.Controllers
{
    public class UsersController : Controller

    {
        private readonly EmailService _emailService;
        private HotelDB10Entities database = new HotelDB10Entities();
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer cust, HttpPostedFileBase ImageCus)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cust.NameCus))
                    ModelState.AddModelError(string.Empty, "Họ Tên không được để trống");
                if (string.IsNullOrEmpty(cust.PhoneCus))
                    ModelState.AddModelError(string.Empty, "Số Điện Thoại không được để trống");
                if (string.IsNullOrEmpty(cust.EmailCus))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(cust.DressCus))
                    ModelState.AddModelError(string.Empty, "Địa chỉ không được để trống");
                if (string.IsNullOrEmpty(cust.CityCus))
                    ModelState.AddModelError(string.Empty, "Thành phố không được để trống");
                if (string.IsNullOrEmpty(cust.Country))
                    ModelState.AddModelError(string.Empty, "Quốc gia không được để trống");
                if (string.IsNullOrEmpty(cust.TaiKhoan))
                    ModelState.AddModelError(string.Empty, "Tên khoản không được để trống");
                if (string.IsNullOrEmpty(cust.MatKhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(cust.ConFirPassword))
                    ModelState.AddModelError(string.Empty, "Mật khẩu nhập lại không được để trống");
              
                if (cust.NgaySinh == DateTime.MinValue || cust.NgaySinh > DateTime.Now)
                {
                    ModelState.AddModelError(string.Empty, "Ngày sinh không hợp lệ");
                }
                // Kiểm tra xem đã chọn ảnh hay chưa
                if (ImageCus == null || ImageCus.ContentLength == 0)
                {
                    ModelState.AddModelError(string.Empty, "Vui lòng chọn ảnh đại diện");
                }

                // Kiểm tra độ lớn của file ảnh
                if (ImageCus != null && ImageCus.ContentLength > 0)
                {
                    if (ImageCus.ContentLength > 2 * 1024 * 1024) // 2MB là giới hạn kích thước
                    {
                        ModelState.AddModelError(string.Empty, "Ảnh đại diện không được lớn hơn 2MB");
                    }
                    else
                    {
                        // Lưu tên file ảnh và lưu file vào thư mục Images trong dự án
                        var fileName = Path.GetFileName(ImageCus.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        ImageCus.SaveAs(path);

                        // Lưu đường dẫn của ảnh vào thuộc tính ImageCus của khách hàng
                        cust.ImageCus = fileName;
                    }
                }





                //Kiểm tra xem có người nào đã đăng kí với tên đăng nhập này hay chưa
                var khachhang = database.Customers.FirstOrDefault(k => k.TaiKhoan == cust.TaiKhoan);
                if (khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí tên này");
                if (ModelState.IsValid)
                {
                    if (cust.MatKhau != cust.ConFirPassword)
                    {
                        ModelState.AddModelError(string.Empty, "Mật khẩu và mật khẩu xác nhận không khớp.");
                        return View();
                    }
                    database.Customers.Add(cust);
                    database.SaveChanges();

                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Customer cust)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cust.TaiKhoan)) ModelState.AddModelError(string.Empty, "Tài khoản không được để trống");
                if (string.IsNullOrEmpty(cust.MatKhau)) ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    // tim khach hang co ten dang nhap va password hop le trong csdl
                    var khachhang = database.Customers.FirstOrDefault(k => k.TaiKhoan == cust.TaiKhoan && k.MatKhau == cust.MatKhau);
                    if (khachhang != null)
                    {

                        //luu vao session
                        Session["TaiKhoan"] = khachhang;
                        return RedirectToAction("Index", "ViewRoomSerVice");

                    }
                    else
                    {
                        ViewBag.ThongBao = " Tên đăng nhập hoặc mật khẩu không đúng";
                    }
                }
            }
            return View("Login");
        }
        // [HttpGet]
        // public ActionResult LogOutCus()
        //{
        //  Session.Remove("TaiKhoan");

        //return RedirectToAction("CateCus", "Home");

        //}
        public UsersController()
        {
            _emailService = new EmailService();
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {

            var customer = database.Customers.FirstOrDefault(k => k.EmailCus == email);
            if (customer != null)
            {
                var resetToken = TokenGenerator.GenerateResetPasswordToken();
                customer.ResetPasswordToken = resetToken;
                database.SaveChanges();

                // Gửi email reset mật khẩu
                _emailService.SendPasswordResetEmail(email, resetToken);

                ViewBag.SuccessMessage = "Chúng tôi đã gửi hướng dẫn khôi phục mật khẩu qua email của bạn.";
            }
            else
            {
                ViewBag.ErrorMessage = "Không tìm thấy tài khoản với địa chỉ email này.";
            }

            return View();
        }
    }
}