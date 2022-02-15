using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TilausDBWebApp.Models;

namespace TilausDBWebApp.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
        if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Out";
            }
            else ViewBag.LoggedStatus = "In";


            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Logins LoginModel)
        {
            TilausDBEntities db = new TilausDBEntities();
            var LoggedUser = db.Logins.SingleOrDefault(x => x.UserName == LoginModel.UserName && x.PassWord == LoginModel.PassWord); //Haetaan käyttäjän/Loginin tiedot annetuilla tunnustiedoilla tietokannasta LINQ -kyselyllä

            if (LoggedUser != null)
            {
                ViewBag.LoggedStatus = "In";
                ViewBag.LoginMessage = "Successfull login";
                Session["UserName"] = LoggedUser.UserName;
                return RedirectToAction("Index", "Home"); //Tässä määritellään mihin onnistunut kirjautuminen johtaa --> Home/Index

            }
            else
            {
                ViewBag.LoggedStatus = "Out";
                ViewBag.LoginMessage = "Login unsuccessfull";
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";

                return View("Index", LoginModel);
            }


        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LoggedOut()
        {
            //ViewBag.LoggedStatus = "Out";
            ViewBag.LoggedOut = "Olet kirjautunut ulos järjestelmästä.";
            return View(); //We have a special wiev LoggedOut which is a copy of Index, but has additional viewbag message of
                           //succesfull logout and possibility to login again.
        }
    }
}