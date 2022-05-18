
using RazerMasterLibrary.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace RazerMasterLibrary.Models
{

    public class Coupons
    {
        [Display(Name = "Coupon Id")]
        public Guid CouponID { get; set; }

        [Key]
        [Required]
        [StringLength(50)]
        [UniqueKey(ErrorMessage = "The coupon code already exists.")]
        public string Code { get; set; }

        [StringLength(300), DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        public decimal Discount { get; set; }

        public bool Is_Fixed { get; set; }
        // Whether or not the discount is a percentage or a fixed price.

        [GreaterThan("Discount")]
        [Display(Name = "MOS")]
        public decimal MinOrderSubtotal { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
    }


}
