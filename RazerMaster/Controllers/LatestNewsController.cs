using System.Web.Mvc;
using RazerMaster.Services;

namespace RazerMaster.Controllers
{
    public class LatestNewsController : Controller
    {
        private PromotionService _promotionService;
        public LatestNewsController()
        {
            _promotionService = new PromotionService();
        }

        // GET: LatestNews
        public ActionResult Promotions()
        {
            var promotionsVM = _promotionService.GetAllPromotions();
            return View(promotionsVM);
        }
        public ActionResult News()
        {
            return View();
        }
    }
}