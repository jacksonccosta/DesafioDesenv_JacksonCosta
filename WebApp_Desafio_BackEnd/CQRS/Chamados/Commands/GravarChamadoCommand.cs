using MediatR;
using System;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class GravarChamadoCommand : IRequest<bool>
    {
        public int ID { get; set; }
        public string Assunto { get; set; }
        public string Solicitante { get; set; }
        public int IdDepartamento { get; set; }
        public DateTime DataAbertura { get; set; }
    }
}