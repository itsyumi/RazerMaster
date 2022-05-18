using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RazerMaster.ViewModels
{
    public class ProductViewModel
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string Picture { get; set; }
        public int? TagID { get; set; }
        public int? CategoryID { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; }
    }
}