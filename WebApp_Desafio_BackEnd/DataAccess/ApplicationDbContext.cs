using Microsoft.EntityFrameworkCore;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<Solicitante> Solicitantes { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chamado>(entity =>
            {
                entity.HasOne(c => c.Solicitante)
                      .WithMany()
                      .HasForeignKey(c => c.IdSolicitante)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Departamento)
                      .WithMany()
                      .HasForeignKey(c => c.IdDepartamento)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}