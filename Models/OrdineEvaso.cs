using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestionale_Pizzeria.Models
{
    public class OrdineEvaso
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int TotaleOrdiniOggi { get; set; }
        public decimal TotaleIncasso { get; set; }
    }
}