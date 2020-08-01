using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        WebshopDBContext db = new WebshopDBContext();
        public static List<SelectListItem> listKategorije = new List<SelectListItem>();
        public ActionResult Index()
        {
            return View(db.Category.ToList());
        }     
    }
}