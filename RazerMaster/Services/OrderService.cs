using RazerMaster.ViewModels;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RazerMaster.Services
{
    public class OrderService
    {
      
        public GenericRepository<Orders> orderrepo = new GenericRepository<Orders>();
        public GenericRepository<OrderDetails> detailsrepo = new GenericRepository<OrderDetails>();
        RazerMasterDataContext db = new RazerMasterDataContext();
        public OrderHistoryViewModel GetOrderHistoryViewModel(string id)
        {
            OrderHistoryViewModel order = new OrderHistoryViewModel();
            var orderList = db.Orders.Where(x => x.UserID == id).OrderByDescending(x => x.OrderTime);
            var orderDetailList = db.OrderDetails.Where(x => orderList.Where(y => x.OrderID == y.OrderID).Any()).ToList();
            order.Order = orderList.ToList();
            order.OrderDetail = orderDetailList;
            return order;
        }
    }
}