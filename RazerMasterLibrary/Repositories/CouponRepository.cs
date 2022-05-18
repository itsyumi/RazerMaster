
using RazerMasterLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RazerMasterLibrary.Repositories
{
    public class CouponRepository
    {
        private RazerMasterDataContext db = new RazerMasterDataContext();
        public List<Coupons> GetAllAvailableCoupons()
        {
            return db.Coupons.Where(x => DateTime.Now >= x.StartTime && DateTime.Now <= x.EndTime).OrderBy(x=>x.Discount).ToList();
        }

        public decimal ReturnCalculatedPrice(decimal originalPrice,Guid couponID)
        {
            var coupon = db.Coupons.Where(x => x.CouponID == couponID).FirstOrDefault();
            if (coupon.MinOrderSubtotal > originalPrice) { return 0; }
            if (coupon.Is_Fixed == true)
            {
                return originalPrice - coupon.Discount;
            }
            else
            {
                return originalPrice * (1 - coupon.Discount);
            }
        }
    }
}