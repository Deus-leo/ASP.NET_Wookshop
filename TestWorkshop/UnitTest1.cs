using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASP.NET.Areas.Insert.Controllers;
namespace TestWorkshop
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
           InsertController aa = new InsertController();
           aa.AddData();
           
           aa.Index("1", "1", DateTime.Now, DateTime.Now, DateTime.Now, "1", "20",
           "taiwan","kuas","a","301","test","AA");

        }
    }
}
