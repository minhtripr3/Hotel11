using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel11.Models
{
    public class Top5YeuThich
    {
        public string NamePro { get; set; }
        public string ImgPro { get; set; }
        public decimal pricePro { get; set; }
        public decimal NameCate { get; set; }
        public decimal DesPro { get; set; }
        [System.ComponentModel.DataAnnotations.Key]
        public int? IDPro { get; set; }
        public decimal Total_Money { get; set; }
        public Room product { get; set; }
        public Category category { get; set; }
        public Quanly orderDetail { get; set; }
        public IEnumerable<Room> ListProduct { get; set; }
        public int? Top5_Quantity { get; set; }
        public int? Sum_Quantity { get; set; }
    }
}