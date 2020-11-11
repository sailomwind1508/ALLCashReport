using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerInfo.Models;
using PagedList;


namespace CustomerInfo.Controllers
{
    public class tblCustomerWSController : Controller
    {
        private CustInfoEntities db = new CustInfoEntities();

        // GET: tblCustomerWS
        public ActionResult Index(int? page)
        {
            ViewBag.ZoneID = new SelectList(db.Zones, "ZoneID", "ZoneName");
            ViewBag.ProvinceID = new SelectList(db.tblProvinces, "ProvinceID", "ProvinceName");
            var tbl = db.tblCustomerWS.Include(t => t.Zone).Include(t => t.tblProvince);
            return View(tbl.ToList().ToPagedList(page ?? 1, 3));
        }
        [HttpPost]
        public ActionResult Index(string ZoneID, string ProvinceID, int? page)
        {

            ViewBag.ZoneID = new SelectList(db.Zones, "ZoneID", "ZoneName");
            ViewBag.ProvinceID = new SelectList(db.tblProvinces, "ProvinceID", "ProvinceName");
            var customer = db.tblCustomerWS.Include(t => t.Zone).Include(t => t.tblProvince);
            if (ZoneID != "" && ProvinceID == "")
            {

                return View(db.tblCustomerWS.Include(t => t.Zone).Include(t => t.tblProvince).Where(a => a.ZoneID == ZoneID).ToList());
            }
            else if (ProvinceID != "" && ZoneID == "")
            {

                return View(db.tblCustomerWS.Include(t => t.tblProvince).Include(t => t.Zone).Where(a => a.ProvinceID == ProvinceID).ToList());
            }
            else if (ProvinceID != "" && ZoneID != "")
            {

                return View(db.tblCustomerWS.Include(t => t.Zone).Include(t => t.tblProvince).Where(a => a.ZoneID == ZoneID && a.ProvinceID == ProvinceID));
            }
            else
            {

                return View(db.tblCustomerWS.Include(t => t.Zone).Include(t => t.tblProvince).ToList());
            }

        }

        // GET: tblCustomerWS/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomerW tblCustomerWS = db.tblCustomerWS.Find(id);
            if (tblCustomerWS == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomerWS);
        }

        // GET: tblCustomerWS/Create
        public ActionResult Create()
        {
            ViewBag.ZoneID = new SelectList(db.Zones, "ZoneID", "ZoneName");
            return View();
        }

        // POST: tblCustomerWS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CustName,Address,District,Area,Province,Lattitude,Longtitude,ZoneID,SalAreaName,SlpCode,SlpName,CustImg")] tblCustomerW tblCustomerW)
        {
            if (ModelState.IsValid)
            {
                db.tblCustomerWS.Add(tblCustomerW);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ZoneID = new SelectList(db.Zones, "ZoneID", "ZoneName", tblCustomerW.ZoneID);
            return View(tblCustomerW);
        }

        // GET: tblCustomerWS/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomerW tblCustomerWS = db.tblCustomerWS.Find(id);
            if (tblCustomerWS == null)
            {
                return HttpNotFound();
            }
            ViewBag.ZoneID = new SelectList(db.Zones, "ZoneID", "ZoneName", tblCustomerWS.ZoneID);
            return View(tblCustomerWS);
        }

        // POST: tblCustomerWS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustName,Address,District,Area,Province,Lattitude,Longtitude,ZoneID,SalAreaName,SlpCode,SlpName,CustImg")] tblCustomerW tblCustomerW)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCustomerW).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ZoneID = new SelectList(db.Zones, "ZoneID", "ZoneName", tblCustomerW.ZoneID);
            return View(tblCustomerW);
        }

        // GET: tblCustomerWS/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomerW tblCustomerWS = db.tblCustomerWS.Find(id);
            if (tblCustomerWS == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomerWS);
        }

        // POST: tblCustomerWS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tblCustomerW tblCustomerWS = db.tblCustomerWS.Find(id);
            db.tblCustomerWS.Remove(tblCustomerWS);
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
