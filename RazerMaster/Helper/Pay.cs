using ECPay.Payment.Integration;
using log4net;
using RazerMaster.Models.Cart;
using RazerMaster.Models.Order;
using RazerMaster.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RazerMasterLibrary.Repositories;
using RazerMasterLibrary.Models;

namespace RazerMaster.Services
{
    public class Pay
    {
        static ILog logger = LogManager.GetLogger("Web");
        public CoinRepository _repo = new CoinRepository();
        public GenericRepository<Orders> orderrepo = new GenericRepository<Orders>();
        public GenericRepository<Coupons> couponrepo = new GenericRepository<Coupons>();
        public GenericRepository<OrderDetails> orderDetailrepo = new GenericRepository<OrderDetails>();
        public string ProcessGreenPay(Guid OrderId, HostViewModel route)
        {
            //綠界訂單編號
            string TradeNo = "RaM" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Decimal total = 0;
            List<string> enErrors = new List<string>();
            string szErrorMessage = String.Empty;
            AllInOne oPayment = new AllInOne();

            
            
            List<Item> items = new List<Item>();
            decimal CoinDiscount = 0;
            
            var Order = orderrepo.Get(x => x.OrderID == OrderId);
            Order.GreenID = TradeNo;
            var coupon = couponrepo.Get(x => x.Code == Order.EventCoupon);
            var id = Order.UserID;
            var details = orderDetailrepo.GetAll().Where(x => x.OrderID == OrderId).ToList();
            route.PaymentWay = Order.PaymentWay;
            CoinDiscount = Order.CoinDiscount;
            //using (RazerMasterDataContext db = new RazerMasterDataContext())
            //{
                //var Order = db.Orders.Where(x => x.OrderID == OrderId).FirstOrDefault();
                //Order.GreenID = TradeNo;
                //coupon = db.Coupons.Where(x => x.Code == Order.EventCoupon).FirstOrDefault();
                //id = Order.UserID;
                //details = db.OrderDatails.Where(x => x.OrderID == OrderId).ToList();
                //route.PaymentWay = Order.PaymentWay;
                //CoinDiscount = Order.CoinDiscount;
                //db.SaveChanges();
            //}
            foreach (var item in details)
            {
                oPayment.Send.Items.Add(new Item()
                {
                    Name = item.ProductName,//商品名稱
                    Price = item.UnitPrice,//商品單價
                    Currency = "US",//幣別單位
                    Quantity = item.Quantity//購買數量

                });
                total += item.UnitPrice * item.Quantity;
            }

            if (coupon != null)
            {
                var itemcoupon = new Item();

                var desc = String.Empty;
                if (coupon.Is_Fixed)
                    desc = "-" + coupon.Discount;
                else
                    desc = coupon.Discount + "%";

                itemcoupon.Name = "活動券 " + desc;//商品名稱
                itemcoupon.Quantity = 1;//商品名稱

                oPayment.Send.Items.Add(itemcoupon);
                CouponRepository couponRepository = new CouponRepository();
                total = couponRepository.ReturnCalculatedPrice(total, coupon.CouponID);
            }

            if (CoinDiscount != 0)
            {
                oPayment.Send.Items.Add(new Item()
                {
                    Name = "雷蛇幣折扣",//商品名稱
                    Price = -CoinDiscount,//商品單價
                    Currency = "US",//幣別單位
                    Quantity = 1//購買數量

                });
                total -= CoinDiscount;
            }


            try
            {
                /* 服務參數 */
                oPayment.ServiceMethod = HttpMethod.HttpPOST;//介接服務時，呼叫 API 的方法
                oPayment.ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";//要呼叫介接服務的網址
                oPayment.HashKey = "5294y06JbISpM5x9";//ECPay提供的Hash Key
                oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay提供的Hash IV
                oPayment.MerchantID = "2000132";//ECPay提供的特店編號


                /* 基本參數 */
                oPayment.Send.ReturnURL = route.urlCompleted;//付款完成通知回傳的網址
                oPayment.Send.ClientBackURL = route.hosturl;//瀏覽器端返回的廠商網址
                oPayment.Send.OrderResultURL = "";//瀏覽器端回傳付款結果網址

                oPayment.Send.MerchantTradeNo = TradeNo;//廠商的交易編號
                oPayment.Send.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");//廠商的交易時間
                oPayment.Send.TotalAmount = Decimal.Round(total) * 30;//交易總金額
                oPayment.Send.TradeDesc = "交易描述";//交易描述
                oPayment.Send.ChoosePayment = GetMethod(route.PaymentWay);//使用的付款方式
                oPayment.Send.Remark = "";//備註欄位
                oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;//使用的付款子項目
                oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.No;//是否需要額外的付款資訊
                oPayment.Send.DeviceSource = DeviceType.PC;//來源裝置
                oPayment.Send.IgnorePayment = ""; //不顯示的付款方式
                oPayment.Send.PlatformID = "";//特約合作平台商代號
                oPayment.Send.HoldTradeAMT = HoldTradeType.Yes;
                oPayment.Send.CustomField1 = id;
                oPayment.Send.CustomField2 = "";
                oPayment.Send.CustomField3 = "";
                oPayment.Send.CustomField4 = "";
                oPayment.Send.EncryptType = 1;

                //訂單的商品資料
                //foreach (var item in items)
                //{
                //    oPayment.Send.Items.Add(item);
                //}
                //oPayment.Send.Items.Add(itemcoupon);
                /*************************非即時性付款:ATM、CVS 額外功能參數**************/

                #region ATM 額外功能參數

                //oPayment.SendExtend.ExpireDate = 3;//允許繳費的有效天數
                //oPayment.SendExtend.PaymentInfoURL = "https://e1dd69646cec.ngrok.io/Order/OrderSucess";//伺服器端回傳付款相關資訊
                oPayment.SendExtend.ClientRedirectURL = route.urlSucess;//Client 端回傳付款相關資訊

                #endregion


                #region CVS 額外功能參數

                //oPayment.SendExtend.StoreExpireDate = 3; //超商繳費截止時間 CVS:以分鐘為單位 BARCODE:以天為單位
                //oPayment.SendExtend.Desc_1 = "test1";//交易描述 1
                //oPayment.SendExtend.Desc_2 = "test2";//交易描述 2
                //oPayment.SendExtend.Desc_3 = "test3";//交易描述 3
                //oPayment.SendExtend.Desc_4 = "";//交易描述 4
                //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
                //oPayment.SendExtend.ClientRedirectURL = "";///Client 端回傳付款相關資訊

                #endregion

                /***************************信用卡額外功能參數***************************/

                #region Credit 功能參數

                //oPayment.SendExtend.BindingCard = BindingCardType.No; //記憶卡號
                //oPayment.SendExtend.MerchantMemberID = ""; //記憶卡號識別碼
                //oPayment.SendExtend.Language = "ENG"; //語系設定

                #endregion Credit 功能參數

                #region 一次付清

                //oPayment.SendExtend.Redeem = false;   //是否使用紅利折抵
                //oPayment.SendExtend.UnionPay = true; //是否為銀聯卡交易

                #endregion

                #region 分期付款

                //oPayment.SendExtend.CreditInstallment = 3;//刷卡分期期數

                #endregion 分期付款

                #region 定期定額

                //oPayment.SendExtend.PeriodAmount = 1000;//每次授權金額
                //oPayment.SendExtend.PeriodType = PeriodType.Day;//週期種類
                //oPayment.SendExtend.Frequency = 1;//執行頻率
                //oPayment.SendExtend.ExecTimes = 2;//執行次數
                //oPayment.SendExtend.PeriodReturnURL = "";//伺服器端回傳定期定額的執行結果網址。

                #endregion


                /* 產生訂單 */
                enErrors.AddRange(oPayment.CheckOut());
                enErrors.AddRange(oPayment.CheckOutString(ref szErrorMessage));


            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                // 顯示錯誤訊息。
                if (enErrors.Count() > 0)
                {
                    szErrorMessage = String.Join("\\r\\n", enErrors);
                }
            }
            logger.Info("送訂單給綠界");
            return szErrorMessage;
        }
        public String FeekbackGreenPay()
        {
            string result = String.Empty;
            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    oPayment.HashKey = "5294y06JbISpM5x9";//ECPay 提供的 HashKey
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay 提供的 HashIV
                    /* 取回付款結果 */
                    enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
                }
                // 取回所有資料
                if (enErrors.Count() == 0)
                {

                    /* 支付後的回傳的基本參數 */
                    string szMerchantID = String.Empty;
                    string szMerchantTradeNo = String.Empty;
                    string szPaymentDate = String.Empty;
                    string szPaymentType = String.Empty;
                    string szPaymentNo = String.Empty;
                    string szCustomField1 = String.Empty;
                    string szPaymentTypeChargeFee = String.Empty;
                    string szRtnCode = String.Empty;
                    string szRtnMsg = String.Empty;
                    string szSimulatePaid = String.Empty;
                    string szTradeAmt = String.Empty;
                    string szTradeDate = String.Empty;
                    string szTradeNo = String.Empty;
                    // 取得資料
                    foreach (string szKey in htFeedback.Keys)
                    {
                        switch (szKey)
                        {
                            /* 支付後的回傳的基本參數 */
                            case "MerchantTradeNo": szMerchantTradeNo = htFeedback[szKey].ToString(); break;
                            case "PaymentNo": szPaymentNo = htFeedback[szKey].ToString(); break;

                            default: break;
                        }
                    }
                    logger.Info(szMerchantTradeNo);
                    var ChangeOrder = orderrepo.Get(x => x.GreenID == szMerchantTradeNo);
                    ChangeOrderStatus(szMerchantTradeNo, 0);
                    ChangeOrder.PaymentNo = szPaymentNo;
                    orderrepo.Update(ChangeOrder);
                    //using (RazerMasterDataContext db = new RazerMasterDataContext())
                    //{
                    //    var ChangeOrder = db.Orders.Where(x => x.GreenID == szMerchantTradeNo).FirstOrDefault();
                    //    ChangeOrder.PaymentNo = szPaymentNo;
                    //    ChangeOrder.Status = 0;
                    //    db.SaveChanges();
                    //}


                }
                else
                {
                    // 其他資料處理。
                }

            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                // 回覆成功訊息。
                if (enErrors.Count() == 0)
                    result = "1|OK";
                // 回覆錯誤訊息。
                else
                    result = (String.Format("0|{0}", String.Join("\\r\\n", enErrors)));

            }
            return result;
        }

        public String PaymentCompleted()
        {
            string result = String.Empty;
            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    oPayment.HashKey = "5294y06JbISpM5x9";//ECPay 提供的 HashKey
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay 提供的 HashIV
                    /* 取回付款結果 */
                    enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
                }
                // 取回所有資料
                if (enErrors.Count() == 0)
                {

                    /* 支付後的回傳的基本參數 */

                    string szMerchantTradeNo = String.Empty;
                    string szRtnCode = String.Empty;
                    // 取得資料
                    foreach (string szKey in htFeedback.Keys)
                    {
                        switch (szKey)
                        {
                            /* 支付後的回傳的基本參數 */

                            case "MerchantTradeNo": szMerchantTradeNo = htFeedback[szKey].ToString(); break;
                            case "RtnCode": szRtnCode = htFeedback[szKey].ToString(); break;
                            default: break;
                        }
                    }
                    logger.Info(szMerchantTradeNo);
                    if (szRtnCode == "1")
                    {
                       
                        ChangeOrderStatus(szMerchantTradeNo, 1);
                        //using (RazerMasterDataContext db = new RazerMasterDataContext())
                        //{
                        //    var ChangeOrder = db.Orders.Where(x => x.GreenID == szMerchantTradeNo).FirstOrDefault();
                        //    ChangeOrder.Status = 1;
                        //    db.SaveChanges();
                        //}
                    }


                }
                else
                {
                    // 其他資料處理。
                }

            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                // 回覆成功訊息。
                if (enErrors.Count() == 0)
                    result = "1|OK";
                // 回覆錯誤訊息。
                else
                    result = (String.Format("0|{0}", String.Join("\\r\\n", enErrors)));

            }
            return result;
        }

        public Guid CreateOrder(string id, Cart currentcart, Ship ship, decimal coinDiscount)
        {
            //取得目前登入使用者Id
            var userId = id;
            var orderId = Guid.NewGuid();

            //建立Order物件
            var order = new Orders()
            {
                OrderID = orderId,
                UserID = userId,
                OrderTime = DateTime.Now,
                TotalPrice = currentcart.TotalAmount,
                CustomerName = ship.RecieverName,
                CustomerTel = ship.RecieverPhone,
                CustomerAddress = ship.RecieverAddress,
                CustomerEmail = ship.RecieverEmail,
                Status = -2,
                Note = ship.Note,
                EventCoupon = ship.Code,
                EventDiscount = 0,
                ShipWay = ship.ShipWay,
                PaymentWay = ship.PaymentWay,
                CoinDiscount = coinDiscount
            };
            Coins coinLog = new Coins();
            if (coinDiscount != 0)
            {
                //雷蛇幣花費紀錄
                coinLog = new RazerMasterLibrary.Models.Coins
                {
                    ID = Guid.NewGuid(),
                    UserID = userId,
                    Quantity = coinDiscount,
                    EventTime = DateTime.Now,
                    Status = 0,
                    EventType = 2,
                    ExpiredDate = DateTime.Now
                };
            }


            //寫進資料庫
            //取得購物車中OrderDetail物件
            var orderDetails = currentcart.ToOrderDetailList(order.OrderID);
            using (RazerMasterDataContext db = new RazerMasterDataContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (coinDiscount != 0)
                            _repo.InsertCasinoGameLog(coinLog);  //寫入Coin紀錄


                        //加其入Orders資料表後，儲存變更
                        db.Orders.Add(order);
                        db.SaveChanges();

                        //將其加入OrderDetails資料表後，儲存變更
                        db.OrderDetails.AddRange(orderDetails);
                        db.SaveChanges();



                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();

                    }
                }
            }
            return orderId;
        }

        public bool CheckOrder(Guid OrderId)
        {
            bool isExist = false;
            
            var ChangeOrder = orderrepo.Get(x => x.OrderID == OrderId);
            var MerchantTradeNo = ChangeOrder.GreenID;
            //using (RazerMasterDataContext db = new RazerMasterDataContext())
            //{
            //    var ChangeOrder = db.Orders.Where(x => x.OrderID == OrderId).FirstOrDefault();
            //    MerchantTradeNo = ChangeOrder.GreenID;
            //}

            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;
            string output = string.Empty;
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    /* 服務參數 */
                    oPayment.ServiceMethod = HttpMethod.ServerPOST; //介接服務時，呼叫 API 的方法
                    oPayment.ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/QueryTradeInfo";//要呼叫介接服務的網址
                    oPayment.HashKey = "5294y06JbISpM5x9";//ECPay 提供的 HashKey
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay 提供的 HashIV
                    oPayment.MerchantID = "2000132";//ECPay 提供的廠商編號
                    /* 基本參數 */
                    oPayment.Query.MerchantTradeNo = MerchantTradeNo;//廠商的交易編號
                                                                     //oPayment.Query.PlatformID = "";//特約合作平台商代號
                    /**********************************************************************************/
                    /* 查詢訂單 */
                    enErrors.AddRange(oPayment.QueryTradeInfo(ref htFeedback));
                }
                // 取回所有資料
                if (enErrors.Count() == 0)
                {
                    /* 查詢後的回傳的基本參數 */

                    string szTradeStatus = String.Empty;

                    // 取得資料於畫面
                    foreach (string szKey in htFeedback.Keys)
                    {
                        switch (szKey)
                        {
                            /* 查詢後的回傳的基本參數 */

                            case "TradeStatus": szTradeStatus = htFeedback[szKey].ToString(); break;
                            default: break;
                        }

                    }
                    // 其他資料處理(將回傳值印在頁面)。
                    output = szTradeStatus;
                    if (szTradeStatus == "1")
                    {

                        ChangeOrderStatus(MerchantTradeNo, 0);
                        //using (RazerMasterDataContext db = new RazerMasterDataContext())
                        //{
                        //    var ChangeOrder = db.Orders.Where(x => x.GreenID == MerchantTradeNo).FirstOrDefault();
                        //    ChangeOrder.Status = 0;
                        //    db.SaveChanges();
                        //}
                        isExist = true;
                    }
                    else
                    {
                        //using (RazerMasterDataContext db = new RazerMasterDataContext())
                        //{
                        //    var ChangeOrder = db.Orders.Where(x => x.GreenID == MerchantTradeNo).FirstOrDefault();
                        //    var time = ChangeOrder.OrderTime.AddMinutes(30);
                        //    if (DateTime.Now > time)
                        //        db.Orders.Remove(ChangeOrder);
                        //    db.SaveChanges();
                        //}
                        isExist = false;
                    }


                }
            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
                isExist = false;
            }
            finally
            {
                // 顯示錯誤訊息。
                if (enErrors.Count() > 0)
                {
                    string szErrorMessage = String.Join("\\r\\n", enErrors);
                    logger.Info(szErrorMessage);
                }
            }

            return isExist;

        }
        public void CanCelOrder(Guid OrderId)
        {
            ChangeOrderStatus(OrderId,-1);
            //using (RazerMasterDataContext db = new RazerMasterDataContext())
            //{
            //    var ChangeOrder = db.Orders.Where(x => x.OrderID == OrderId).FirstOrDefault();
            //    ChangeOrder.Status = -1;
            //    db.SaveChanges();
            //}
        }
        public PaymentMethod GetMethod(int payment)
        {
            Dictionary<int, PaymentMethod> _payments = new Dictionary<int, PaymentMethod>()
        {
            {  1,PaymentMethod.Credit },
            {  2, PaymentMethod.WebATM },
            {  3,PaymentMethod.ATM},
            {  4,PaymentMethod.CVS },
            {  5,PaymentMethod.BARCODE }
        };
            return _payments[payment];
        }
        public List<Item> GetItems(Cart currentcart)
        {
            var items = new List<Item>();
            foreach (var item in currentcart)
            {
                items.Add(new Item()
                {
                    Name = item.Name,//商品名稱
                    Price = item.SalePrice,//商品單價
                    Currency = "US",//幣別單位
                    Quantity = item.Quantity,//購買數量
                    URL = item.ImgUrl,//商品的說明網址
                });


            }
            return items;
        }
        /// <summary>
        /// 變更訂單的狀態
        /// </summary>
        /// <param name="id">綠界編號</param>
        /// <param name="status">訂單狀態</param>
        public void ChangeOrderStatus(string id,int status)
        {
            var ChangeOrder = orderrepo.Get(x => x.GreenID == id);
            ChangeOrder.Status = status;
            orderrepo.Update(ChangeOrder);
        }
        /// <summary>
        /// 變更訂單的狀態
        /// </summary>
        /// <param name="id">訂單編號</param>
        /// <param name="status">訂單狀態</param>
        public void ChangeOrderStatus(Guid id, int status)
        {
            var ChangeOrder = orderrepo.Get(x => x.OrderID == id);
            ChangeOrder.Status = status;
            orderrepo.Update(ChangeOrder);
        }
    }
}