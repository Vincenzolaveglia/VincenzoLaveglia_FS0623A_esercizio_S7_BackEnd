using Gestionale_Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Gestionale_Pizzeria.Controllers
{
    [Authorize]
    public class ProdottoController : Controller
    {
        readonly DBContext db = new DBContext();

        public ActionResult ListaProdotto()
        {
            return View(db.Prodotti.ToList());
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

        public ActionResult CreaProdotto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreaProdotto(Prodotti prodotti, HttpPostedFileBase Foto)
        {
            if (Foto != null && Foto.ContentLength > 0)
            {
                string nomeFile = Foto.FileName;
                string pathToSave = Path.Combine(Server.MapPath("~/Content/imgs"), nomeFile);
                Foto.SaveAs(pathToSave);

                prodotti.Foto = nomeFile;
            }

            if (ModelState.IsValid)
            {
                db.Prodotti.Add(prodotti);
                db.SaveChanges();
                return RedirectToAction("ListaProdotto");
            }

            return View(prodotti);
        }


        public ActionResult ModificaProdotto(int? id)
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
            TempData["fotoNome"] = prodotto.Foto;
            return View(prodotto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificaProdotto(Prodotti prodotti, HttpPostedFileBase FotoUrl)
        {
            if (FotoUrl != null)
            {
                string nomeFile = FotoUrl.FileName;
                string pathToSave = Path.Combine(Server.MapPath("~/Content/imgs"), nomeFile);
                FotoUrl.SaveAs(pathToSave);

                prodotti.Foto = nomeFile;
            }
            else
            {
                prodotti.Foto = TempData["fotoNome"].ToString();
            }

            if (ModelState.IsValid)
            {
                db.Entry(prodotti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListaProdotto");
            }
            return View(prodotti);
        }

        public ActionResult EliminaProdotto(int id)
        {
            Prodotti prodotto = db.Prodotti.Find(id);
            db.Prodotti.Remove(prodotto);
            db.SaveChanges();
            return RedirectToAction("ListaProdotto");
        }
    }
}