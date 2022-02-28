using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TilausDBWebApp.Models;

namespace TilausDBWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //if (Session["UserName"] == null)
            //{
            //    ViewBag.LoggedStatus = "Out";
            //    return RedirectToAction("login", "home");
            //}
            //else ViewBag.LoggedStatus = "In";
            return View();
        }

        public ActionResult About()
        {
            //if (Session["UserName"] == null)
            //{
            //    ViewBag.LoggedStatus = "Out";
            //    return RedirectToAction("login", "home");
            //}
            //else ViewBag.LoggedStatus = "In";
            //{
                ViewBag.Message = "Tietoja.";

                return View();
            }
        }

        public ActionResult Contact()
        {
            //if (Session["UserName"] == null)
            //{
            //    ViewBag.LoggedStatus = "Out";
            //    return RedirectToAction("login", "home");
            //}
            //else ViewBag.LoggedStatus = "In";
            //{
                ViewBag.Message = "Yhteystietoja.";
                //ViewBag.UserName = Session["UserName"];
                return View();
            //}
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Logins LoginModel)
        {
            TilausDBEntities db = new TilausDBEntities();
            //Haetaan käyttäjän/Loginin tiedot annetuilla tunnustiedoilla tietokannasta LINQ -kyselyllä
            var LoggedUser = db.Logins.SingleOrDefault(x => x.UserName == LoginModel.UserName && x.PassWord == LoginModel.PassWord);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Successfull login";
                ViewBag.LoggedStatus = "In";
                ViewBag.LoginError = 0; //Ei virhettä...
                Session["UserName"] = LoggedUser.UserName;  // Perustetaan user-name sessio
                Session["LoginID"] = LoggedUser.LoginId;
                return RedirectToAction("Index", "Home"); //Tässä määritellään mihin onnistunut kirjautuminen johtaa --> Home/Index
            }
            else
            {
                ViewBag.LoginMessage = "Login unsuccessfull";
                ViewBag.LoggedStatus = "Out";
                ViewBag.LoginError = 1;     //Pakotetaan modaali login-ruutu uudelleen, koska kirjautumisyritys on epäonnistunut
                //LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Index", LoginModel);
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("Index", "Home"); //Uloskirjautumisen jälkeen pääsivulle
        }
    }
}