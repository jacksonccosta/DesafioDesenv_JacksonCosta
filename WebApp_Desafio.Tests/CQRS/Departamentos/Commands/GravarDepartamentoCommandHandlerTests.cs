using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;
using Xunit;

namespace WebApp_Desafio_BackEnd.Tests.CQRS.Departamentos.Commands
{
    public class GravarDepartamentoCommandHandlerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Handle_QuandoComandoValido_DeveSalvarNoBanco()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var handler = new GravarDepartamentoCommandHandler(context);
            var command = new GravarDepartamentoCommand { Descricao = "RH" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            var departamentoSalvo = await context.Departamentos.FirstOrDefaultAsync(d => d.Descricao == "RH");
            departamentoSalvo.Should().NotBeNull();
            departamentoSalvo.Descricao.Should().Be("RH");
        }
    }
}