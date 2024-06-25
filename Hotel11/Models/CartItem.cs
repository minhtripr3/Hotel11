using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel11.Models
{
    public class CartItem
    {
        HotelDB10Entities db = new HotelDB10Entities();

        public int ProductID { get; set; }
        public string NamePro { get; set; }
        public string ImagePro { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }
        public int ThueID { get; set; } // Thêm trường ThueID để lưu ID của thuế áp dụng
        public string Code { get; set; } // Mã giảm giá
        public Discount1 Discount { get; set; } // Thông tin giảm giá

        public decimal FinalPrice()
        {
            decimal finalPrice = Number * Price;

            // Lấy thông tin thuế từ cơ sở dữ liệu và tính toán thuế
            var tax = db.Thues.Find(ThueID);
            if (tax != null)
            {
                decimal taxPercent = tax.PhanTramThue ?? 0; // Lấy phần trăm thuế, nếu có
                decimal taxAmount = finalPrice * (taxPercent / 100); // Tính số tiền thuế
                finalPrice += taxAmount; // Tổng giá cuối cùng bao gồm cả thuế
            }
            if (Discount != null)
            {
                finalPrice -= Discount.Amount;
            }

            return finalPrice;
        }
        public CartItem() { }

        public CartItem(int ProductID, int thueId)
        {
            this.ProductID = ProductID;
            this.ThueID = thueId; // Gán giá trị thueId cho thuộc tính ThueID
            var productDB = db.Rooms.Single(s => s.ProductID == this.ProductID);
            this.NamePro = productDB.NamePro;
            this.ImagePro = productDB.ImagePro;
            this.Price = (decimal)productDB.Price;

            this.Number = 1;
        }
    }
}
