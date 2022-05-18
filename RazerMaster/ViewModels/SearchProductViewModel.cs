using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;

namespace RazerMaster.ViewModels
{
    public class SearchProductViewModel
    {
        public IEnumerable<ProductViewModel> products { get; set; }
        public IEnumerable<ProductTags> productTags { get; set; }
    }
}