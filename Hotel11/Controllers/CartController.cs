using Hotel11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel11.Controllers
{
    public class CartController : Controller
    {
        HotelDB10Entities database = new HotelDB10Entities();

        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public List<CartItem> GetCart()
        {
            List<CartItem> myCart = Session["GioHang"] as List<CartItem>;

            //Nếu giỏ hàng chưa tồn tại thì tạo mới và đưa vào Session
            if (myCart == null)
            {
                myCart = new List<CartItem>();
                Session["GioHang"] = myCart;
            }

            return myCart;
        }

        public ActionResult AddToCart(int id, int? thueId)
        {
            // Lấy giỏ hàng hiện tại từ Session
            List<CartItem> myCart = GetCart();

            // Lấy thông tin sản phẩm từ database
            var room = database.Rooms.Find(id);

            if (room == null)
            {
                // Xử lý khi không tìm thấy sản phẩm
                return HttpNotFound();
            }

            // Kiểm tra số lượng đặt hàng với số lượng tồn kho
            if (myCart.Count(p => p.ProductID == id) >= room.Quantity)
            {
                // Hiển thị thông báo lỗi nếu vượt quá số lượng tồn kho
                TempData["Message"] = $"Bạn chỉ có thể đặt tối đa {room.Quantity} phòng.";
                return RedirectToAction("Index", "ViewRoomSerVice", new { id = id });
            }

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
            CartItem currentProduct = myCart.FirstOrDefault(p => p.ProductID == id);

            if (currentProduct == null)
            {
                // Sử dụng thueId được truyền vào nếu có
                if (thueId == null)
                {
                    thueId = 1; // Ví dụ gán mặc định là 1 nếu thueId không được truyền vào
                }

                // Tạo mới CartItem và thêm vào giỏ hàng
                currentProduct = new CartItem(id, thueId.Value); // Sử dụng thueId.Value để lấy giá trị int của Nullable<int>
                myCart.Add(currentProduct);
            }
            else
            {
                currentProduct.Number++; // Sản phẩm đã có trong giỏ thì tăng số lượng lên 1
            }

            // Lưu lại giỏ hàng mới vào Session
            Session["GioHang"] = myCart;

            // Chuyển hướng đến trang giỏ hàng
            return RedirectToAction("GetCartInfo");
        }

        private int GetTotalNumber()
        {
            int totalNumber = 0;
            List<CartItem> myCart = GetCart();

            if (myCart != null)
            {
                totalNumber = myCart.Sum(sp => sp.Number);
            }

            return totalNumber;
        }

        private decimal GetTotalPrice()
        {
            decimal totalPrice = 0;
            List<CartItem> myCart = GetCart();

            if (myCart != null)
            {
                totalPrice = myCart.Sum(sp => sp.FinalPrice());
            }

            return totalPrice;
        }

        public ActionResult GetCartInfo()
        {
            List<CartItem> myCart = GetCart();

            // Hiển thị thông tin giỏ hàng
            return View(myCart);
        }

        public ActionResult Cartpartial()
        {
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();

            return PartialView();
        }

        public ActionResult DeleteCartItem(int id)
        {
            List<CartItem> myCart = GetCart();

            // Lấy sản phẩm trong giỏ hàng để xóa
            var currentProduct = myCart.FirstOrDefault(p => p.ProductID == id);

            if (currentProduct != null)
            {
                // Lấy thông tin sản phẩm từ cơ sở dữ liệu
                var productDB = database.Rooms.Find(id);

                if (productDB != null)
                {
                    // Khôi phục số lượng hàng tồn khi xóa khỏi giỏ hàng
                    productDB.Quantity += currentProduct.Number;

                    // Cập nhật trạng thái của sản phẩm về chưa đặt
                    productDB.BookingStatus1 = false;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    database.SaveChanges();
                }

                // Xóa sản phẩm khỏi giỏ hàng
                myCart.RemoveAll(p => p.ProductID == id);
            }

            // Nếu giỏ hàng trống sau khi xóa, chuyển hướng về trang chủ
            if (myCart.Count == 0)
            {
                return RedirectToAction("Index", "ViewRoomSerVice");
            }

            // Chuyển hướng về trang hiển thị giỏ hàng
            return RedirectToAction("GetCartInfo");
        }

        public ActionResult UpdateCartItem(int id, int Number)
        {
            List<CartItem> myCart = GetCart();

            // Lấy sản phẩm trong giỏ hàng cần cập nhật số lượng
            var currentProduct = myCart.FirstOrDefault(p => p.ProductID == id);

            if (currentProduct != null)
            {
                // Cập nhật lại số lượng sản phẩm trong giỏ hàng
                currentProduct.Number = Number;

                // Lấy thông tin sản phẩm từ cơ sở dữ liệu
                var productDB = database.Rooms.Find(id);

                if (productDB != null)
                {
                    // Kiểm tra số lượng tồn kho trước khi cập nhật
                    if (Number <= productDB.Quantity)
                    {
                        // Cập nhật số lượng tồn kho sau khi mua hàng
                        productDB.Quantity -= Number;
                        database.SaveChanges();
                    }
                    else
                    {
                        // Xử lý khi số lượng mua vượt quá số lượng tồn kho
                        TempData["Message"] = $"Bạn chỉ có thể đặt tối đa {productDB.Quantity} phòng.";
                        return RedirectToAction("GetCartInfo");
                    }
                }
            }

            // Chuyển hướng về trang giỏ hàng
            return RedirectToAction("GetCartInfo");
        }

        public ActionResult ConfirmCart()
        {
            // Kiểm tra đăng nhập
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "Users");
            }

            // Lấy thông tin khách hàng từ Session
            Customer customer = Session["TaiKhoan"] as Customer;

            if (customer == null)
            {
                return RedirectToAction("Login", "Users");
            }

            ViewBag.Customer = customer;

            List<CartItem> myCart = GetCart();

            // Kiểm tra giỏ hàng có sản phẩm không
            if (myCart == null || myCart.Count == 0)
            {
                return RedirectToAction("Index", "ViewRoomSerVice");
            }

            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();

            // Trả về View xác nhận đặt hàng
            return View(myCart);
        }
        public ActionResult AgreeCart()
        {
            // Kiểm tra đăng nhập
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "Users");
            }

            // Lấy thông tin khách hàng từ Session
            Customer khach = Session["TaiKhoan"] as Customer;

            if (khach == null)
            {
                return RedirectToAction("Login", "Users");
            }

            // Lấy giỏ hàng từ Session
            List<CartItem> myCart = GetCart();

            // Tạo mới đơn đặt hàng
            OrderRoom DonHang = new OrderRoom
            {
                NameCus = khach.NameCus,
                PhoneCus = khach.PhoneCus,
                DateOrder = DateTime.Now
            };

            // Thêm đơn đặt hàng vào cơ sở dữ liệu
            database.OrderRooms.Add(DonHang);
            database.SaveChanges(); // Lưu lại để có được DonHang.ID

            // Thêm từng chi tiết đơn hàng vào cơ sở dữ liệu và cập nhật trạng thái sản phẩm
            foreach (var product in myCart)
            {
                var room = database.Rooms.Find(product.ProductID);
                if (room != null)
                {
                    if (room.Quantity > 0)
                    {
                        room.Quantity -= 1;

                        // Cập nhật trạng thái BookingStatus1 của phòng đã đặt
                        room.BookingStatus1 = true; // Đã đặt
                        database.SaveChanges();

                        // Thêm chi tiết đơn hàng vào cơ sở dữ liệu
                        Quanly chitiet = new Quanly
                        {
                            IDOrder = DonHang.ID,
                            IDProduct = product.ProductID,
                            Quantity = product.Number,
                            UnitPrice = (double)product.Price
                        };
                        database.Quanlies.Add(chitiet);
                        database.SaveChanges();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Không còn phòng trống.";
                        return RedirectToAction("Index", "ViewRoomSerVice");
                    }
                }
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            database.SaveChanges();

            // Xóa giỏ hàng sau khi đã đặt hàng thành công
            Session["GioHang"] = null;

            // Chuyển hướng về trang thông báo đặt hàng thành công
            return RedirectToAction("Index","ViewRoomSerVice");
        }

        [HttpGet]
        public ActionResult EditCustomerInfo()
        {
            // Lấy thông tin khách hàng từ Session
            Customer customer = Session["TaiKhoan"] as Customer;

            if (customer == null)
            {
                return RedirectToAction("Login", "Users");
            }

            // Truyền thông tin khách hàng sang View
            ViewBag.Customer = customer;

            // Trả về View chỉnh sửa thông tin khách hàng
            return View(customer);
        }

        [HttpPost]
        public ActionResult EditCustomerInfo(Customer editedCustomer)
        {
            // Lấy thông tin khách hàng từ Session
            Customer customer = Session["TaiKhoan"] as Customer;

            if (customer == null)
            {
                return RedirectToAction("Login", "Users");
            }

            // Cập nhật thông tin khách hàng
            customer.NameCus = editedCustomer.NameCus;
            customer.PhoneCus = editedCustomer.PhoneCus;
            customer.EmailCus = editedCustomer.EmailCus;

            // Lưu các thay đổi vào cơ sở dữ liệu (nếu cần)

            // Chuyển hướng về trang xác nhận đặt hàng
            return RedirectToAction("ConfirmCart");
        }

        public ActionResult checkOut_Success()
        {
            // Trả về View thông báo đặt hàng thành công
            return View();
        }
    }
}