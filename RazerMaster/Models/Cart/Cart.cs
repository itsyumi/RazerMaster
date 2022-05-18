using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RazerMasterLibrary.Models;


namespace RazerMaster.Models.Cart
{
    public class Cart : IEnumerable<CartItem>
    {
        //儲存所有商品
        private List<CartItem> cartItems;
        private RazerMasterDataContext db = new RazerMasterDataContext();

        //建構子
        public Cart()
        {
            this.cartItems = new List<CartItem>();
        }

        public int Count
        {
            get
            {
                return this.cartItems.Count;
            }
        }

        public decimal TotalAmount
        {
            get
            {
                decimal totalAmount = 0.0m;
                foreach (var cartItem in this.cartItems)
                {
                    totalAmount += cartItem.SubTotal;
                }
                return totalAmount;
            }
        }


        //新增一筆Product，使用ProductId
        public bool AddProduct(int ProductId,int count,string type)
        {
            var findItem = this.cartItems
                            .Where(s => s.ProductID == ProductId)
                            .Select(s => s)
                            .FirstOrDefault();

            //判斷相同Id的CartItem是否已經存在購物車內
            if (findItem == default(CartItem))
            {   //不存在購物車內，則新增一筆

                var product = (from s in db.Products
                               where s.ProductID == ProductId
                               select s).FirstOrDefault();
                if (product != default(Products))
                {
                    this.AddProduct(product,count);
                }

            }
            else
            {   //存在購物車內，則將商品數量累加
                if (type.Equals("addMany"))//直接輸入數字就直接取代原有數字
                {
                    findItem.Quantity = count;
                }
                else
                {
                    findItem.Quantity += count;
                }
            }
            return true;
        }




        //新增一筆Product，使用Product物件
        private bool AddProduct(Products product,int count)
        {
            //將Product轉為CartItem
            var cartItem = new CartItem()
            {
                ProductID = product.ProductID,
                Name = product.ProductName,
                ImgUrl = product.Picture.Split(',')[0],
                SalePrice = product.SalePrice,
                Quantity = count
            };

            //加入CartItem至購物車
            this.cartItems.Add(cartItem);
            return true;
        }

        public bool RemoveProduct(int ProductId)
        {
            var targetItem = this.cartItems.Where(x => x.ProductID == ProductId).Select(x => x).FirstOrDefault();

            if (targetItem != default(CartItem))
            {
                this.cartItems.Remove(targetItem);
            }
            return true;
        }

        public bool ClearCart()
        {
            this.cartItems.Clear();
            return true;
        }

        //將購物車商品轉成OrderDetail的List
        public List<OrderDetails> ToOrderDetailList(Guid orderId)
        {
            var result = new List<OrderDetails>();
            foreach (var cartItem in this.cartItems)
            {
                result.Add(new OrderDetails()
                {
                    OrderDetailID = Guid.NewGuid(),
                    ProductName = cartItem.Name,
                    UnitPrice = cartItem.SalePrice,
                    Quantity = cartItem.Quantity,
                    OrderID = orderId,
                    ProductID = cartItem.ProductID
                });
            }
            return result;
        }

        #region IEnumerator

        public IEnumerator<CartItem> GetEnumerator()
        {
            return ((IEnumerable<CartItem>)cartItems).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)cartItems).GetEnumerator();
        }
        #endregion
    }
}