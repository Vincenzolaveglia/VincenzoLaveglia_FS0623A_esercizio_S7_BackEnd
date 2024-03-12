using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestionale_Pizzeria.Models;

namespace Gestionale_Pizzeria.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly DBContext db = new DBContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Prodotti.ToList());
        }

        public ActionResult Profilo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DettagliProdotto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotto = db.Prodotti.Find(id);
            if (prodotto == null)
            {
                return HttpNotFound();
            }
            return View(prodotto);
        }
    }
}
