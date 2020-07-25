using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        { 
            if (listKategorije.Count != 0)
            {
                listKategorije.Clear();
            }
            foreach (Kategorije k in db.Kategorija)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = k.ImeKategorije;
                selectListItem.Value = k.ID.ToString();
                listKategorije.Add(selectListItem);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Proizvodi proizvod)
        {
            if (ModelState.IsValid)
            {
                if (proizvod.SlikaFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(proizvod.SlikaFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(proizvod.SlikaFile.FileName);
                    proizvod.SlikaPath = "~/Images/Proizvodi/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/Proizvodi/"), fileName);
                    proizvod.SlikaFile.SaveAs(fileName);
                }
                db.Proizvod.Add(proizvod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        Response.Write(error);
                    }
                }
            }
            return View();
        }

        public ActionResult Kategorija(int cat, string search, string sortBy, int? page)
        {
            var p = db.Proizvod.Where(x => x.KategorijaID == cat).ToList().AsQueryable();
            ViewBag.Kategorije = db.Kategorija;
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

        public ActionResult Proizvod(int id)
        {
            ViewBag.Kategorije = db.Kategorija;
            Proizvodi proizvod = db.Proizvod.Single(x => x.ID == id);
            return View(proizvod);
        }
    }
}