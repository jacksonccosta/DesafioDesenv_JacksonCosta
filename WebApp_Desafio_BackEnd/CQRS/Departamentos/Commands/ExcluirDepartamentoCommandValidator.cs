using FluentValidation;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands
{
    public class ExcluirDepartamentoCommandValidator : AbstractValidator<ExcluirDepartamentoCommand>
    {
        public ExcluirDepartamentoCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("O ID do departamento para exclusão é inválido.");
        }
    }
}