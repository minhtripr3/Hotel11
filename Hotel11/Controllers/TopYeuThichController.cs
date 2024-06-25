using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel11.Models;

namespace Hotel11.Controllers
{
    public class TopYeuThichController : Controller
    {
        // GET: OrderDetail
        HotelDB10Entities database = new HotelDB10Entities();
        public ActionResult GroupByTop5()
        {
            List<Quanly> orderD = database.Quanlies.ToList();
            List<Room> proList = database.Rooms.ToList();
            var query = from od in orderD
                        join p in proList on od.IDProduct equals p.ProductID into tbl
                        group od by new { idPro = od.IDProduct, namePro = od.Room.NamePro, ImagePro = od.Room.ImagePro, price = od.Room.Price }
            into gr
                        orderby gr.Sum(s => s.Quantity) descending
                        select new Top5YeuThich
                        {
                            IDPro = gr.Key.idPro,
                            NamePro = gr.Key.namePro,
                            ImgPro = gr.Key.ImagePro,
                            pricePro = (decimal)gr.Key.price,
                            Sum_Quantity = gr.Sum(s => s.Quantity)
                        };
            return View(query.Take(5).ToList());
        }
    }
}