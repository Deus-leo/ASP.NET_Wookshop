using ASP.NET.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ASP.NET.Areas.Search.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/Search/

        List<SelectListItem> empdata = new List<SelectListItem>();
        List<SelectListItem> shippdata = new List<SelectListItem>();

        public ActionResult Index()
        {

            AddData();



            ViewBag.empdata = empdata;
            ViewBag.shipperdata = shippdata;
            return View();
        }

        [ActionName("Search")]
        public ActionResult Index(String OrderID, String User, DateTime? OrderDate, DateTime? SentDate, DateTime? NeedDate, String employeeName, String shipperdata, int page = 0)
        {

            String OrderDate_String = OrderDate.ToString();
            String SentDate_String = SentDate.ToString();
            String NeedDate_String = NeedDate.ToString();
            KuasDB db = new KuasDB();

            // 條件搜尋,沒有輸入的則不進行搜尋
            double count = db.Orders.Where(x => (
                (!string.IsNullOrEmpty(User)) ? x.Customers.CompanyName.Contains(User) : true) &&
                ((!string.IsNullOrEmpty(OrderID)) ? x.OrderID.ToString().Equals(OrderID) : true) &&
                ((!string.IsNullOrEmpty(employeeName)) ? x.EmployeeID.ToString().Equals(employeeName) : true) &&
                ((!string.IsNullOrEmpty(shipperdata)) ? x.ShipperID.ToString().Equals(shipperdata) : true) &&
                ((!string.IsNullOrEmpty(OrderDate_String)) ? (x.OrderDate.Year.Equals(OrderDate.Value.Year) && x.OrderDate.Month.Equals(OrderDate.Value.Month) && x.OrderDate.Day.Equals(OrderDate.Value.Day)) : true) &&
                ((!string.IsNullOrEmpty(SentDate_String)) ? (x.ShippedDate.Value.Year.Equals(SentDate.Value.Year) && x.ShippedDate.Value.Month.Equals(SentDate.Value.Month) && x.ShippedDate.Value.Day.Equals(SentDate.Value.Day)) : true) &&
                ((!string.IsNullOrEmpty(NeedDate_String)) ? (x.RequiredDate.Year.Equals(NeedDate.Value.Year) && x.RequiredDate.Month.Equals(NeedDate.Value.Month) && x.RequiredDate.Day.Equals(NeedDate.Value.Day)) : true)




                ).Count();

            int maxPage = (int)Math.Ceiling(count / 10);



            var Query_data = db.Orders.Where(x => (
                (!string.IsNullOrEmpty(User)) ? x.Customers.CompanyName.Contains(User) : true) &&
                ((!string.IsNullOrEmpty(OrderID)) ? x.OrderID.ToString().Equals(OrderID) : true) &&
                ((!string.IsNullOrEmpty(employeeName)) ? x.EmployeeID.ToString().Equals(employeeName) : true) &&
                ((!string.IsNullOrEmpty(shipperdata)) ? x.ShipperID.ToString().Equals(shipperdata) : true) &&
                ((!string.IsNullOrEmpty(OrderDate_String)) ? (x.OrderDate.Year.Equals(OrderDate.Value.Year) && x.OrderDate.Month.Equals(OrderDate.Value.Month) && x.OrderDate.Day.Equals(OrderDate.Value.Day)) : true) &&
                ((!string.IsNullOrEmpty(SentDate_String)) ? (x.ShippedDate.Value.Year.Equals(SentDate.Value.Year) && x.ShippedDate.Value.Month.Equals(SentDate.Value.Month) && x.ShippedDate.Value.Day.Equals(SentDate.Value.Day)) : true) &&
                ((!string.IsNullOrEmpty(NeedDate_String)) ? (x.RequiredDate.Year.Equals(NeedDate.Value.Year) && x.RequiredDate.Month.Equals(NeedDate.Value.Month) && x.RequiredDate.Day.Equals(NeedDate.Value.Day)) : true)




                )


                .Select(x => new
                {
                    x.OrderID,
                    CompanyName = x.Customers.CompanyName,
                    x.OrderDate,
                    x.ShippedDate

                })
            .OrderBy(x => x.OrderID)
            .Skip(10 * page)
            .Take(10)
            .ToList().Select(x => x.ToExpando());




            AddData();

            ViewBag.empdata = empdata;
            ViewBag.shipperdata = shippdata;
            ViewBag.Query_data = Query_data;
            ViewBag.maxpage = maxPage;
            return View("Index");
        }


        public void AddData()
        {
            KuasDB db = new KuasDB();
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
