using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RazerMasterLibrary.Models;


namespace RazerMasterAdmin.Controllers
{
    public class StatusChangeController : Controller
    {
        private RazerMasterDataContext db = new RazerMasterDataContext();

        // GET: StatusChange
        public ActionResult ChangeStatus(int? id, string modelName)
        {
            switch (modelName)
            {
                case "Product":
                    var product = db.Products.Find(id);
                    product.Status = product.Status == 1 ? 0 : 1;
                    break;
                case "Category":
                    var category = db.Categories.Find(id);
                    category.Status = category.Status == 1 ? 0 : 1;
                    break;
                case "ProductTag":
                    var productTag = db.ProductTags.Find(id);
                    productTag.Status = productTag.Status == 1 ? 0 : 1;
                    break;
                case "Promotion":
                    var promotion = db.Promotions.Find(id);
                    promotion.Status = promotion.Status == 1 ? 0 : 1;
                    break;
                case "Advertisement":
                    var ads = db.Advertisements.Find(id);
                    ads.Status = ads.Status == 1 ? 0 : 1;
                    break;
                case "Comment":
                    var pc = db.ProductComments.Find(id);
                    pc.Status = pc.Status == 1 ? 0 : 1;
                    break;
                default:
                    break;
            };

            db.SaveChanges();
            return RedirectToAction("Index", modelName);
        }
    }
}