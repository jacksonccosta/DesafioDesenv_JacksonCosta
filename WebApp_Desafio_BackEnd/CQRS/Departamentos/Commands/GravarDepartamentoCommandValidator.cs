using FluentValidation;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands
{
    public class GravarDepartamentoCommandValidator : AbstractValidator<GravarDepartamentoCommand>
    {
        public GravarDepartamentoCommandValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("O campo 'Descrição' é obrigatório.")
                .MaximumLength(100).WithMessage("O campo 'Descrição' deve ter no máximo 100 caracteres.");
        }
    }
}