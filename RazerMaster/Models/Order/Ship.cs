using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace RazerMaster.Models.Order
{
    public class Ship
    {

        [Required(ErrorMessage = "Receiver is necessary")]
        [Display(Name = "Customer Name")]
        public string RecieverName { get; set; }

        [Required(ErrorMessage = "Receiver phone is necessary")]
        [Display(Name = "Phone")]
        public string RecieverPhone { get; set; }

        [Required(ErrorMessage = "Receiver Address is necessary")]
        [Display(Name = "Address")]
        public string RecieverAddress { get; set; }

        [Required(ErrorMessage = "Receiver Email is necessary")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string RecieverEmail { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }
        public int ShipWay { get; set; }
        public int PaymentWay { get; set; }

        public string Code { get; set; }

        public bool CoinDiscount { get; set; }

    }
}