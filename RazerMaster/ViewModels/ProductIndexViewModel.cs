using RazerMasterLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RazerMaster.ViewModels
{
    public class ProductIndexViewModel
    {
        public IEnumerable<Products> ProductCollection { get; set; }

        public IEnumerable<Categories> CategoryCollection { get; set; }

        public IEnumerable<ProductTags> ProductTagCollection { get; set; }
    }
}