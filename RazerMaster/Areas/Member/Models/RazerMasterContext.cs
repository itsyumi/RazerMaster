namespace RazerMaster.Areas.Member.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;


    public class RazerMasterContext : IdentityDbContext<ApplicationUser>
    {
        // 您的內容已設定為使用應用程式組態檔 (App.config 或 Web.config)
        // 中的 'RazerMaster' 連接字串。根據預設，這個連接字串的目標是
        // 您的 LocalDb 執行個體上的 'RazerMaster.Areas.Member.Models.RazerMaster' 資料庫。
        // 
        // 如果您的目標是其他資料庫和 (或) 提供者，請修改
        // 應用程式組態檔中的 'RazerMaster' 連接字串。

        public RazerMasterContext()
              : base("RazerMaster", throwIfV1Schema: false)
        {
        }

        //public RazerMasterContext()
        //     : base("razermasterformal", throwIfV1Schema: false)
        //{
        //}

        public static RazerMasterContext Create()
        {
            return new RazerMasterContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<IdentityUser>()
                .ToTable("Users");
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users");
        }
        // 針對您要包含在模型中的每種實體類型新增 DbSet。如需有關設定和使用
        // Code First 模型的詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}