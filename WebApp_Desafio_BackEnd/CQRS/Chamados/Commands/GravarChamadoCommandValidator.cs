using FluentValidation;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class GravarChamadoCommandValidator : AbstractValidator<GravarChamadoCommand>
    {
        public GravarChamadoCommandValidator()
        {
            RuleFor(x => x.Assunto)
                .NotEmpty().WithMessage("O campo 'Assunto' é obrigatório.")
                .MaximumLength(100).WithMessage("O campo 'Assunto' deve ter no máximo 100 caracteres.");

            RuleFor(x => x.IdSolicitante)
                .GreaterThan(0).WithMessage("O campo 'Solicitante' é obrigatório.");

            RuleFor(x => x.IdDepartamento)
                .GreaterThan(0).WithMessage("O campo 'Departamento' é obrigatório.");

            RuleFor(x => x.DataAbertura)
                .NotEmpty().WithMessage("O campo 'Data de Abertura' é obrigatório.");
        }
    }
}