using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Gestionale_Pizzeria.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<OrdiniProdotti> OrdiniProdotti { get; set; }
        public virtual DbSet<Prodotti> Prodotti { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ordini>()
                .HasMany(e => e.OrdiniProdotti)
                .WithRequired(e => e.Ordini)
                .HasForeignKey(e => e.OrdineId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Prodotti>()
                .Property(e => e.Prezzo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Prodotti>()
                .HasMany(e => e.OrdiniProdotti)
                .WithRequired(e => e.Prodotti)
                .HasForeignKey(e => e.ProdottoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Ordini)
                .WithRequired(e => e.Utenti)
                .HasForeignKey(e => e.UtenteId)
                .WillCascadeOnDelete(false);
        }
    }
}
