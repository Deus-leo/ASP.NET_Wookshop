using ASP.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}
