using RazerMaster.Areas.Member.Models;
using RazerMaster.Areas.Member.ViewModels;
using System.Linq;

namespace RazerMaster.Areas.Member.Repositories
{
    public class MemberinfoRepository
    {
        private RazerMasterContext db = new RazerMasterContext();
        private MemberDetail memberDetail = null;
        public MemberinfoRepository(string id)
        {


            var user = db.Users.Where(x => x.Id == id).FirstOrDefault();
            memberDetail = new MemberDetail()
            {
                RealName = user.RealName,
                Address = user.Address,
                Number = user.PhoneNumber
            };


        }


        public string GetRealName()
        {
            return memberDetail.RealName;
        }
        public string GetAddress()
        {
            return memberDetail.Address;
        }
        public string GetNumber()
        {
            return memberDetail.Number;
        }


    }

}