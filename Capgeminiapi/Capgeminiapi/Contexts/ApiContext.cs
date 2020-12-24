using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capgeminiapi.Models;
using Microsoft.EntityFrameworkCore;

namespace Capgeminiapi.Contexts
{
    public class ApiContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<LoteImportacao> LoteImportacao { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Produto>().ToTable("Produto");
            builder.Entity<Produto>().HasKey(r => r.IdProduto);
            builder.Entity<Produto>().Property(r => r.DataEntrega).IsRequired();
            builder.Entity<Produto>().Property(r => r.NomeProduto).IsRequired().HasMaxLength(50);
            builder.Entity<Produto>().Property(r => r.Quantidade).IsRequired();
            builder.Entity<Produto>().Property(r => r.ValorUnitario).IsRequired();

            builder.Entity<LoteImportacao>().ToTable("LoteImportacao");
            builder.Entity<LoteImportacao>().HasKey(r => r.IdImportacao);
            builder.Entity<LoteImportacao>().Property(r => r.DataImportacao).IsRequired();
            builder.Entity<LoteImportacao>().Property(r => r.QuantidadeItens).IsRequired();
            builder.Entity<LoteImportacao>().Property(r => r.DataMenorEntrega).IsRequired();
            builder.Entity<LoteImportacao>().Property(r => r.ValorTotal).IsRequired();
          
    }

  
    }
}
