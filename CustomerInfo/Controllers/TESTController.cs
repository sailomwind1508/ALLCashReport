using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerInfo.Models;

namespace CustomerInfo.Controllers
{
    public class TESTController : Controller
    {
        private CustInfoEntities db = new CustInfoEntities();

        // GET: TEST
        public ActionResult Index()
        {
            var tblCustomerAM2 = db.tblCustomerAM2.Include(t => t.tblProvince).Where(x => x.CustomerID == "0"); ;
            ViewBag.BranchID = new SelectList(GetBranches(), "BranchID", "BranchName");


            return View(tblCustomerAM2.ToList());
        }
        public List<Branch> GetBranches()
        {
            List<Branch> branches = db.Branches.ToList();
            return branches;
           

        }
        public ActionResult GetbranchToProvince(string BranchID)
        {
            var branchToProvinces = from tblBP in db.tblBranchToProvinces
                                    join province in db.tblProvinces
                                    on tblBP.ProvinceID equals province.ProvinceID
                                    where tblBP.BranchID == BranchID
                                    select new
                                    {
                                        tblBP.ProvinceID,
                                        Name = province.ProvinceName
                                    };
            return Json(branchToProvinces, JsonRequestBehavior.AllowGet);
        }
      

     


        // GET: TEST/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomerAM2 tblCustomerAM2 = db.tblCustomerAM2.Find(id);
            if (tblCustomerAM2 == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomerAM2);
        }

        // GET: TEST/Create
        public ActionResult Create()
        {
            ViewBag.ProvinceID = new SelectList(db.tblProvinces, "ProvinceID", "ProvinceName");
            return View();
        }

        // POST: TEST/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CustName,Address,DistrictID,AreaID,ProvinceID,Lattitude,Longtitude,ZoneID,SalAreaName,SlpCode,SlpName,CustImg")] tblCustomerAM2 tblCustomerAM2)
        {
            if (ModelState.IsValid)
            {
                db.tblCustomerAM2.Add(tblCustomerAM2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProvinceID = new SelectList(db.tblProvinces, "ProvinceID", "ProvinceName", tblCustomerAM2.ProvinceID);
            return View(tblCustomerAM2);
        }

        // GET: TEST/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomerAM2 tblCustomerAM2 = db.tblCustomerAM2.Find(id);
            if (tblCustomerAM2 == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProvinceID = new SelectList(db.tblProvinces, "ProvinceID", "ProvinceName", tblCustomerAM2.ProvinceID);
            return View(tblCustomerAM2);
        }

        // POST: TEST/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustName,Address,DistrictID,AreaID,ProvinceID,Lattitude,Longtitude,ZoneID,SalAreaName,SlpCode,SlpName,CustImg")] tblCustomerAM2 tblCustomerAM2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCustomerAM2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProvinceID = new SelectList(db.tblProvinces, "ProvinceID", "ProvinceName", tblCustomerAM2.ProvinceID);
            return View(tblCustomerAM2);
        }

        // GET: TEST/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomerAM2 tblCustomerAM2 = db.tblCustomerAM2.Find(id);
            if (tblCustomerAM2 == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomerAM2);
        }

        // POST: TEST/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tblCustomerAM2 tblCustomerAM2 = db.tblCustomerAM2.Find(id);
            db.tblCustomerAM2.Remove(tblCustomerAM2);
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
