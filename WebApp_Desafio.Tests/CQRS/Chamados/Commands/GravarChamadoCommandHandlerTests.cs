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
                ID = 0,
                Assunto = "Teste de Assunto",
                IdSolicitante = 1,
                IdDepartamento = 1,
                DataAbertura = DateTime.Now
            };

            var solicitante = new Solicitante { ID = 1, Nome = "Solicitante Teste" };

            _solicitantesDalMock.Setup(s => s.ObterSolicitante(command.IdSolicitante)).ReturnsAsync(solicitante);

            _chamadosDalMock.Setup(c => c.GravarChamado(It.IsAny<Chamado>())).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _solicitantesDalMock.Verify(s => s.ObterSolicitante(command.IdSolicitante), Times.Once);
            _chamadosDalMock.Verify(c => c.GravarChamado(It.IsAny<Chamado>()), Times.Once);
        }

        [Fact]
        public async Task Handle_QuandoDataForRetroativa_DeveLancarApplicationException()
        {
            // Arrange
            var command = new GravarChamadoCommand
            {
                ID = 0,
                DataAbertura = DateTime.Now.AddDays(-1)
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>()
                .WithMessage("A data de abertura não pode ser retroativa.");
        }
    }
}