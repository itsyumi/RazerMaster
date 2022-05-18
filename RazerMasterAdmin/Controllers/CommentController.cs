using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RazerMasterLibrary.Repositories;


namespace RazerMasterAdmin.Controllers
{
    public class CommentController : Controller
    {
        public CommentRepository _repo;
        public CommentController()
        {
            _repo = new CommentRepository();
        }

        // GET: Comment
        [Authorize]
        public ActionResult Index()
        {
            var commentList = _repo.GetAllComment();
            return View(commentList);
        }
       
    }
}