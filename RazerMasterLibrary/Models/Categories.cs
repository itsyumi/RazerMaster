namespace RazerMasterLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Categories
    {
        [Key]
        public long CategoryID { get; set; }

        [Required(ErrorMessage ="Category Name is necessary")]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public int? Status { get; set; }

        public int? Sequence { get; set; }

        [StringLength(300), DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
