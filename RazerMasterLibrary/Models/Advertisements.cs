namespace RazerMasterLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Advertisements
    {
        [Key]
        public long AdID { get; set; }

        [Required(ErrorMessage = "AD Name is necessary")]
        [StringLength(50)]
        public string AdName { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start Time is necessary")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End Time is necessary")]
        public DateTime EndTime { get; set; }

        public string Picture { get; set; }

        public int Status { get; set; }
    }
}
