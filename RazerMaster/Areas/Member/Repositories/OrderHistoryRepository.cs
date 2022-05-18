using RazerMasterLibrary.Models;
using System.Linq;


namespace RazerMaster.Areas.Member.Repositories
{
    public class OrderHistoryRepository
    {
        private static RazerMasterDataContext db = new RazerMasterDataContext();

        public static string GetProductPic(long id)
        {
            var pic = (db.Products.Where(x => x.ProductID == id).First().Picture.Split(','))[0];
            return pic;
        }
    }
}