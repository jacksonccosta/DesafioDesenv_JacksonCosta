using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.CQRS.Chamados.Commands;
using WebApp_Desafio_BackEnd.DataAccess;
using Xunit;

namespace WebApp_Desafio_BackEnd.Tests.CQRS.Chamados.Commands
{
    public class ExcluirChamadoCommandHandlerTests
    {
        private readonly Mock<IChamadosDAL> _chamadosDalMock;
        private readonly ExcluirChamadoCommandHandler _handler;

        public ExcluirChamadoCommandHandlerTests()
        {
            _chamadosDalMock = new Mock<IChamadosDAL>();
            _handler = new ExcluirChamadoCommandHandler(_chamadosDalMock.Object);
        }

        [Fact]
        public async Task Handle_QuandoIdValido_DeveChamarDalERetornarTrue()
        {
            // Arrange
            var command = new ExcluirChamadoCommand { Id = 1 };
            _chamadosDalMock.Setup(d => d.ExcluirChamado(command.Id)).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _chamadosDalMock.Verify(d => d.ExcluirChamado(command.Id), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_QuandoIdInvalido_DeveLancarArgumentException(int invalidId)
        {
            // Arrange
            var command = new ExcluirChamadoCommand { Id = invalidId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("O ID do chamado é inválido.");
        }
    }
}