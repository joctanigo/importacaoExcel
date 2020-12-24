using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capgeminiapi.Contexts;
using Capgeminiapi.Models;
using Capgeminiapi.Services.ProdutoService;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Microsoft.AspNetCore.Hosting;

namespace Capgeminiapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;

       

        private readonly ApiContext _context;
    

        public ProdutosController(ApiContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
           
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        [HttpGet("getAllImports/{id}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAllImports(int id)
        {
            var produtos = await _context.Produtos.ToListAsync();
            var retorno = produtos.Where(x => x.idLoteImportacao == id).ToList();
            return retorno;
        }

     

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.IdProduto)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            ProdutoValidator validator = new ProdutoValidator();

            if (!validator.Validate(produto).IsValid){
                return BadRequest(
                );
            }
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.IdProduto }, produto);
        }

       
       
        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.IdProduto == id);
        }
    }
}
