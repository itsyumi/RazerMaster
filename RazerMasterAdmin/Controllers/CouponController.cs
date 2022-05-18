using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace RazerMasterAdmin.Controllers
{
    public class CouponController : Controller
    {
        //private RazerMasterDataContext db = new RazerMasterDataContext();
        public GenericRepository<Coupons> couponsrepo = new GenericRepository<Coupons>();
        // GET: Coupon
        public ActionResult Index()
        {
            return View(couponsrepo.GetAll().ToList());
        }

        // GET: Coupon/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coupon/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Coupons coupon)
        {
            if (ModelState.IsValid)
            {
                coupon.CouponID = Guid.NewGuid();
                couponsrepo.Update(coupon);
                //db.Coupons.Add(coupon);
                //db.SaveChanges();
                TempData["message"] = "Create Success";

                return RedirectToAction("Index");
            }

            return View(coupon);
        }

        // GET: Coupon/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coupon =couponsrepo.Get(x => x.Code == id);
            //Coupons coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: Coupon/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Coupons coupon)
        {
            if (ModelState.IsValid)
            {
                couponsrepo.Update(coupon);
                //db.Entry(coupon).State = EntityState.Modified;
                //db.SaveChanges();
                TempData["message"] = "Update Success";

                return RedirectToAction("Index");
            }
            return View(coupon);
        }

        // POST: Coupon/Delete/5
        [HttpGet,ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var coupon = couponsrepo.Get(x => x.Code == id);
            //Coupons coupon = db.Coupons.Find(id);
            couponsrepo.Delete(coupon);
            //db.Coupons.Remove(coupon);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //TempData["message"] = "Delete Success";

                couponsrepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
