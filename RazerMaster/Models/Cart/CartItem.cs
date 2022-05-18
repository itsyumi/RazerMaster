using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RazerMaster.Models.Cart
{
    public class CartItem
    {
        public long ProductID { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public decimal SalePrice { get; set; }

        public int Quantity { get; set; }

        //商品小計
        public decimal SubTotal
        {
            get { return this.SalePrice * this.Quantity; }
        }
    }
}