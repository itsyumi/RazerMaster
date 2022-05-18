namespace RazerMasterLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductComments
    {
        [Key]
        public int ID { get; set; }

        public string UserID { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public long ProductID { get; set; }

        public decimal Score { get; set; }

        public int Status { get; set; }
    }
}
