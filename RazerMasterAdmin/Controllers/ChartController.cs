using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RazerMasterLibrary.Repositories;


namespace RazerMasterAdmin.Controllers
{
    public class ChartController : Controller
    {

        private ChartRepository _repo;
        public ChartController()
        {
           
            _repo = new ChartRepository();
        }

        // GET: Chart
        public ActionResult Index()
        {
            ViewBag.reMonthANLS = _repo.LineCharted();
            ViewBag.PieMember = _repo.PieMembered();
            ViewBag.LineCount = _repo.LineCounted();
            ViewBag.LineTotal = _repo.LineTotaled();

            return View();
        }
    }
}