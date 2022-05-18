using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RazerMaster.ViewModels
{
    public class CommentViewModel
    {
        public string NickName { get; set; }
        public decimal Score { get; set; }
        public string CreateDate { get; set; }
        public string Content { get; set; }
    }
}