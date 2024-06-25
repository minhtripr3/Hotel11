using Hotel11.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Hotel11.Controllers
{
    public class BookSerViceController : Controller
    {
        private HotelDB10Entities db = new HotelDB10Entities();

        // GET: OrderRooms
        public ActionResult Index()
        {
            var bookServices = db.BookSerVices.Include(b => b.SerVice);
            return View(bookServices.ToList());
        }

        // GET: OrderRooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookSerVice bookService = db.BookSerVices.Find(id);
            if (bookService == null)
            {
                return HttpNotFound();
            }
            return View(bookService);
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SerVice service = db.SerVices.Find(id);

            if (service == null)
            {
                return HttpNotFound();
            }

            BookSerVice bookService = new BookSerVice
            {
                IDSerVice = id.Value,
                SerViceDateOrder = DateTime.Now,
                NameSerVice = service.NameSerVice,
            };

            if (service.PriceSerVice.HasValue)
            {
                bookService.Total = service.PriceSerVice.Value;
            }
            else
            {
                bookService.Total = 0;
            }

            return View(bookService);
        }

        // POST: OrderRooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookSerViceID,SerViceDateOrder,IDSerVice,NameCus,PhoneCus,DressCus,Total,DateBookSerVice,NameSerVice,CheckIntDate,CheckOutDate")] BookSerVice bookService)
        {
            if (ModelState.IsValid)
            {
                db.BookSerVices.Add(bookService);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookService);
        }

        // GET: OrderRooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookSerVice bookService = db.BookSerVices.Find(id);
            if (bookService == null)
            {
                return HttpNotFound();
            }
            ViewBag.SerViceID = new SelectList(db.SerVices, "SerViceID", "NameSerVice", bookService.IDSerVice);
            return View(bookService);
        }

        // POST: OrderRooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookSerViceID,SerViceDateOrder,IDSerVice,NameCus,PhoneCus,DressCus,Total,DateBookSerVice,NameSerVice,CheckIntDate,CheckOutDate")] BookSerVice bookService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SerViceID = new SelectList(db.SerVices, "SerViceID", "NameSerVice", bookService.IDSerVice);
            return View(bookService);
        }

        // GET: OrderRooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookSerVice bookService = db.BookSerVices.Find(id);
            if (bookService == null)
            {
                return HttpNotFound();
            }
            return View(bookService);
        }

        // POST: OrderRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookSerVice bookService = db.BookSerVices.Find(id);
            db.BookSerVices.Remove(bookService);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
