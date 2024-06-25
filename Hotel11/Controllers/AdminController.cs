using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel11.Models;

namespace Hotel11.Controllers
{
    public class AdminController : Controller
    {
        private HotelDB10Entities  database = new HotelDB10Entities();
        [HttpGet]
        public ActionResult Registeradmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registeradmin(AdminUser admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.NameUser))
                    ModelState.AddModelError(string.Empty, " Tên không được để trống");
                if (string.IsNullOrEmpty(admin.RoleUser))
                    ModelState.AddModelError(string.Empty, "Role không được để trống");
                if (string.IsNullOrEmpty(admin.PasswordUser))
                    ModelState.AddModelError(string.Empty, "Pasword không được để trống");






                //Kiểm tra xem có người nào đã đăng kí với tên đăng nhập này hay chưa
                var Admin = database.AdminUsers.FirstOrDefault(k => k.NameUser == admin.NameUser);
                if (Admin != null)
                    ModelState.AddModelError(string.Empty, "Tên này đã được admin tạo");
                if (ModelState.IsValid)
                {
                    database.AdminUsers.Add(admin);
                    database.SaveChanges();

                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Loginadmin");
        }
        [HttpGet]
        public ActionResult Loginadmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Loginadmin(AdminUser admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.NameUser)) ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(admin.RoleUser)) ModelState.AddModelError(string.Empty, "Role không được để trống");
                if (string.IsNullOrEmpty(admin.PasswordUser)) ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    // tim khach hang co ten dang nhap va password hop le trong csdl
                    var Admin = database.AdminUsers.FirstOrDefault(k => k.NameUser == admin.NameUser && k.RoleUser == admin.RoleUser && k.PasswordUser == admin.PasswordUser);
                    if (Admin != null)
                    {

                        //luu vao session
                        Session["TaiKhoan"] = Admin;
                        return RedirectToAction("Index", "Categories");
                        // ViewBag.ThongBao = " Đăng nhập thành công";
                    }
                    else
                    {
                        ViewBag.ThongBao = " Tên đăng nhập hoặc mật khẩu không đúng";
                    }
                }
            }
            return View("Loginadmin");
        }
    }
}