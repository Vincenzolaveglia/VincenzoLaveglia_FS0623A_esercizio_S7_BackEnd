using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Gestionale_Pizzeria.Models;

namespace Gestionale_Pizzeria.Controllers
{
    [Authorize]
    public class OrdineController : Controller
    {
        readonly DBContext db = new DBContext();

        public ActionResult ListaOrdine()
        {
            int? userId = (int?)Session["UserId"];

            if (userId == 0)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }

            if (userId.HasValue)
            {
                if (User.IsInRole("Admin"))
                {
                    var ordineAdmin = db.Ordini.Include(o => o.Utenti);
                    return View(ordineAdmin.ToList());
                }
                else
                {
                    var ordineUtente = db.Ordini.Include(o => o.Utenti).Where(o => o.UtenteId == userId.Value).ToList();
                    return View(ordineUtente);
                }
            }
            return View(new List<Ordini>());
        }

        public ActionResult ModificaOrdine(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordine = db.Ordini.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtente", "Nome", ordine.UtenteId);
            return View(ordine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificaOrdine(Ordini ordine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListaOrdine");
            }
            ViewBag.IdUtente = new SelectList(db.Utenti, "IdUtente", "Nome", ordine.UtenteId);
            return View(ordine);
        }

        public ActionResult EliminaOrdine(int id)
        {
            Ordini ordine = db.Ordini.Find(id);
            db.Ordini.Remove(ordine);
            db.SaveChanges();
            return RedirectToAction("ListaOrdine");
        }


        public ActionResult ControlloOrdine()
        {
            return View();
        }

        [HttpPost]
        public JsonResult OrdiniEvasi()
        {
            {
                DateTime today = DateTime.Today;
                DateTime tomorrow = today.AddDays(1);

                List<Ordini> ordine = db.Ordini.Where(a => a.IsCompleto == "evaso" && a.DataOrdine >= today && a.DataOrdine < tomorrow).ToList();
                List<OrdineEvaso> ordineEvaso = new List<OrdineEvaso>();

                foreach (var o in ordine)
                {
                    ordineEvaso.Add(new OrdineEvaso
                    {
                        Id = o.UtenteId,
                        Nome = o.Utenti.Username,
                        TotaleOrdiniOggi = ordine.Count()
                    });
                }

                return Json(ordineEvaso);
            }
        }

    }
}