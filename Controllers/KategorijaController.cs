using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;
using PagedList;

namespace Webshop.Controllers
{
    public class KategorijaController : Controller
    {
        WebshopDBContext db = new WebshopDBContext();
        // GET: Kategorija
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.Proizvodi = db.Product;
            return View(db.Category.ToList());
        }

        public ActionResult Proizvodi(int cat, string search, string sortBy, int? page)
        {
            var p = db.Product.Where(x => x.Category.ID == cat).ToList().AsQueryable();
            ViewBag.Kategorije = db.Category;
            ViewBag.SortNaziv = sortBy == "Naziv" ? "Naziv desc" : "Naziv";
            ViewBag.SortCijena = sortBy == "Cijena" ? "Cijena desc" : "Cijena";
            p = p.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                p = p.Where(x => x.Naziv.ToLower().StartsWith(search.ToLower()) || x.Naziv.ToLower().Contains(search.ToLower()));
            }
            switch (sortBy)
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

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Category kategorija)
        {
            if (ModelState.IsValid)
            {
                if (kategorija.SlikaFile != null)
                {
                    string fileName = kategorija.ImeKategorije + Path.GetExtension(kategorija.SlikaFile.FileName);
                    kategorija.SlikaPath = "~/Images/Kategorije/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/Kategorije/"), fileName);
                    kategorija.SlikaFile.SaveAs(fileName);
                }
                db.Category.Add(kategorija);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategorija);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Category k = db.Category.Single(x => x.ID == id);
            return View(k);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Category kategorija)
        {
            if (ModelState.IsValid)
            {
                Category c = db.Category.Single(x => x.ID == kategorija.ID); ;
                if (kategorija.SlikaFile != null)
                {
                    
                    if (System.IO.File.Exists(Request.MapPath(c.SlikaPath)))
                    {
                        System.IO.File.Delete(Request.MapPath(c.SlikaPath));
                    }
                    string fileName = kategorija.ImeKategorije + Path.GetExtension(kategorija.SlikaFile.FileName);
                    kategorija.SlikaPath = "~/Images/Kategorije/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/Kategorije/"), fileName);
                    kategorija.SlikaFile.SaveAs(fileName);
                }
                else
                {
                    kategorija.SlikaPath = c.SlikaPath;
                }
                db.Category.AddOrUpdate(kategorija);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategorija);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Proizvod()
        {
            if (HomeController.listKategorije.Count != 0)
            {
                HomeController.listKategorije.Clear();
            }
            foreach (Category k in db.Category)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = k.ImeKategorije;
                selectListItem.Value = k.ID.ToString();
                HomeController.listKategorije.Add(selectListItem);
            }
            return View(db.Product.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditProizvod(int id)
        {
            Product p = db.Product.Single(x => x.ID == id);
            return View(p);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditProizvod([Bind(Include = "ID,CategoryID")] Product proizvod)
        {
            Product p = db.Product.Single(x => x.ID == proizvod.ID);
            p.Category = db.Category.Single(x => x.ID == proizvod.CategoryID);
            db.Product.AddOrUpdate(p);
            db.SaveChanges();
            return RedirectToAction("Proizvod");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category kategorija = db.Category.Find(id);
            if (kategorija == null)
            {
                return HttpNotFound();
            }
            return View(kategorija);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category kategorija = db.Category.Find(id);
            if (System.IO.File.Exists(Request.MapPath(kategorija.SlikaPath)))
            {
                System.IO.File.Delete(Request.MapPath(kategorija.SlikaPath));
            }
            db.Category.Remove(kategorija);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}