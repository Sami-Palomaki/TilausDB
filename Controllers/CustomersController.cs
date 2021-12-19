using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDBWebApp.Models;

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
                //TilausDBEntities db = new TilausDBEntities();
                List<Asiakkaat> model = db.Asiakkaat.ToList();
                db.Dispose();
                return View(model);
            }
        }

        public ActionResult Create()
        {
            //ViewBag.RegionID = new SelectList(db.Region, "RegionID", "RegionDescription");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AsiakasID,Nimi,Osoite")] Asiakkaat asiakas)
        {
            if (ModelState.IsValid)
            {
                db.Asiakkaat.Add(asiakas);
                db.SaveChanges();
                //ViewBag.RegionID = new SelectList(db.Region, "RegionID", "RegionDescription");
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
    }
}