using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;

namespace RazerMaster.ViewModels
{
    public class ProductDetailViewModel
    {
        public Products product { get; set; }
        public ProductTags productTag { get; set; }
        public IEnumerable<CommentViewModel> comments { get; set; }

    }
}