using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel11.Models;

namespace Hotel11.Controllers
{
    public class QuanliesController : Controller
    {
        private HotelDB10Entities db = new HotelDB10Entities();

        // GET: Quanlies
        public ActionResult Index()
        {
            var quanlies = db.Quanlies.Include(q => q.OrderRoom).Include(q => q.Room);
            return View(quanlies.ToList());
        }

        // GET: Quanlies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quanly quanly = db.Quanlies.Find(id);
            if (quanly == null)
            {
                return HttpNotFound();
            }
            return View(quanly);
        }

        // GET: Quanlies/Create
        public ActionResult Create()
        {
            ViewBag.IDOrder = new SelectList(db.OrderRooms, "ID", "NameCus");
            ViewBag.IDProduct = new SelectList(db.Rooms, "ProductID", "NamePro");
            return View();
        }

        // POST: Quanlies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDProduct,IDOrder,UnitPrice,Quantity")] Quanly quanly)
        {
            if (ModelState.IsValid)
            {
                db.Quanlies.Add(quanly);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDOrder = new SelectList(db.OrderRooms, "ID", "NameCus", quanly.IDOrder);
            ViewBag.IDProduct = new SelectList(db.Rooms, "ProductID", "NamePro", quanly.IDProduct);
            return View(quanly);
        }

        // GET: Quanlies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quanly quanly = db.Quanlies.Find(id);
            if (quanly == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDOrder = new SelectList(db.OrderRooms, "ID", "NameCus", quanly.IDOrder);
            ViewBag.IDProduct = new SelectList(db.Rooms, "ProductID", "NamePro", quanly.IDProduct);
            return View(quanly);
        }

        // POST: Quanlies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDProduct,IDOrder,UnitPrice,Quantity")] Quanly quanly)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quanly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDOrder = new SelectList(db.OrderRooms, "ID", "NameCus", quanly.IDOrder);
            ViewBag.IDProduct = new SelectList(db.Rooms, "ProductID", "NamePro", quanly.IDProduct);
            return View(quanly);
        }

        // GET: Quanlies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quanly quanly = db.Quanlies.Find(id);
            if (quanly == null)
            {
                return HttpNotFound();
            }
            return View(quanly);
        }

        // POST: Quanlies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quanly quanly = db.Quanlies.Find(id);
            db.Quanlies.Remove(quanly);
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
