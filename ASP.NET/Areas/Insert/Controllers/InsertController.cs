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
        List<SelectListItem> cusdata = new List<SelectListItem>();
        List<SelectListItem> empdata = new List<SelectListItem>();
        List<SelectListItem> shippdata = new List<SelectListItem>();
        List<SelectListItem> productdata = new List<SelectListItem>();

        public ActionResult Index()
        {

            AddData();

            ViewBag.cusdata = cusdata;
            ViewBag.empdata = empdata;
            ViewBag.shippdata = shippdata;
            ViewBag.productdata = productdata;


            return View();
        }

        /// <summary>
        /// 收到來自View輸入的值，並存入資料庫
        /// </summary>
       
        [HttpPost]
        public ActionResult Index(String cuslistdata, String emplistdata, DateTime? OrderDate, DateTime? NeedDate, DateTime? SentDate, String shipplistdata, String Shipment, String ShipCountry, String ShipCity, String ShipArea, String Zipcode, String ShipAddress, String ShipDes, String OrderMoney, String productlistdata, String price, String count)
        {


            AddData();
            ViewBag.cusdata = cusdata;
            ViewBag.empdata = empdata;
            ViewBag.shippdata = shippdata;
            ViewBag.productdata = productdata;
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

            return this.Json(price,JsonRequestBehavior.AllowGet);
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

                Text = "---請選擇負責員工---",
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
