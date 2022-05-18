using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RazerMasterAdmin.Functions
{
    public class Functions
    {
        public static decimal GetPaymentTotal(decimal coinDiscount, decimal couponDiscount, decimal total, bool isFixed)
        {
            //total = total - coinDiscount;
            if (isFixed)
            {
                total = total - couponDiscount - coinDiscount;
            }
            else
            {
                total = total - (total * (couponDiscount / 100)) - coinDiscount;
            }
            return total;
        }
        public static decimal GetDiscountTotal(decimal couponDiscount, decimal total, bool isFixed)
        {
            if (isFixed)
            {
                return couponDiscount;
            }
            else
            {
                return total * (couponDiscount / 100);
            }
        }
    }
}