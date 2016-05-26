using ASP.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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


            db.OrderDetails.RemoveRange(orderdetail);
          
            //一律將明細資料全部刪除後，再統一新增
            //更新訂單明細資料
          

            for (int i = 0; i < productlistdata.Length; i++)
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
                totalMoney = String.Format("{0:NT$#,0.####}", Convert.ToDecimal(x.OrderDetails.Sum(y => y.UnitPrice * y.Qty))),
               

            }).Select(x => x.ToExpando()).ToList<object>();



            var orderdetails = db.OrderDetails.Where(x => x.OrderID.ToString().Equals(OrderId)).ToList()
                .Select(
                    x => new
                    {
                        x.UnitPrice,
                        x.Qty,
                        total = x.UnitPrice * x.Qty,
                        productdata =  db.Products.ToList()
                    }.ToExpando()
                ).ToList();


            ViewBag.OrderData = OrderData;
            ViewBag.OrderDetailsData = orderdetails;

            //填入新增商品data
            List<Products> ProductData = db.Products.ToList();
            //填入訂單明細detail
            List<OrderDetails> orderDetail = db.OrderDetails.Where(x => x.OrderID.ToString() == OrderId).ToList();

            ViewBag.ProductData = ProductData;
            ViewBag.LoadOrderDetail = orderDetail;




            #endregion
        
        }

        /// <summary>
        /// 更新在查詢UI的訂單資料
        /// </summary>
        /// <param name="models"></param>
        [HttpPost]
        public ActionResult UpdateData(String models)
        {

            var json_serializer = new JavaScriptSerializer(); //透過反序列化將每一個json的欄位讀出來
            var routes_list = (IDictionary<string, object>)json_serializer.DeserializeObject(models);

            int Orderid = (int)routes_list["OrderID"];
        
            String OrderDate =(String) routes_list["OrderDate"];
            String ShippedDate = (String)routes_list["ShippedDate"];

            KuasDB db = new KuasDB();

            //更新訂單資料
            Orders orders = db.Orders.Find(Orderid);
            orders.ShippedDate = Convert.ToDateTime(ShippedDate);
            orders.OrderDate = Convert.ToDateTime(OrderDate);

            db.SaveChanges();

            return null;
        }
    }
}
