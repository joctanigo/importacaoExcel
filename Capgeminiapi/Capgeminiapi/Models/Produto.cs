using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capgeminiapi.Models
{
    public class Produto
    { 
        [Key]
        public int IdProduto { get; set; }
        public int idLoteImportacao { get; set; }
        public DateTime DataEntrega { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
