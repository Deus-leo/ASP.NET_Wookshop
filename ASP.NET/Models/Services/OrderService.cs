using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET.Models.Services
{
    public class OrderService
    {

        private String Dbconnecion()
        {
            return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["KuasDB"].ConnectionString; 
        }
        public List<Orders> GetOrders(String OrderID, String User, DateTime? OrderDate, DateTime? SentDate, DateTime? NeedDate, String employeeName, String shipperdata)
       {
            
            return null;
        }
    }
}