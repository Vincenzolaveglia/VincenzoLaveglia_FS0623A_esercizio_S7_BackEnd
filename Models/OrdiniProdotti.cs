namespace Gestionale_Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdiniProdotti")]
    public partial class OrdiniProdotti
    {
        public int Id { get; set; }

        public int Quantita { get; set; }

        public int OrdineId { get; set; }

        public int ProdottoId { get; set; }

        public virtual Ordini Ordini { get; set; }

        public virtual Prodotti Prodotti { get; set; }
    }
}
