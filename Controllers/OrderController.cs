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
        WebshopDBContext db = new WebshopDBContext();
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
            foreach (Product p in Session["Cart"] as List<Product>)
            {
                order.Product = db.Product.Single(x => x.ID == p.ID);
                db.Order.Add(order);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}