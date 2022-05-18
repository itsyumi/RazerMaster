namespace RazerMasterLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductTags
    {
        [Key]
        public long TagID { get; set; }

        [Required(ErrorMessage = "Tag Name is necessary")]
        [StringLength(100)]
        public string TagName { get; set; }

        [StringLength(10)]
        public string TagColor { get; set; }

        public int? Status { get; set; }
    }
}
