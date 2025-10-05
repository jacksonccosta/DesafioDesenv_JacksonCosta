using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Handle_QuandoComandoValidoParaNovoChamado_DeveSalvarNoBanco()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var handler = new GravarChamadoCommandHandler(context);

            var solicitante = new Solicitante { ID = 1, Nome = "Solicitante Teste" };
            var departamento = new Departamento { ID = 1, Descricao = "Depto Teste" };

            context.Solicitantes.Add(solicitante);
            context.Departamentos.Add(departamento);
            await context.SaveChangesAsync();

            var command = new GravarChamadoCommand
            {
                ID = 0, // Novo chamado
                Assunto = "Teste de Assunto",
                IdSolicitante = 1,
                IdDepartamento = 1,
                DataAbertura = DateTime.Now
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            var chamadoSalvo = await context.Chamados.FirstOrDefaultAsync();
            chamadoSalvo.Should().NotBeNull();
            chamadoSalvo.Assunto.Should().Be("Teste de Assunto");
            chamadoSalvo.IdSolicitante.Should().Be(1);
        }

        [Fact]
        public async Task Handle_QuandoDataForRetroativa_DeveLancarApplicationException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var handler = new GravarChamadoCommandHandler(context);
            var command = new GravarChamadoCommand
            {
                ID = 0,
                DataAbertura = DateTime.Now.AddDays(-1)
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>()
                     .WithMessage("Não é permitido CRIAR chamados com data retroativa.");
        }

        [Fact]
        public async Task Handle_QuandoSolicitanteNaoEncontrado_DeveLancarApplicationException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var handler = new GravarChamadoCommandHandler(context);
            var command = new GravarChamadoCommand
            {
                ID = 0,
                IdSolicitante = 999, // ID inexistente
                IdDepartamento = 1,
                DataAbertura = DateTime.Now
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>()
                     .WithMessage($"Solicitante com ID {command.IdSolicitante} não encontrado.");
        }
    }
}