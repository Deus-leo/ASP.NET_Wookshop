using ASP.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET.Areas.Insert.Controllers
{
    public class InsertController : Controller
    {
        //
        // GET: /Insert/Insert/
       public List<SelectListItem> cusdata = new List<SelectListItem>();
       public List<SelectListItem> empdata = new List<SelectListItem>();
       public List<SelectListItem> shippdata = new List<SelectListItem>();
       public List<SelectListItem> productdata = new List<SelectListItem>();

        public ActionResult Index()
        {

            AddData();

            ViewBag.cusdata = cusdata;
            ViewBag.empdata = empdata;
            ViewBag.shippdata = shippdata;

            //填入新增商品data
            KuasDB db = new KuasDB();
            List<Products> ProductData = db.Products.ToList();
            ViewBag.ProductData = ProductData;

            return View();
        }

        /// <summary>
        /// 收到來自View輸入的值，並存入資料庫
        /// </summary>
       
        [HttpPost]
        public ActionResult Index(String cuslistdata, String emplistdata, DateTime OrderDate, DateTime NeedDate, DateTime? SentDate, String shipplistdata, String Shipment, String ShipCountry, String ShipCity, String ShipArea, String Zipcode, String ShipAddress, String ShipDes)
        {


            Shipment = String.IsNullOrEmpty(Shipment) ? "0" : Shipment;
            ShipCountry = String.IsNullOrEmpty(ShipCountry) ? "" : ShipCountry;
            ShipCity = String.IsNullOrEmpty(ShipCity) ? "" : ShipCity;
            ShipArea = String.IsNullOrEmpty(ShipArea) ? "" : ShipArea;
            Zipcode = String.IsNullOrEmpty(Zipcode) ? "" : Zipcode;
            ShipAddress = String.IsNullOrEmpty(ShipAddress) ? "" : ShipAddress;
            ShipDes = String.IsNullOrEmpty(ShipDes) ? "" : ShipDes;

            
            
            String[] productlistdata = Request.Form.GetValues("productlistdata"); 
            String[] price = Request.Form.GetValues("price"); 
            String[] count = Request.Form.GetValues("count");


            KuasDB kuas = new KuasDB();
            Orders orders = new Orders();
           
            orders.CustomerID = Int32.Parse(cuslistdata);
            orders.EmployeeID = Int32.Parse(emplistdata);
            orders.OrderDate = OrderDate;
            orders.RequiredDate = NeedDate;
            orders.ShippedDate = SentDate;
            orders.ShipperID = Int32.Parse(shipplistdata);
            orders.Freight = Int32.Parse(Shipment);
            orders.ShipName = ShipDes;
            orders.ShipAddress = ShipAddress;
            orders.ShipCity = ShipCity;
            orders.ShipRegion = ShipArea;
            orders.ShipPostalCode = Zipcode;
            orders.ShipCountry = ShipCountry;

            
            kuas.Orders.Add(orders);

            for (int i = 0; i < productlistdata.Length; i++)
            {
                OrderDetails orderdetail = new OrderDetails();
                orderdetail.ProductID = Int32.Parse(productlistdata[i]);
                orderdetail.UnitPrice = decimal.Parse(price[i]);
                orderdetail.Qty = short.Parse(count[i]);
                orders.OrderDetails.Add(orderdetail);
                    


               


            }
            try
            {
                kuas.SaveChanges();


            }

            catch (Exception ex)
            {
                throw;
            }

            AddData();
            ViewBag.cusdata = cusdata;
            ViewBag.empdata = empdata;
            ViewBag.shippdata = shippdata;
            //填入新增商品data
            List<Products> ProductData = kuas.Products.ToList();
            ViewBag.ProductData = ProductData;
            ViewBag.success = "success";
            return View();
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
            var price = db.Products.Where(x=>x.ProductID.Equals(Id)).Select(x=>x.UnitPrice).First();

            return this.Json(price);
        }
 

        public void AddData()
        {


            KuasDB db = new KuasDB();
            productdata.Add(new SelectListItem()
            {

                Text = "---請選擇商品---",
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
                Value = "0"

            });

            foreach (var product in db.Products.Select(x => x).ToList())
            {
                productdata.Add(new SelectListItem()
                {

                    Text = product.ProductName.ToString(),
                    Value = product.ProductID.ToString()

                });


            }


            foreach (var customer in db.Customers.Select(x => x).ToList())
            {
                cusdata.Add(new SelectListItem()
                {

                    Text = customer.CompanyName.ToString(),
                    Value = customer.CustomerID.ToString()

                });


            }

            foreach (var employee in db.Employees.Select(x => x).ToList())
            {
                empdata.Add(new SelectListItem()
                {

                    Text = employee.FirstName.ToString(),
                    Value = employee.EmployeeID.ToString()

                });


            }





            foreach (var shipper in db.Shippers.Select(x => x).ToList())
            {
                shippdata.Add(new SelectListItem()
                {

                    Text = shipper.CompanyName.ToString(),
                    Value = shipper.ShipperID.ToString()


                });
            }



        }

    }
}
