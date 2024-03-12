using Gestionale_Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Gestionale_Pizzeria.Controllers
{
    public class AuthController : Controller
    {
        DBContext db = new DBContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Utenti utente)
        {
            var loggedUser = db.Utenti.Where(u => u.Username == utente.Username && u.Password == utente.Password).FirstOrDefault();
            if (loggedUser == null)
            {
                TempData["ErrorLogin"] = true;
                return RedirectToAction("Login");
            }

            FormsAuthentication.SetAuthCookie(loggedUser.IdUtente.ToString(), true);
            return RedirectToAction("Index", "Home");

        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }

    }
}