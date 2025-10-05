using FluentValidation;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands
{
    public class GravarDepartamentoCommandValidator : AbstractValidator<GravarDepartamentoCommand>
    {
        public GravarDepartamentoCommandValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A Descrição do departamento é obrigatória.")
                .MaximumLength(100).WithMessage("A Descrição não pode exceder 100 caracteres.");
        }
    }
}