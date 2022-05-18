using RazerMasterAdmin.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RazerMasterLibrary.Models;


namespace RazerMasterAdmin.Controllers
{
    public class AdvertisementController : Controller
    {
        private AdvertisementService advertisementService;

        public AdvertisementController()
        {
            advertisementService = new AdvertisementService();
        }

        [Authorize]
        public ActionResult Index()
        {
            var advertisements = advertisementService.GetAll();
            return View(advertisements);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Advertisements ad, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                if (files[0] != null && files[0].ContentLength > 0)
                {
                    ad.Picture = await ImgurService.Upload(files[0]);
                    var result = advertisementService.Create(ad);
                    if (result.IsSuccessful)
                    {
                        TempData["message"] = "Create Success";
                    }
                    else
                    {
                        var path = result.WriteLog();
                        TempData["message"] = "Create Error ref:" + path;
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMsg = "請至少上傳一張圖片";
                }
            }
            return View(ad);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertisements ad = advertisementService.GetByID(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImgString = ad.Picture;
            return View(ad);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Advertisements ad, HttpPostedFileBase[] files)
        {
            var OriginalImgString = advertisementService.GetByID(Convert.ToInt32(ad.AdID)).Picture;
            ViewBag.ImgString = OriginalImgString;
            if (ModelState.IsValid)
            {
                if (files[0] != null && files[0].ContentLength > 0)
                {
                    ad.Picture = await ImgurService.Upload(files[0]);
                }
                else
                {
                    ad.Picture = OriginalImgString;
                }

                var result = advertisementService.Update(ad);
                if (result.IsSuccessful)
                {
                    TempData["message"] = "Update Success";
                }
                else
                {
                    var path = result.WriteLog();
                    TempData["message"] = "Update Error ref:" + path;
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}