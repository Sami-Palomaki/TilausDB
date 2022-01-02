using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDBWebApp.Models;
using System.Data.Entity;

namespace TilausDBWebApp.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        TilausDBEntities db = new TilausDBEntities();
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                var asiakkaat = db.Asiakkaat.Include(a => a.Postitoimipaikat);
                ViewBag.LoggedStatus = "In";
                ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi");
                return View(asiakkaat.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AsiakasID,Nimi,Osoite,Postinumero")] Asiakkaat asiakas)
        {
            if (ModelState.IsValid)
            {
                db.Asiakkaat.Add(asiakas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asiakas);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
            if (asiakkaat == null) return HttpNotFound();
            return View(asiakkaat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
            db.Asiakkaat.Remove(asiakkaat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)                   // Linkkipyyntö edittiin
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
            if (asiakkaat == null) return HttpNotFound();    // Jos ei löydy, palautetaan HttpNotFound
            return View(asiakkaat);                          // Jos löytyy palautetaan näkymä
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598
        public ActionResult Edit([Bind(Include = "AsiakasID, Nimi, Osoite, Postinumero")] Asiakkaat asiakas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asiakas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asiakas);
        }
    }
}