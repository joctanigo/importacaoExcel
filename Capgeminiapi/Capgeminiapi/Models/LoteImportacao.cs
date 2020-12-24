using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capgeminiapi.Models
{
    public class LoteImportacao
    {
        [Key]
        public int IdImportacao { get; set; }
        public DateTime DataImportacao { get; set; }
        public int QuantidadeItens { get; set; }
        public DateTime DataMenorEntrega { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
