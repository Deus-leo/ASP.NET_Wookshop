using ASP.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET.Areas.Update.Controllers
{
    public class UpdateController : Controller
    {
        //
        // GET: /Update/Update/
        List<SelectListItem> cusdata = new List<SelectListItem>();
        List<SelectListItem> empdata = new List<SelectListItem>();
        List<SelectListItem> shippdata = new List<SelectListItem>();
        List<SelectListItem> productdata = new List<SelectListItem>();
        public ActionResult Index(String orderId)
        {
            KuasDB db = new KuasDB();

           
            AddData();

            PutData(orderId);
            return  View();
        }
        /// <summary>
        /// 初始化SelectListItem
        /// </summary>
        public void AddData()
        {
            
            KuasDB db = new KuasDB();

            productdata.Add(new SelectListItem()
            {
                Text = "---請選擇商品名稱---",
                Value = ""
            });


            cusdata.Add(new SelectListItem()
            {

                Text = "---請選擇顧客名稱---",
                Value = ""

            });

            empdata.Add(new SelectListItem()
            {

                Text = "---請選擇負責員工---",
                Value = ""

            });

            shippdata.Add(new SelectListItem()
            {

                Text = "---請選擇出貨公司---",
                Value = ""

            });

            productdata.AddRange(db.Products.Select(x => new SelectListItem()
            {
                Text = x.ProductName,
                Value = x.ProductID.ToString()
            }).ToList());

            cusdata.AddRange(db.Customers.Select(x => new SelectListItem()
            {

                Text = x.ContactName,
                Value = x.CustomerID.ToString()

            }).ToList());


            empdata.AddRange(db.Employees.Select(x => new SelectListItem()
            {

                Text = x.FirstName,
                Value = x.EmployeeID.ToString()


            }).ToList());


            shippdata.AddRange(db.Shippers.Select(x => new SelectListItem()
            {

                Text = x.CompanyName,
                Value = x.ShipperID.ToString()


            }).ToList());



          



        }

        /// <summary>
        /// 收到商品編號後，回傳該商品價格
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpPost]
        public JsonResult Product_Price(int id)
        {

            int Id = id;
            KuasDB db = new KuasDB();
            var price = db.Products.Where(x => x.ProductID.Equals(Id)).Select(x => x.UnitPrice).First();

            return this.Json(price, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(String Orderid, String cuslistdata, String emplistdata, DateTime OrderDate, DateTime NeedDate, DateTime? SentDate, String shipplistdata, String Shipment, String ShipCountry, String ShipCity, String ShipArea, String Zipcode, String ShipAddress, String ShipDes)
        {
            String[] productlistdata = Request.Form.GetValues("productlistdata");
            String[] price = Request.Form.GetValues("price");
            String[] count = Request.Form.GetValues("count");

            KuasDB db = new KuasDB();

            //更新訂單資料
            Orders orders = db.Orders.Find(Int32.Parse(Orderid));
            orders.CustomerID = Int32.Parse(cuslistdata);
            orders.EmployeeID = Int32.Parse(emplistdata);
            orders.OrderDate = OrderDate;
            orders.RequiredDate = NeedDate;
            orders.ShippedDate = SentDate;
            orders.ShipperID = Int32.Parse(shipplistdata);
            orders.Freight = decimal.Parse(Shipment);
            orders.ShipName = ShipDes;
            orders.ShipAddress = ShipAddress;
            orders.ShipCity = ShipCity;
            orders.ShipRegion = ShipArea;
            orders.ShipPostalCode = Zipcode;
            orders.ShipCountry = ShipCountry;

            List<OrderDetails> orderdetail = db.OrderDetails.Where(x => x.OrderID.ToString().Equals(Orderid)).ToList();
            //更新訂單明細資料
            int temp = 0;
            for (int i = 0; i < orderdetail.Count; i++)
            {
                ++temp;

                
                orderdetail[i].ProductID = Int32.Parse(productlistdata[i]);
                orderdetail[i].UnitPrice = decimal.Parse(price[i]);
                orderdetail[i].Qty = short.Parse(count[i]);
                orders.OrderDetails.Add(orderdetail[i]);


            }

            for (int i = temp; i < productlistdata.Length; i++)
            {
                OrderDetails orderdetails = new OrderDetails();
                orderdetails.OrderID = Int32.Parse(Orderid);
                orderdetails.ProductID = Int32.Parse(productlistdata[i]);
                orderdetails.UnitPrice = decimal.Parse(price[i]);
                orderdetails.Qty = short.Parse(count[i]);
                orders.OrderDetails.Add(orderdetails);
            }


                db.SaveChanges();

            //將資料重新放回view中
            AddData();
            PutData(Orderid);

            ViewBag.success = "success";
            return View();
        }
       

        public void PutData(String OrderId)
        {
            KuasDB db = new KuasDB();

            #region 填入表格內資料

            List<object> OrderData = db.Orders.Where(x => x.OrderID.ToString().Equals(OrderId)).ToList().Select(x => new
            {
                x.Freight,
                OrderDate = String.Format("{0:yyyy-MM-dd}", x.OrderDate),
                RequiredDate = String.Format("{0:yyyy-MM-dd}", x.RequiredDate),
                ShippedDate = String.Format("{0:yyyy-MM-dd}", x.ShippedDate),
                cusdata = new SelectList(cusdata, "Value", "Text", x.CustomerID),
                empdata = new SelectList(empdata, "Value", "Text", x.EmployeeID),
                shippdata = new SelectList(shippdata, "Value", "Text", x.ShipperID),
                x.ShipAddress,
                x.ShipCity,
                x.ShipCountry,
                x.ShipName,
                x.ShipPostalCode,
                x.ShipRegion,
                x.OrderID,
                totalMoney = x.OrderDetails.Sum(y => y.UnitPrice * y.Qty)

            }).Select(x => x.ToExpando()).ToList<object>();



            var orderdetails = db.OrderDetails.Where(x => x.OrderID.ToString().Equals(OrderId)).ToList()
                .Select(
                    x => new
                    {
                        x.UnitPrice,
                        x.Qty,
                        total = x.UnitPrice * x.Qty,
                        productdata = new SelectList(productdata, "Value", "Text", x.ProductID)
                    }.ToExpando()
                ).ToList();


            ViewBag.OrderData = OrderData;
            ViewBag.OrderDetailsData = orderdetails;






            #endregion
        
        }
    }
}
