using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RazerMaster.Services;

namespace RazerMaster.Controllers
{
    public class HomeController : Controller
    {

        private ProductService _productService;
        private AdvertisementService _adService;
        private HomeService _homeService;

        public HomeController()
        {
            _productService = new ProductService();
            _adService = new AdvertisementService();
            _homeService = new HomeService();
        }

        public ActionResult Index()
        {
            var productNewList = _productService.GetAll().Where(y => y.TagID == 1).OrderBy(x => x.Sequence).ToList();
            var productHotList = _productService.GetAll().Where(y => y.TagID == 2).OrderBy(x => x.Sequence).ToList();
            var tagList = _productService.GetProductTagList();
            var adList = _adService.GetAll();

            var indexVM = _homeService.GetAllDisplayData(productNewList, productHotList, tagList, adList);
            return View(indexVM);
        }

    }
}