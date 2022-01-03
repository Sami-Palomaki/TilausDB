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
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                List<Tuotteet> model = db.Tuotteet.ToList();
                db.Dispose();
                return View(model);
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