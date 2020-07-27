using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;

namespace Webshop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KategorijaController : Controller
    {
        WebshopDBContext db = new WebshopDBContext();
        // GET: Kategorija
        public ActionResult Index()
        {
            ViewBag.Proizvodi = db.Proizvod;
            return View(db.Kategorija.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Kategorije kategorija)
        {
            if (ModelState.IsValid)
            {
                db.Kategorija.Add(kategorija);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategorija);
        }

        public ActionResult Edit(int id)
        {
            Kategorije k = db.Kategorija.Single(x => x.ID == id);
            return View(k);
        }

        [HttpPost]
        public ActionResult Edit(Kategorije kategorija)
        {
            if (ModelState.IsValid)
            {
                db.Kategorija.AddOrUpdate(kategorija);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategorija);
        }

        public ActionResult Proizvodi()
        {
            if (HomeController.listKategorije.Count != 0)
            {
                HomeController.listKategorije.Clear();
            }
            foreach (Kategorije k in db.Kategorija)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = k.ImeKategorije;
                selectListItem.Value = k.ID.ToString();
                HomeController.listKategorije.Add(selectListItem);
            }
            return View(db.Proizvod.ToList());
        }

        public ActionResult EditProizvod(int id)
        {
            Proizvodi p = db.Proizvod.Single(x => x.ID == id);
            return View(p);
        }

        [HttpPost]
        public ActionResult EditProizvod([Bind(Include = "ID,Kategorije")] Proizvodi proizvod)
        {
            Proizvodi p = db.Proizvod.Single(x => x.ID == proizvod.ID);
            p.Kategorije = db.Kategorija.Single(x => x.ID == proizvod.Kategorije.ID);
            db.Proizvod.AddOrUpdate(p);
            db.SaveChanges();
            return RedirectToAction("Proizvodi");
        }
    }
}