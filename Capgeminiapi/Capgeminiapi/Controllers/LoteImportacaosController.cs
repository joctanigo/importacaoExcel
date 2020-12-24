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
    public class LoteImportacaosController : ControllerBase
    {

        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ApiContext _context;
        private readonly ProdutoService _produtoService;

        public LoteImportacaosController(ProdutoService produtoService, ApiContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _produtoService = produtoService;
        }

        // GET: api/LoteImportacaos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoteImportacao>>> GetLoteImportacao()
        {
            return await _context.LoteImportacao.ToListAsync();
        }

        // GET: api/LoteImportacaos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoteImportacao>> GetLoteImportacao(int id)
        {
            var loteImportacao = await _context.LoteImportacao.FindAsync(id);

            if (loteImportacao == null)
            {
                return NotFound();
            }

            return loteImportacao;
        }

        

        // POST: api/LoteImportacaos
        [HttpPost]
        public async Task<ActionResult<LoteImportacao>> PostLoteImportacao(LoteImportacao loteImportacao)
        {
            _context.LoteImportacao.Add(loteImportacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoteImportacao", new { id = loteImportacao.IdImportacao }, loteImportacao);
        }

     
        [HttpPost("EnviarArquivo")]
        public async Task<ActionResult<LoteImportacao>> UploadFile(IFormFile file)

        {

            List<Produto> produtos = new List<Produto>();
            List<string> excelData = new List<string>();

            DateTime menorDataEntrega = DateTime.MaxValue;
            int quantidadeTotal = 0;
            decimal valorTotal = 0; 

            using (MemoryStream stream = new MemoryStream())
            {

                await file.CopyToAsync(stream).ConfigureAwait(false);
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {

                    foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                    {

                        for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                        {

                            for (int j = worksheet.Dimension.Start.Column; j <=
                           worksheet.Dimension.End.Column; j++)
                            {

                                if (worksheet.Cells[i, j].Value != null)
                                {
                                    excelData.Add(worksheet.Cells[i, j].Value.ToString());
                                }
                                else
                                {
                                    excelData.Add("");
                                }
                            }
                        }
                    }
                    var erros = new List<string>();
                    for (int i = 0; i < excelData.Count(); i = i + 4)
                    {
                        if(menorDataEntrega > Convert.ToDateTime(excelData[i]))
                        {
                            menorDataEntrega = Convert.ToDateTime(excelData[i]);
                        }
                        quantidadeTotal += Convert.ToInt32(excelData[i + 2]);
                        valorTotal += Convert.ToDecimal(excelData[i + 3]);

                        produtos.Add(new Produto()
                        {
                            DataEntrega = Convert.ToDateTime(excelData[i]),
                            NomeProduto = excelData[i + 1],
                            Quantidade = Convert.ToInt32(excelData[i + 2]),
                            ValorUnitario = Convert.ToDecimal(excelData[i + 3]),
                        });
                        ProdutoValidator validator = new ProdutoValidator();
                        var result = validator.Validate(produtos.Last());

                        foreach (var failure in result.Errors)
                        {
                            erros.Add($"Coluna: {failure.PropertyName}, Linha: {produtos.Count()}  Error : {failure.ErrorMessage}");
                        }

                    }

                    if (erros.Count() > 0)
                    {
                        return BadRequest(erros);
                    }
                    else
                    {


                        //await _context.Produtos.AddRangeAsync(produtos);
                        LoteImportacao lote = new LoteImportacao();

                        lote.DataImportacao = DateTime.Today;
                        lote.DataMenorEntrega = menorDataEntrega;
                        lote.ValorTotal = valorTotal;
                        lote.QuantidadeItens = quantidadeTotal;

                        var loteResult =_context.LoteImportacao.Add(lote);
                        foreach (Produto item in produtos)
                        {
                            item.idLoteImportacao = loteResult.Entity.IdImportacao;
                        }
                        var retorno = _produtoService.addProdutos(produtos);
                       

                        _context.SaveChanges();

                        return loteResult.Entity;
                    }

                }

            }

        }

        private bool LoteImportacaoExists(int id)
        {
            return _context.LoteImportacao.Any(e => e.IdImportacao == id);
        }
    }
}

