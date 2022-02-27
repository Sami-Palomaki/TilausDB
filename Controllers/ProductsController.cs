using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDBWebApp.Models;

namespace TilausDBWebApp.Controllers
{
    public class ProductsController : Controller
    {
        TilausDBEntities db = new TilausDBEntities();
        // GET: Products
        public ActionResult Index(string sortOrder, string currentFilter1, string searchString1, int? page, int? pagesize)
        {


            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Out";
                return RedirectToAction("login", "home");
            }
            else ViewBag.LoggedStatus = "In";
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.ProductNameSortParm = String.IsNullOrEmpty(sortOrder) ? "productname_desc" : "";
                ViewBag.UnitPriceSortParm = sortOrder == "Ahinta" ? "unitprice_desc" : "Ahinta";

                if (searchString1 != null)
                {
                    page = 1;
                }
                else
                {
                    searchString1 = currentFilter1;
                }
                
                //List<Tuotteet> model = db.Tuotteet.ToList();
                //db.Dispose();
                var tuotteet = from p in db.Tuotteet
                               select p;

                if (!String.IsNullOrEmpty(searchString1))
                {
                    tuotteet = tuotteet.Where(p => p.Nimi.Contains(searchString1));
                }

                switch (sortOrder)
                {
                    case "productname_desc":
                        tuotteet = tuotteet.OrderByDescending(p => p.Nimi);
                        break;
                    case "Ahinta":
                        tuotteet = tuotteet.OrderBy(p => p.Ahinta);
                        break;
                    case "unitprice_desc":
                        tuotteet = tuotteet.OrderByDescending(p => p.Ahinta);
                        break;
                    default:
                        tuotteet = tuotteet.OrderBy(p => p.Nimi);
                        break;
                }

                int pageSize = (pagesize ?? 10); //Tämä palauttaa sivukoon taikka jos pagesize on null, niin palauttaa koon 10 riviä per sivu
                int pageNumber = (page ?? 1); // Tämä palauttaa sivunumeron taikka jos page on null, niin palauttaa numeron yksi
                return View(tuotteet.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null) return HttpNotFound();
            return View(tuotteet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            db.Tuotteet.Remove(tuotteet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TuoteID,Nimi,Ahinta,Kuva")] Tuotteet tuote)
        {
            if (ModelState.IsValid)
            {
                db.Tuotteet.Add(tuote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tuote);
        }

        [HttpGet]
        public ActionResult Edit(int? id)                   // Linkkipyyntö edittiin
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null) return HttpNotFound();    // Jos ei löydy, palautetaan HttpNotFound
            return View(tuotteet);                          // Jos löytyy palautetaan näkymä
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598
        public ActionResult Edit([Bind(Include = "TuoteID, Nimi, Ahinta, Kuva")] Tuotteet tuote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tuote);
        }
    }
}