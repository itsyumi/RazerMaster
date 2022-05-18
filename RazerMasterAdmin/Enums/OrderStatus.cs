using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RazerMasterAdmin.Enums
{
    public enum OrderStatus
    {
        Waiting = -2,
        Cancel = -1,
        Unpaid = 0,
        Done = 1,
        Delivered = 2
    }
}