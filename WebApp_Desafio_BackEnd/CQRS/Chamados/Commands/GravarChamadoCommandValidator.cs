using FluentValidation;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class GravarChamadoCommandValidator : AbstractValidator<GravarChamadoCommand>
    {
        public GravarChamadoCommandValidator()
        {
            RuleFor(c => c.Assunto)
                .NotEmpty().WithMessage("O Assunto é obrigatório.")
                .MaximumLength(100).WithMessage("O Assunto não pode exceder 100 caracteres.");

            RuleFor(c => c.IdSolicitante)
                .GreaterThan(0).WithMessage("O Solicitante é obrigatório.");

            RuleFor(c => c.IdDepartamento)
                .GreaterThan(0).WithMessage("O Departamento é obrigatório.");
        }
    }
}