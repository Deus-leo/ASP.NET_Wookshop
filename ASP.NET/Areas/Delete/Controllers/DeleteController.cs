using ASP.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ASP.NET.Areas.Delete.Controllers
{
    public class DeleteController : Controller
    {
        //
        // GET: /Delete/Delete/

        public ActionResult Index(String orderid)
        {

            KuasDB db = new KuasDB();

            Orders order = db.Orders.Find(Int32.Parse(orderid));
            List<OrderDetails> orderdeatil = db.OrderDetails.Where(x => x.OrderID.ToString().Equals(orderid)).ToList();

            db.Orders.Remove(order);
            db.OrderDetails.RemoveRange(orderdeatil);
            db.SaveChanges();
           
            ViewBag.success = "success";
                
            return View();         
                
        }

        /// <summary>
        /// 刪除在查詢UI的訂單資料&明細資料
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost]
        public void DeleteData(String models) //需要從前端傳整個models才能取的到值 且須宣告成String
        {
            var json_serializer = new JavaScriptSerializer();
            var routes_list = (IDictionary<string, object>)json_serializer.DeserializeObject(models);

            int Orderid = (int)routes_list["OrderID"];
            KuasDB db = new KuasDB();

            Orders order = db.Orders.Find(Orderid);
            List<OrderDetails> orderdeatil = db.OrderDetails.Where(x => x.OrderID.Equals(Orderid)).ToList();

            db.Orders.Remove(order);
            db.OrderDetails.RemoveRange(orderdeatil);
            db.SaveChanges();


           
        }

    }
}
