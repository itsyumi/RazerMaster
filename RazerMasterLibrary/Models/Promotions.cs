namespace RazerMasterLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;


    public partial class Promotions
    {
        [Key]
        public long EventID { get; set; }

        [Required(ErrorMessage = "Event Name is necessary")]
        public string EventName { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start Time is necessary")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End Time is necessary")]
        public DateTime EndTime { get; set; }

        public string Picture { get; set; }

        public int Status { get; set; }
    }
}
