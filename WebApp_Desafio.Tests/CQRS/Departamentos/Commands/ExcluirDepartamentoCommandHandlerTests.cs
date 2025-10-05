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
    public class ExcluirDepartamentoCommandHandlerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Handle_QuandoIdValido_DeveExcluirDoBancoERetornarTrue()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var departamento = new Departamento { ID = 1, Descricao = "Financeiro" };
            context.Departamentos.Add(departamento);
            await context.SaveChangesAsync();

            var handler = new ExcluirDepartamentoCommandHandler(context);
            var command = new ExcluirDepartamentoCommand { Id = 1 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            var departamentoNoBanco = await context.Departamentos.FindAsync(1);
            departamentoNoBanco.Should().BeNull();
        }
    }
}