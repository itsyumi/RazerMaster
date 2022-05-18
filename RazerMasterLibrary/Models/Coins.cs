namespace RazerMasterLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Coins
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string UserID { get; set; }

        public decimal Quantity { get; set; }

        public DateTime EventTime { get; set; }

        public int Status { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public int EventType { get; set; }
    }
}
