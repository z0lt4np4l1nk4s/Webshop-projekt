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
    
    public class ProizvodController : Controller
    {
        WebshopDBContext db = new WebshopDBContext();

        public ActionResult Show(int id)
        {
            ViewBag.Kategorije = db.Category;
            Product proizvod = db.Product.Single(x => x.ID == id);
            ViewBag.KatID = proizvod.ID;
            return View(proizvod);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
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
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Product proizvod)
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
                db.Product.Add(proizvod);
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

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
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
            Product proizvod = db.Product.Single(x => x.ID == id);
            return View(proizvod);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Product proizvod)
        {
            if (ModelState.IsValid)
            {
                Product p = db.Product.Single(x => x.ID == proizvod.ID);
                if (proizvod.SlikaPath != null)
                {
                    if (System.IO.File.Exists(Request.MapPath(p.SlikaPath)))
                    {
                        System.IO.File.Delete(Request.MapPath(p.SlikaPath));
                    }
                    string fileName = Path.GetFileNameWithoutExtension(proizvod.SlikaFile.FileName) + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(proizvod.SlikaFile.FileName);
                    proizvod.SlikaPath = "~/Images/Proizvodi/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/Proizvodi/"), fileName);
                    proizvod.SlikaFile.SaveAs(fileName);
                }
                else
                {
                    proizvod.SlikaPath = p.SlikaPath;
                }
                db.Product.AddOrUpdate(proizvod);
                db.SaveChanges();
                return RedirectToAction("Proizvod", new { id = proizvod.ID });
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Kategorije = db.Category;
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product proizvod = db.Product.Find(id);
            if (proizvod == null)
            {
                return HttpNotFound();
            }
            return View(proizvod);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product proizvod = db.Product.Find(id);
            int kID = proizvod.Kategorije.ID;
            db.Product.Remove(proizvod);
            db.SaveChanges();
            return RedirectToAction("Kategorija", new { cat = kID });
        }
    }
}