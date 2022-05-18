using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using RazerMasterLibrary.Models;
using RazerMasterAdmin.Services;

namespace RazerMasterAdmin.Controllers
{

    public class ProductController : Controller
    {

        private ProductService productService;
        private CategoryService categoryService;
        private ProductTagService productTagService;

        public IEnumerable<Categories> Categories
        {
            get
            {
                return categoryService.GetAll().Where(x => x.Status == 1);
            }
        }

        public IEnumerable<ProductTags> ProductTags
        {
            get
            {
                return productTagService.GetAll().Where(x => x.Status == 1);
            }
        }

        public ProductController()
        {
            productService = new ProductService();
            categoryService = new CategoryService();
            productTagService = new ProductTagService();
        }


        [Authorize]
        public ActionResult Index()
        {
            var products = productService.GetAll();
            return View(products);
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(Categories, "CategoryID", "CategoryName");
            ViewBag.TagList = new SelectList(ProductTags, "TagID", "TagName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Products product, HttpPostedFileBase[] files)
        {

            ViewBag.CategoryList = new SelectList(Categories, "CategoryID", "CategoryName");
            ViewBag.TagList = new SelectList(ProductTags, "TagID", "TagName");

            if (ModelState.IsValid)
            {
                if (files[0] != null)
                {
                    var imgName = String.Empty;

                    foreach (var file in files)
                    {
                        if (file.ContentLength > 0)
                        {
                            imgName += await ImgurService.Upload(file) + ",";
                        }
                    }
                    product.Picture = imgName.Substring(0, imgName.Length - 1);


                    var result = productService.Create(product);

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
                    ViewBag.ErrorMsg = "請上傳至少一張圖片";
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Products product = productService.GetByID(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryList = new SelectList(Categories, "CategoryID", "CategoryName");
            ViewBag.TagList = new SelectList(ProductTags, "TagID", "TagName");
            ViewBag.ImgString = product.Picture;
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Products product, HttpPostedFileBase[] files)
        {

            ViewBag.CategoryList = new SelectList(Categories, "CategoryID", "CategoryName");
            ViewBag.TagList = new SelectList(ProductTags, "TagID", "TagName");

            var OriginalImgString = productService.GetByID(Convert.ToInt32(product.ProductID)).Picture;
            ViewBag.ImgString = OriginalImgString;


            if (ModelState.IsValid)
            {
                if (files[0] != null)
                {
                    var imgName = String.Empty;

                    foreach (var file in files)
                    {
                        if (file.ContentLength > 0)
                        {
                            imgName += await ImgurService.Upload(file) + ",";
                        }
                    }
                    product.Picture = imgName.Substring(0, imgName.Length - 1);
                }
                else
                {
                    product.Picture = OriginalImgString;
                }

                var result = productService.Update(product);
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
            return View(product);
        }

    }
}