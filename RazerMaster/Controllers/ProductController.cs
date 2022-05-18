using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Microsoft.AspNet.Identity;
using RazerMaster.ViewModels;
using RazerMasterLibrary.Models;
using RazerMaster.Services;

namespace RazerMaster.Controllers
{
    public class ProductController : Controller
    {
        private ProductService _productService;
        private CommentService _commentService;

        public ProductController()
        {
            _productService = new ProductService();
            _commentService = new CommentService();
        }

        // GET: Product
        public ActionResult Index(string cat)
        {
            var product = _productService.GetAll();
            var category = _productService.GetCategoryList();
            var productTag = _productService.GetProductTagList();

            ProductIndexViewModel productIndexVM = _productService.GetTotalProductList(product,category,productTag);

            if (cat != null)
            {
                ViewBag.routeParameter = Convert.ToString(RouteData.Values["cat"]);
            }

            return View(productIndexVM);
        }

        public ActionResult ProductDetail(int? id)
        {
            var product = _productService.GetByID(id);
            if (product == default(Products))
            {
                return RedirectToAction("Index","home");
            }
            else
            {
                ProductTags productTag = product.TagID == null ? null : _productService.GetProductTagById(product.TagID);

                List<CommentViewModel> commentList = _commentService.GetCommentListByProduct(id);
                var ratingCountList = _commentService.GetRatingScoreByProduct(id);
                ViewBag.RatingCountList = ratingCountList;
                var productDetailVM = _productService.GetProductDetail(product, productTag, commentList);
                return View(productDetailVM);
            }
        }

        [HttpPost]
        [Authorize] //登入會員才可留言
        public ActionResult AddComment(long ProductID, string Content, string Score)
        {
            //取得目前登入使用者Id
            var userId = HttpContext.User.Identity.GetUserId();

            var comment = new ProductComments()
            {
                ProductID = ProductID,
                Content = Content,
                UserID = userId,
                CreateDate = DateTime.Now,
                Score = Convert.ToDecimal(Score),
                Status = 0
            };

            var result = _commentService.Create(comment);
            if (result.IsSuccessful)
            {
                TempData["message"] = "Comment submitted successfully, please wait for verification";
            }
            else
            {
                var path = result.WriteLog();
                TempData["message"] = "Comment Error ref:" + path;
            }

            return RedirectToAction("ProductDetail", new { id = ProductID });

        }

        public ActionResult SearchProduct(string keyword)
        {
            var products = _productService.SearchProduct(keyword);
            var productTags = _productService.GetProductTagList();
            string searchText = products.Count > 0 ? "Search Result" : "Related products not found";
            var searchProductVM = _productService.GetSearchProductList(products, productTags);
            ViewBag.SearchText = searchText;

            return View(searchProductVM);
        }
    }
}