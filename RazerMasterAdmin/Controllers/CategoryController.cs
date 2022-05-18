using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using RazerMasterLibrary.Models;
using RazerMasterAdmin.Services;


namespace RazerMasterAdmin.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryService categoryService;

        public CategoryController()
        {
            categoryService = new CategoryService();
        }


        [Authorize]
        public ActionResult Index()
        {
            var categories = categoryService.GetAll();
            return View(categories);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories category)
        {
            if (ModelState.IsValid)
            {
                var result = categoryService.Create(category);
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
            return View(category);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories category = categoryService.GetByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories category)
        {
            if (ModelState.IsValid)
            {
                var result = categoryService.Update(category);
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