namespace RazerMasterLibrary.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RazerMasterDataContext : DbContext
    {
        public RazerMasterDataContext()
            : base("name=RazerMaster")
        {
        }

        //public RazerMasterDataContext()
        //    : base("name=RazerMasterFormal")
        //{
        //}

        public virtual DbSet<AdminUsers> AdminUsers { get; set; }
        public virtual DbSet<Advertisements> Advertisements { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Coins> Coins { get; set; }
        public virtual DbSet<Coupons> Coupons { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<ProductComments> ProductComments { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductTags> ProductTags { get; set; }
        public virtual DbSet<Promotions> Promotions { get; set; }
        public virtual DbSet<Users> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
