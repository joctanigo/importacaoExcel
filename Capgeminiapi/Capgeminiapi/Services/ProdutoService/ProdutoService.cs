using Capgeminiapi.Contexts;
using Capgeminiapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Capgeminiapi.Services.ProdutoService
{
    public class ProdutoService
    {
        private readonly ApiContext _context;

        public ProdutoService(ApiContext context)
        {
            _context = context;
        }

        public ProdutoService()
        {
        }

        public List<Produto> addProdutos(List<Produto> produto)
        {
            _context.Produtos.AddRangeAsync(produto);
            _context.SaveChanges();
            return produto;
        }

        public LoteImportacao addLote()
        {
            LoteImportacao lote = new LoteImportacao();
            return lote;
        }
    }
}
