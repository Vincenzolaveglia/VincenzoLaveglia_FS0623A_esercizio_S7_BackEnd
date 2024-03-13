using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Gestionale_Pizzeria.Models;
using static Gestionale_Pizzeria.Models.Carrello;

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

        public ActionResult VisualizzaCarrello()
        {
            if (Session["Carrello"] is List<CarrelloItem> carrelloItems && carrelloItems.Any())
            {
                return View(carrelloItems);
            }
            else
            {
                return View("Home");
            }
        }

        [HttpPost]
        public ActionResult AggiungiAlCarrello(int id, string nome, decimal prezzo, int quantita)
        {
            if (!(Session["Carrello"] is List<CarrelloItem> carrello))
            {
                carrello = new List<CarrelloItem>();
            }

            var carrelloItem = carrello.FirstOrDefault(item => item.Prodotto.IdProdotti == id);

            if (carrelloItem != null)
            {
                carrelloItem.Quantita += quantita;
            }
            else
            {
                carrello.Add(new CarrelloItem
                {
                    Prodotto = new Prodotti { IdProdotti = id, Nome = nome, Prezzo = prezzo },
                    Quantita = quantita
                });
            }

            Session["Carrello"] = carrello;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ConcludiOrdine(string indirizzoConsegna, string noteSpeciali)
        {

            int userId = (int)Session["UserId"];

            if (userId == 0)
            {
                FormsAuthentication.SignOut();
            }

            if (Session["UserId"] != null)
            {
                if (Session["Carrello"] is List<CarrelloItem> carrelloItems && carrelloItems.Any())
                {
                    decimal totale = carrelloItems.Sum(item => item.Prodotto.Prezzo * item.Quantita);

                    var nuovoOrdine = new Ordini
                    {
                        UtenteId = (int)Session["UserId"],
                        DataOrdine = DateTime.Now,
                        IsCompleto = "false",
                        IndirizzoConsegna = indirizzoConsegna,
                        Nota= noteSpeciali,
                        OrdiniProdotti = carrelloItems.Select(item => new OrdiniProdotti
                        {
                            ProdottoId = item.Prodotto.IdProdotti,
                            Quantita = item.Quantita
                        }).ToList()
                    };

                    db.Ordini.Add(nuovoOrdine);
                    db.SaveChanges();

                    Session["Carrello"] = null;

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
