using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RazerMasterLibrary.Models;
using RazerMasterAdmin.Services;

namespace RazerMasterAdmin.Controllers
{
    public class PromotionController : Controller
    {
        private PromotionSerivce promotionService;

        public PromotionController()
        {
            promotionService = new PromotionSerivce();
        }



        // GET: Promotions
        [Authorize]
        public ActionResult Index()
        {
            var promotions = promotionService.GetAll();
            return View(promotions);
        }

        // GET: Promotions/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Promotions promotion, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                if (files[0] != null && files[0].ContentLength > 0)
                {
                    promotion.Picture = await ImgurService.Upload(files[0]);

                    var result = promotionService.Create(promotion);
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
            return View(promotion);
        }

        // GET: Promotions/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotions promotion = promotionService.GetByID(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImgString = promotion.Picture;
            return View(promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Promotions promotion, HttpPostedFileBase[] files)
        {
            var OriginalImgString = promotionService.GetByID(Convert.ToInt32(promotion.EventID)).Picture;
            ViewBag.ImgString = OriginalImgString;
            if (ModelState.IsValid)
            {
                if (files[0] != null && files[0].ContentLength > 0)
                {
                    promotion.Picture = await ImgurService.Upload(files[0]);
                }
                else
                {
                    promotion.Picture = OriginalImgString;
                }

                var result = promotionService.Update(promotion);
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
