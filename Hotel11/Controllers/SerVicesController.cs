using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel11.Models;

namespace Hotel11.Controllers
{
    public class SerVicesController : Controller
    {
        private HotelDB10Entities db = new HotelDB10Entities();

        // GET: Products
        public ActionResult Index()
        {

            var services = db.SerVices.ToList();
            return View(services);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
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
            return View(service);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SerViceID,ServiceDescription,PriceSerVice,ImageSerVice,NameSerVice")] SerVice service,
         HttpPostedFileBase ImageSerVice)
        {
            if (ModelState.IsValid)
            {
                if (ImageSerVice != null)
                {
                    //Lấy tên file của hình được up lên

                    var fileName = Path.GetFileName(ImageSerVice.FileName);

                    //Tạo đường dẫn tới file

                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    //Lưu tên

                  service .ImageSerVice = fileName;
                    //Save vào Images Folder
                    ImageSerVice.SaveAs(path);

                }
               
                db.SerVices.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(service);
        }
        public ActionResult Edit(int? id)
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
          
            return View(service);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SerViceID,ServiceDescription,PriceSerVice,ImageSerVice,NameSerVice")] SerVice service, HttpPostedFileBase ImageSerVice)
        {
            if (ModelState.IsValid)
            {
                var serviceDB = db.SerVices.FirstOrDefault(s => s.SerViceID == service.SerViceID);
                if (serviceDB != null)
                {
                    serviceDB.ServiceDescription = service.ServiceDescription;
                    serviceDB.PriceSerVice = service.PriceSerVice;
                   
                    if (ImageSerVice != null)
                    {
                        //;ay ten file cua hinh duoc up len
                        var fileName = Path.GetFileName(ImageSerVice.FileName);
                        //tao duong dan toi file
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        // luu ten
                        serviceDB.ImageSerVice = fileName;
                        //save vao images folder
                        ImageSerVice.SaveAs(path);
                    }
                    serviceDB.NameSerVice = service.NameSerVice;

                }
                db.SaveChanges();
                return RedirectToAction("Index");

            }
           
            return View(service);
        }


        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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
            return View(service);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SerVice service = db.SerVices.Find(id);
            db.SerVices.Remove(service);
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
