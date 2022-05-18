using RazerMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RazerMaster.Helper
{
    public class OrderHelper
    {
        private List<SelectListItem> _shipWayOptions;
        private List<SelectListItem> _paymentOptions;
        private Dictionary<int, string> _payments;
        public OrderHelper()
        {
            _shipWayOptions = new List<SelectListItem>()
        {
            new SelectListItem{ Text="CVS",Value="1"},
            new SelectListItem{ Text="POST",Value="2"},
            new SelectListItem{ Text="Home delivery",Value="3"}
        };

            _paymentOptions = new List<SelectListItem>()
        {
            new SelectListItem{ Text="Credit Card",Value="1"},
            new SelectListItem{ Text="Direct bank transfer",Value="2"},
            new SelectListItem{ Text="ATM",Value="3"},
            new SelectListItem{ Text="Business code",Value="4"},
            new SelectListItem{ Text="Business Barcode",Value="5"},
        };
            _payments = new Dictionary<int, string>()
        {
            {  1,"Credit" },
            {  2, "WebATM" },
            {  3,"ATM" },
            {  4,"CVS" },
            {  5,"BARCODE" }
        };
        }
        public List<SelectListItem> GetShipWayOptions()
        {
            return _shipWayOptions;
        }
        public List<SelectListItem> GetPaymentOptions()
        {
            return _paymentOptions;
        }
        public Dictionary<int, string> GetPayments()
        {
            return _payments;
        }
        public string GetPayments(int choose)
        {
            return _payments[choose];
        }

        public HostViewModel GetRoute(string hostname)
        {
            var route = new HostViewModel();
            route.urlCompleted = $"https://{hostname}/Order/CallBack";
            route.urlSucess = $"https://{hostname}/Order/OrderSucess";
            route.hosturl = $"https://{hostname}";
         
            //$"https://{hostname}";
            //route.urlCompleted = $"https://3703c41ff319.ngrok.io/Order/CallBack";
            //route.urlSucess = $"https://3703c41ff319.ngrok.io/Order/OrderSucess";
            //route.hosturl = $"https://3703c41ff319.ngrok.io"; //$"https://{hostname}";
            return route;
        }
    }
}