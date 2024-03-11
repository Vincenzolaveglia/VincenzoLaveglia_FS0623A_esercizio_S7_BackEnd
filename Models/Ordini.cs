namespace Gestionale_Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordini")]
    public partial class Ordini
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordini()
        {
            OrdiniProdotti = new HashSet<OrdiniProdotti>();
        }

        [Key]
        public int IdOrdine { get; set; }

        [Required]
        [StringLength(100)]
        public string IndirizzoConsegna { get; set; }

        [StringLength(1000)]
        public string Nota { get; set; }

        public DateTime? DataOrdine { get; set; }

        [Required]
        [StringLength(50)]
        public string IsCompleto { get; set; }

        public int UtenteId { get; set; }

        public virtual Utenti Utenti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdiniProdotti> OrdiniProdotti { get; set; }
    }
}
