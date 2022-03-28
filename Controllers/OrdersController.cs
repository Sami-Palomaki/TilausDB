using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDBWebApp.Models;

namespace TilausDBWebApp.Controllers
{
    public class OrdersController : Controller
    {
        TilausDBEntities3 db = new TilausDBEntities3();

        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                //ViewBag.LoggedStatus = "Out";
                return RedirectToAction("login", "home");
            }
            else /*ViewBag.LoggedStatus = "In";*/
            {
                List<Tilaukset> model = db.Tilaukset.ToList();
                db.Dispose();
                return View(model);
                //var tilaukset = db.Tilaukset.Include(t => t.Tilausrivit);
                //ViewBag.LoggedStatus = "In";
                //ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "AsiakasID");
                //return View(tilaukset.ToList());
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null) return HttpNotFound();
            return View(tilaukset);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            db.Tilaukset.Remove(tilaukset);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            //var asiakasNumerotCreate = db.Asiakkaat.Select(a => a.AsiakasID).ToList();
            //var asiakasNumerotSelectList = new SelectList(asiakasNumerotCreate);
            //ViewData["AsiakasNumerotCreate"] = asiakasNumerotSelectList;
            //return View();
            var posti = db.Postitoimipaikat;
            IEnumerable<SelectListItem> selectPostiList = from p in posti
                                                          select new SelectListItem
                                                          {
                                                              Value = p.Postinumero.ToString(),
                                                              Text = p.Postinumero + " " + p.Postitoimipaikka.ToString()
                                                          };

            ViewBag.PostiID = new SelectList(selectPostiList, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaus)
        {
            if (ModelState.IsValid)
            {
                db.Tilaukset.Add(tilaus);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    Console.WriteLine("VIRHE!");
                }
                return RedirectToAction("Index");
            }
            return View(tilaus);
        }

        [HttpGet]
        public ActionResult Edit(int? id)                   // Linkkipyyntö edittiin
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tilaukset tilaukset = db.Tilaukset.Find(id);

            var asiakasNumerot = db.Asiakkaat.Select(a => a.AsiakasID).ToList();
            var asiakasNumerotSelectList = new SelectList(asiakasNumerot);
            ViewData["AsiakasNumerot"] = asiakasNumerotSelectList;

            if (tilaukset == null) return HttpNotFound();    // Jos ei löydy, palautetaan HttpNotFound
            return View(tilaukset);                          // Jos löytyy palautetaan näkymä
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598
        public ActionResult Edit([Bind(Include = "TilausID, AsiakasID, Toimitusosoite, Postinumero, Tilauspvm, Toimituspvm")] Tilaukset tilaus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilaus).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    Console.WriteLine("VIRHE!");
                }
                return RedirectToAction("Index");
            }
            return View(tilaus);
        }

        //public ActionResult TilausOtsikot()
        //{
        //    var orders = db.Tilaukset.Include(o => o.)
        //}
    }
}