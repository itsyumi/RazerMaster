using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RazerMasterLibrary.Repositories;


namespace RazerMasterAdmin.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {

        private ReportRepository reportRepo;

        public ReportController()
        {
            reportRepo = new ReportRepository();
        }

        // GET: Report
        public ActionResult Sale()
        {
            var defaultData = reportRepo.GetProductReportByTime(new DateTime(1753, 1, 2), DateTime.Now);
            ViewBag.name = "ProductName";
            return View(defaultData);
        }
        public ActionResult SearchData(string filter, DateTime? start, DateTime? end)
        {
            List<dynamic> searchResult = null;
            if (start == null || end == null)
            {
                return RedirectToAction("Sale");
            }
            switch (filter)
            {
                case "Product":
                    searchResult = reportRepo.GetProductReportByTime(start, end);
                    ViewBag.name = "ProductName";
                    break;
                case "Category":
                    searchResult = reportRepo.GetCategroyReportByTime(start, end);
                    ViewBag.name = "CategoryName";
                    break;
            }
            return View("Sale", searchResult);
        }

        public ActionResult Member()
        {
            ViewBag.ByYear = reportRepo.GetMemberReportByYear();
            ViewBag.ByMonth = reportRepo.GetMemberReportByMonth();
            return View();
        }
    }
}