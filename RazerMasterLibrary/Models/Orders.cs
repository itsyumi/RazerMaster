namespace RazerMasterLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders
    {
        [Key]
        public Guid OrderID { get; set; }
        public string GreenID { get; set; }
        public string UserID { get; set; }

        public DateTime OrderTime { get; set; }

        public decimal TotalPrice { get; set; }

        public string CustomerName { get; set; }
        [Required]
        public string CustomerTel { get; set; }
        [Required]
        public string CustomerAddress { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public int Status { get; set; }

        public string Note { get; set; }

        public string EventCoupon { get; set; }
        public decimal EventDiscount { get; set; }

        public decimal CoinDiscount { get; set; }
        public string PaymentNo { get; set; }

        public int ShipWay { get; set; }
        public int PaymentWay { get; set; }
    }
}
