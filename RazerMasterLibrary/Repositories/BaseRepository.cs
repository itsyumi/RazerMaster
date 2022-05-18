using System.Configuration;

namespace RazerMasterLibrary.Repositories
{
    public abstract class BaseRepository
    {
        public string ConnectionString { get; set; }

        public BaseRepository()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["RazerMaster"].ConnectionString;
            //this.ConnectionString = ConfigurationManager.ConnectionStrings["RazerMasterFormal"].ConnectionString;

        }
    }
}