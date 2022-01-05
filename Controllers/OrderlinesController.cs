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
    public class OrderlinesController : Controller
    {
        // GET: Orderlines
        TilausDBEntities db = new TilausDBEntities();
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Out";
                return RedirectToAction("login", "home");
            }
            else
            {
                ViewBag.LoggedStatus = "In";
                //List<Tilausrivit> model = db.Tilausrivit.ToList();
                //db.Dispose();
                //return View(model);
                var tilausrivit = db.Tilausrivit.Include(t => t.Tuotteet);          // TÄTÄ MUUTIN
                return View(tilausrivit.ToList());
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tilausrivit tilausrivit = db.Tilausrivit.Find(id);
            if (tilausrivit == null) return HttpNotFound();
            return View(tilausrivit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tilausrivit tilausrivit = db.Tilausrivit.Find(id);
            db.Tilausrivit.Remove(tilausrivit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausriviID,TilausID,TuoteID,Maara,Ahinta")] Tilausrivit tilausrivi)
        {
            if (ModelState.IsValid)
            {
                db.Tilausrivit.Add(tilausrivi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tilausrivi);
        }

        [HttpGet]
        public ActionResult Edit(int? id)                   // Linkkipyyntö edittiin
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tilausrivit tilausrivit = db.Tilausrivit.Find(id);

            var tuoteIDt = db.Tuotteet.Select(t => t.TuoteID).ToList();
            var tuoteIDSelectList = new SelectList(tuoteIDt);
            ViewData["TuoteIDt"] = tuoteIDSelectList;

            if (tilausrivit == null) return HttpNotFound();    // Jos ei löydy, palautetaan HttpNotFound
            return View(tilausrivit);                          // Jos löytyy palautetaan näkymä
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598
        public ActionResult Edit([Bind(Include = "TilausriviID, TilausID, TuoteID, Maara, Ahinta")] Tilausrivit tilausrivi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilausrivi).State = EntityState.Modified;
                db.SaveChanges();    
                return RedirectToAction("Index");
            }
            return View(tilausrivi);
        }
    }
}