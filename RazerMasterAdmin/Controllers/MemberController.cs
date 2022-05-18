using RazerMasterAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RazerMasterLibrary.Repositories;
using RazerMasterLibrary.Models;

namespace RazerMasterAdmin.Controllers
{

    public class MemberController : Controller
    {
        private MemberRepository memberRepo;

        public MemberController()
        {
            memberRepo = new MemberRepository();
        }

        // GET: Member
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            var member = memberRepo.GetAllMember();
            return View(member);
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            var memberDetail = memberRepo.MemberDetail(id);
            return View(memberDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users member)
        {
             memberRepo.UpdateMemberDetail(member);
             TempData["message"] = "Update Success";
             return RedirectToAction("Index");
        }

    }
}