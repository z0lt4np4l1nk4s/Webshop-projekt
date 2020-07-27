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

        public ActionResult Kategorija(int cat, string search, string sortBy, int? page)
        {
            var p = db.Product.Where(x => x.Kategorije.ID == cat).ToList().AsQueryable();
            ViewBag.Kategorije = db.Category;
            ViewBag.SortNaziv = sortBy == "Naziv" ? "Naziv desc" : "Naziv";
            ViewBag.SortCijena = sortBy == "Cijena" ? "Cijena desc" : "Cijena";
            p = p.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                p = p.Where(x => x.Naziv.ToLower().StartsWith(search.ToLower()) || x.Naziv.ToLower().Contains(search.ToLower()));
            }
            switch(sortBy)
            {
                case "Naziv":
                    p = p.OrderBy(x => x.Naziv);
                    break;
                case "Naziv desc":
                    p = p.OrderByDescending(x => x.Naziv);
                    break;
                case "Cijena":
                    p = p.OrderBy(x => x.Cijena);
                    break;
                case "Cijena desc":
                    p = p.OrderByDescending(x => x.Cijena);
                    break;
                default:
                    p = p.OrderBy(x => x.ID);
                    break;
            }
            return View(p.ToList().ToPagedList(page ?? 1, 5));
        }      
    }
}