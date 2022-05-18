using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;

namespace RazerMaster.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Products> ProductNewCollection { get; set; }
        public IEnumerable<Products> ProductHotCollection { get; set; }
        public IEnumerable<ProductTags> TagCollection { get; set; }
        public IEnumerable<Advertisements> AdCollection { get; set; }
    }
}