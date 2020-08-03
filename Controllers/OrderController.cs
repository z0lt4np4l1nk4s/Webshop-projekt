using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Buy()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Buy(Order order)
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Product> list = Session["Cart"] as List<Product>;
            using (WebshopDBContext db = new WebshopDBContext())
            {
                foreach(Product p in list)
                {
                    order.Product = db.Product.Single(x => x.ID == p.ID);
                    db.Order.Add(order);
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}