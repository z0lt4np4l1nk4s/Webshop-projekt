using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class ProizvodController : Controller
    {
        WebshopDBContext db = new WebshopDBContext();
        List<Product> CartList = new List<Product>();
        List<KeyValuePair<int, int>> kolicina = new List<KeyValuePair<int, int>>();

        public ActionResult Show(int id)
        {
            ViewBag.Kategorije = db.Category;
            Product proizvod = db.Product.Single(x => x.ID == id);
            ViewBag.KatID = proizvod.CategoryID;
            return View(proizvod);
        }

        [HttpPost, ActionName("Show")]
        public ActionResult AddToCart(int id)
        {
            Product proizvod = db.Product.Single(x => x.ID == id);
            if (Session["Cart"] != null)
            {
                CartList.AddRange(Session["Cart"] as List<Product>);
                kolicina.AddRange(Session["Kolicina"] as List<KeyValuePair<int, int>>);
            }
            if (!CartList.Contains(proizvod))
            {
                CartList.Add(proizvod);
                kolicina.Add(new KeyValuePair<int, int>(proizvod.ID, 1));
            }
            Session["Cart"] = CartList;
            Session["Kolicina"] = kolicina;
            return RedirectToAction("Show", new { id = proizvod.ID});
        }

        private int FindID(Product product)
        {
            for (int i = 0; i < CartList.Count; i++)
            {
                if (product.ID == CartList[i].ID)
                {
                    return i;
                }
            }
            return -1;
        }

        public ActionResult Empty()
        {
            Session["Cart"] = null;
            Session["Kolicina"] = null;
            return RedirectToAction("Cart");
        }

        public ActionResult Remove(int id)
        {
            Product p = db.Product.Single(x => x.ID == id);
            CartList.AddRange(Session["Cart"] as List<Product>);
            kolicina.AddRange(Session["Kolicina"] as List<KeyValuePair<int, int>>);
            int y = FindID(p);
            if (y != -1)
            {
                CartList.RemoveAt(y);
                kolicina.RemoveAt(y);
                if (CartList.Count != 0)
                {
                    Session["Cart"] = CartList;
                    Session["Kolicina"] = kolicina;
                }
                else
                {
                    Session["Cart"] = null;
                    Session["Kolicina"] = null;
                }
            }
            return RedirectToAction("Cart");
        }

        public ActionResult Cart()
        {
            return View();
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
                return RedirectToAction("Proizvodi", "Kategorija", new { cat = proizvod.CategoryID});
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
                if (proizvod.SlikaFile != null)
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
                return RedirectToAction("Proizvodi", "Kategorija", new { cat = proizvod.CategoryID });
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
            int kID = proizvod.Category.ID;
            db.Product.Remove(proizvod);
            db.SaveChanges();
            return RedirectToAction("Proizvodi", "Kategorija", new { cat = kID });
        }

        public ActionResult IncrementDecrement(int id, string submit)
        {
            kolicina = Session["Kolicina"] as List<KeyValuePair<int, int>>;
            CartList = Session["Cart"] as List<Product>;
            if (submit == "+")
            {
                for (int i = 0; i < kolicina.Count; i++)
                {
                    if (kolicina[i].Key == id)
                    {
                        KeyValuePair<int, int> v = new KeyValuePair<int, int>(kolicina[i].Key, kolicina[i].Value + 1);
                        kolicina[i] = v;
                        continue;
                    }
                }
            }
            else if (submit == "-")
            {
                for (int i = 0; i < kolicina.Count; i++)
                {
                    if (kolicina[i].Key == id)
                    {
                        if (kolicina[i].Value - 1 == 0)
                        {
                            kolicina.RemoveAt(i);
                            CartList.RemoveAt(i);
                            if (CartList.Count == 0)
                            {
                                Session["Cart"] = null;
                            }
                            else
                            {
                                Session["Cart"] = CartList;
                            }
                        }
                        else
                        {
                            KeyValuePair<int, int> v = new KeyValuePair<int, int>(kolicina[i].Key, kolicina[i].Value - 1);
                            kolicina[i] = v;
                        }
                        continue;
                    }
                }
            }
            Session["Kolicina"] = kolicina;
            return RedirectToAction("Cart");
        }
    }
}