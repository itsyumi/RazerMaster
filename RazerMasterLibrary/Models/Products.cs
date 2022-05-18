namespace RazerMasterLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;


    public partial class Products
    {
        [Key]
        public long ProductID { get; set; }

        [Required(ErrorMessage = "Product Name is necessary"), StringLength(100)]
        public string ProductName { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

        public int? Status { get; set; }

        public string Picture { get; set; }

        [Required(ErrorMessage = "Start Time is necessary")]
        public DateTime StartSellTime { get; set; }
        [Required(ErrorMessage = "End Time is necessary")]
        public DateTime EndSellTime { get; set; }

        [StringLength(300), DataType(DataType.MultilineText)]
        public string Note { get; set; }

        public int Sequence { get; set; }

        public int? CategoryID { get; set; }

        public int? TagID { get; set; }

        [Required(ErrorMessage = "Original Price is necessary")]
        public decimal OriginalPrice { get; set; }
        [Required(ErrorMessage = "Sale Price is necessary")]
        public decimal SalePrice { get; set; }

        public int Stock { get; set; }
    }
}
