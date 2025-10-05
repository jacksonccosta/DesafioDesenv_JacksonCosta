using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp_Desafio_BackEnd.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Solicitantes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitantes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Chamados",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Assunto = table.Column<string>(nullable: false),
                    IdSolicitante = table.Column<int>(nullable: false),
                    IdDepartamento = table.Column<int>(nullable: false),
                    DataAbertura = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chamados", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Chamados_Departamentos_IdDepartamento",
                        column: x => x.IdDepartamento,
                        principalTable: "Departamentos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chamados_Solicitantes_IdSolicitante",
                        column: x => x.IdSolicitante,
                        principalTable: "Solicitantes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_IdDepartamento",
                table: "Chamados",
                column: "IdDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_IdSolicitante",
                table: "Chamados",
                column: "IdSolicitante");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chamados");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "Solicitantes");
        }
    }
}
