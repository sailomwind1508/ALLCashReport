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
    public class tblCustomerAM2Controller : Controller
    {
        private CustInfoEntities db = new CustInfoEntities();

        // GET: tblCustomerAM2
        public ActionResult Index(string BranchID, string ProvinceID, string AreaID, string DistrictID,int? page)
        {

            ViewBag.BranchID = new SelectList(GetBranches(), "BranchID", "BranchName");

            var tblCustomerAM2 = db.tblCustomerAM2.Include(t => t.tblProvince).Where(x => x.CustomerID == "0");


            if (!string.IsNullOrEmpty(BranchID))
            {
                tblCustomerAM2 = db.tblCustomerAM2.Include(t => t.tblProvince).Where(x => x.SlpCode.StartsWith(BranchID));

                if (!string.IsNullOrEmpty(ProvinceID))
                {
                    tblCustomerAM2 = tblCustomerAM2.Where(x => x.SlpCode.StartsWith(BranchID) & x.ProvinceID == ProvinceID);

                    if (!string.IsNullOrEmpty(AreaID))
                    {
                        tblCustomerAM2 = tblCustomerAM2.Where(x => x.SlpCode.StartsWith(BranchID) & x.ProvinceID == ProvinceID & x.AreaID == AreaID);

                        if (!string.IsNullOrEmpty(DistrictID))
                        {
                            tblCustomerAM2 = tblCustomerAM2.Where(x => x.SlpCode.StartsWith(BranchID) & x.ProvinceID == ProvinceID & x.AreaID == AreaID & x.DistrictID == DistrictID);
                        }
                    }
                }

            }

            string filterID = "";
            if (BranchID != null)
            {
                Dictionary<string, string> FilterSession = new Dictionary<string, string>();
                FilterSession.Add("BranchID", BranchID);
                FilterSession.Add("ProvinceID", ProvinceID);
                FilterSession.Add("AreaID", AreaID);
                FilterSession.Add("DistrictID", DistrictID);


                filterID = string.Join(",", BranchID, ProvinceID, AreaID, DistrictID);

                Session["FilterSession"] = filterID;
            }
            Session["FilterSession"] = filterID;

            return View(tblCustomerAM2.ToList().ToPagedList(page ?? 1,3));
        }

        [HttpPost]
        public JsonResult GetSession()
        {
            return Json(Session["FilterSession"].ToString());
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

        public ActionResult GetprovinceToArea(string ProvinceID)
        {

            var provinceToArea = from tblPA in db.tblProvinceToAreas
                                 join area in db.tblAreas
                                 on tblPA.AreaID equals area.AreaID
                                 where tblPA.ProvinceID == ProvinceID
                                 select new
                                 {
                                     tblPA.AreaID,
                                     Name = area.AreaName

                                 };
            return Json(provinceToArea, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetareaTodistrict(string AreaID)
        {
            var areaTodistrict = from aTod in db.tblAreaToDistricts
                                 join dis in db.tblDistricts
                                 on aTod.DistrictID equals dis.DistrictID
                                 where aTod.AreaID == AreaID
                                 select new
                                 {
                                     aTod.DistrictID,
                                     Name = dis.DistrictName
                                 };

            return Json(areaTodistrict, JsonRequestBehavior.AllowGet);
        }
        // GET: tblCustomerAM2/Details/5
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

        // GET: tblCustomerAM2/Create
        public ActionResult Create()
        {
            ViewBag.ProvinceID = new SelectList(db.tblProvinces, "ProvinceID", "ProvinceName");
            return View();
        }

        // POST: tblCustomerAM2/Create
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

        // GET: tblCustomerAM2/Edit/5
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

        // POST: tblCustomerAM2/Edit/5
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

        // GET: tblCustomerAM2/Delete/5
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

        // POST: tblCustomerAM2/Delete/5
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