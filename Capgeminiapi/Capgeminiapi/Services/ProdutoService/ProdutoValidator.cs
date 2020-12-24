using Capgeminiapi.Models;
using FluentValidation;
using System;

namespace Capgeminiapi.Services.ProdutoService
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(produto => produto.DataEntrega).NotNull().Must(ValidDate).WithMessage("Data de entrega inválida");
            RuleFor(produto => produto.NomeProduto).NotNull().MaximumLength(50).WithMessage("Descrição do produto deve ter no maximo 50 caracteres"); ;
            RuleFor(produto => produto.Quantidade).NotNull().GreaterThan(0).WithMessage("Quantidade inválida"); ;
            RuleFor(produto => produto.ValorUnitario).NotNull().GreaterThan(0).WithMessage("Valor do produto inválida"); ;
        }

        private bool ValidDate(DateTime date)
        {
            if (date > DateTime.Now)
                return true;

            return false;
        }
    }
}
