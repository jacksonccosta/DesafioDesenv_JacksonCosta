using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.CQRS.Chamados.Queries;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;
using Xunit;

namespace WebApp_Desafio_BackEnd.Tests.CQRS.Chamados.Queries
{
    public class GetChamadoByIdQueryHandlerTests
    {
        private readonly Mock<IChamadosDAL> _chamadosDalMock;
        private readonly GetChamadoByIdQueryHandler _handler;

        public GetChamadoByIdQueryHandlerTests()
        {
            _chamadosDalMock = new Mock<IChamadosDAL>();
            _handler = new GetChamadoByIdQueryHandler(_chamadosDalMock.Object);
        }

        [Fact]
        public async Task Handle_QuandoChamadoExiste_DeveRetornarChamado()
        {
            // Arrange
            var query = new GetChamadoByIdQuery { Id = 1 };
            var chamadoEsperado = new Chamado { ID = 1, Assunto = "Chamado Existente" };

            _chamadosDalMock.Setup(d => d.ObterChamado(query.Id)).ReturnsAsync(chamadoEsperado);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ID.Should().Be(chamadoEsperado.ID);
            result.Assunto.Should().Be(chamadoEsperado.Assunto);
        }

        [Fact]
        public async Task Handle_QuandoChamadoNaoExiste_DeveLancarApplicationException()
        {
            // Arrange
            var query = new GetChamadoByIdQuery { Id = 999 };
            _chamadosDalMock.Setup(d => d.ObterChamado(query.Id)).ReturnsAsync((Chamado)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>().WithMessage("Chamado não encontrado.");
        }
    }
}