using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;

namespace RazerMaster.ViewModels
{
    public class OrderHistoryViewModel
    {
        public List<Orders>  Order { get; set; }

        public List<OrderDetails> OrderDetail { get; set; }
       
        
    }
}