using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using RazerMasterLibrary.Models;
using RazerMasterAdmin.Services;


namespace RazerMasterAdmin.Controllers
{
    public class ProductTagController : Controller
    {
        private ProductTagService productTagService;


        public ProductTagController()
        {
            productTagService = new ProductTagService();
        }

        // GET: ProductTag
        [Authorize]
        public ActionResult Index()
        {
            var productTags = productTagService.GetAll();
            return View(productTags);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductTags productTag)
        {
            if (ModelState.IsValid)
            {

                var result = productTagService.Create(productTag);
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
            return View(productTag);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTags productTag = productTagService.GetByID(id);
            if (productTag == null)
            {
                return HttpNotFound();
            }
            return View(productTag);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductTags productTag)
        {
            if (ModelState.IsValid)
            {
                var result = productTagService.Update(productTag);
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