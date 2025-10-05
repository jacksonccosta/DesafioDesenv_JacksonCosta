using FluentValidation;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class ExcluirChamadoCommandValidator : AbstractValidator<ExcluirChamadoCommand>
    {
        public ExcluirChamadoCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("O ID do chamado para exclusão é inválido.");
        }
    }
}