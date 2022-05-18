using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories.Interface;
using RazerMasterLibrary.Repositories;
using RazerMasterAdmin.ViewModels;

namespace RazerMasterAdmin.Services
{
    public class OrderService
    {
        private IGenericRepository<Orders> orderRepository = new GenericRepository<Orders>();
        private IGenericRepository<OrderDetails> orderDetailRepository = new GenericRepository<OrderDetails>();
        private IGenericRepository<Coupons> couponRepository = new GenericRepository<Coupons>();
        public OperationResult Update(Orders instance)
        {
            var result = new OperationResult();

            try
            {
                orderRepository.Update(instance);
                result.IsSuccessful = true;

            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.exception = ex;
            }
            return result;

        }


        public Orders GetOrderByID(Guid orderID)
        {
            ////var coupons = couponRepository.Get();
            //var order = orderRepository.Get(x => x.OrderID == orderID);
            ////var coupon = couponRepository.GetAll().Where(c => c.Code == order.EventCoupon).ToList();
            return orderRepository.Get(x => x.OrderID == orderID);
        }
        public Coupons GetCouponCode(string code) 
        {
            return couponRepository.GetAll().FirstOrDefault(c => c.Code == code);
        }
        public List<Coupons> GetAllCoupons()
        {
            return couponRepository.GetAll().ToList();
        }
        public List<OrderViewModel> GetOrderDetailByID(Guid orderID)
        {
            var result = orderDetailRepository.GetAll().Where(x => x.OrderID == orderID).Select(
                p => new OrderViewModel
                {
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice,
                    Total = p.UnitPrice * p.Quantity
                }).ToList();

            return result;
        }

        public List<Orders> GetAll()
        {
            var result = orderRepository.GetAll().ToList();
            return result;
        }
    }
}