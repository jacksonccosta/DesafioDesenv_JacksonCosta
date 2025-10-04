using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.CQRS.Chamados.Commands;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;
using Xunit;

namespace WebApp_Desafio_BackEnd.Tests.CQRS.Chamados.Commands
{
    public class GravarChamadoCommandHandlerTests
    {
        private readonly Mock<IChamadosDAL> _chamadosDalMock;
        private readonly Mock<ISolicitantesDAL> _solicitantesDalMock;
        private readonly GravarChamadoCommandHandler _handler;

        public GravarChamadoCommandHandlerTests()
        {
            // Arrange (Setup inicial para todos os testes)
            _chamadosDalMock = new Mock<IChamadosDAL>();
            _solicitantesDalMock = new Mock<ISolicitantesDAL>();
            _handler = new GravarChamadoCommandHandler(_chamadosDalMock.Object, _solicitantesDalMock.Object);
        }

        [Fact]
        public async Task Handle_QuandoComandoValidoParaNovoChamado_DeveRetornarTrue()
        {
            // Arrange
            var command = new GravarChamadoCommand
            {
                ID = 0, // Novo chamado
                Assunto = "Teste de Assunto",
                IdSolicitante = 1,
                IdDepartamento = 1,
                DataAbertura = DateTime.Now
            };

            var solicitante = new Solicitante { ID = 1, Nome = "Solicitante Teste" };

            // Configura o Mock para simular que o solicitante foi encontrado
            _solicitantesDalMock.Setup(s => s.ObterSolicitante(command.IdSolicitante)).Returns(solicitante);

            // Configura o Mock para simular que a gravação no banco de dados teve sucesso
            _chamadosDalMock.Setup(c => c.GravarChamado(It.IsAny<Chamado>())).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            // Verifica se o método ObterSolicitante foi chamado exatamente uma vez
            _solicitantesDalMock.Verify(s => s.ObterSolicitante(command.IdSolicitante), Times.Once);
            // Verifica se o método GravarChamado foi chamado exatamente uma vez
            _chamadosDalMock.Verify(c => c.GravarChamado(It.IsAny<Chamado>()), Times.Once);
        }

        [Fact]
        public async Task Handle_QuandoDataForRetroativa_DeveLancarApplicationException()
        {
            // Arrange
            var command = new GravarChamadoCommand
            {
                ID = 0,
                DataAbertura = DateTime.Now.AddDays(-1) // Data no passado
            };

            // Act
            // Usamos uma Action para encapsular a chamada que esperamos que lance uma exceção
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>()
                     .WithMessage("Não é permitido CRIAR chamados com data retroativa.");
        }

        [Fact]
        public async Task Handle_QuandoSolicitanteNaoEncontrado_DeveLancarApplicationException()
        {
            // Arrange
            var command = new GravarChamadoCommand
            {
                ID = 0,
                IdSolicitante = 999, // ID inexistente
                DataAbertura = DateTime.Now
            };

            // Configura o Mock para simular que o solicitante NÃO foi encontrado (retorna null)
            _solicitantesDalMock.Setup(s => s.ObterSolicitante(command.IdSolicitante)).Returns((Solicitante)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>()
                     .WithMessage($"Solicitante com ID {command.IdSolicitante} não encontrado.");
        }
    }
}