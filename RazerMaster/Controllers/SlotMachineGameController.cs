using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RazerMasterLibrary.Repositories;
using RazerMasterLibrary.Models;

namespace RazerMaster.Controllers
{

    public class SlotMachineGameController : Controller
    {
        public CoinRepository _repo = new CoinRepository();

        // GET: SlotMachineGame
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult PlayCasino(decimal Quantity)
        {
            var msg = String.Empty;
            var userId = HttpContext.User.Identity.GetUserId();
            var checkPlayOrNot = _repo.CheckPlayCasinoGameOrNot(userId);

            //設定每日次數
            if (checkPlayOrNot > 2)
            {
                return Content("Played");
            }
            else
            {
                var coinLog = new Coins
                {
                    ID = Guid.NewGuid(),
                    UserID = userId,
                    Quantity = Quantity,
                    EventTime = DateTime.Now,
                    Status = 1,
                    EventType = 1,
                    ExpiredDate = null
                };

                var result = _repo.InsertCasinoGameLog(coinLog);
                if (result > 0)
                {
                    msg = "OK";
                }
                else
                {
                    msg = "WriteLog Error";
                }
                return Content(msg);

            }

        }

        public class Prize
        {
            public string Name { get; set; }
            public decimal GetCount { get; set; }
            public int Chance { get; set; }
            public string Img { get; set; }
        }

        public ActionResult SetPrizeData()
        {
            List<Prize> prizeList = new List<Prize>() {
                new Prize{ Name="RazerCoin $1",GetCount=1 ,Chance=300,Img="slot1.png" },
                new Prize{ Name="RazerCoin $2",GetCount=2 ,Chance=300,Img="slot2.png" },
                new Prize{ Name="RazerCoin $5",GetCount=5 ,Chance=300,Img="slot3.png" },
                new Prize{ Name="RazerCoin $10",GetCount=10 ,Chance=89,Img="slot4.png" },
                new Prize{ Name="RazerCoin $50",GetCount=50 ,Chance=10,Img="slot5.png" },
                new Prize{ Name="RazerCoin $100",GetCount=100 ,Chance=1,Img="slot6.png" },
             };


            return Json(prizeList, JsonRequestBehavior.AllowGet);
        }
    }
}