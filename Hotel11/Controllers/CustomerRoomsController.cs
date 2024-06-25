using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Hotel11.Models;

namespace Hotel11.Controllers
{
    public class CustomerRoomsController : Controller
    {
        private HotelDB10Entities db = new HotelDB10Entities();
        public ActionResult Index()
        {
            var rooms = db.Rooms;
            return View(rooms.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new
                HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }
    }
}