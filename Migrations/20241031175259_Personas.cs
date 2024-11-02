using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_2024.Migrations
{
    /// <inheritdoc />
    public partial class Personas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Lugares",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonaID",
                table: "Lugares",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonaID",
                table: "EjerciciosFisicos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    PersonaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    Genero = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Altura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CuentaID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.PersonaID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lugares_PersonaID",
                table: "Lugares",
                column: "PersonaID");

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosFisicos_PersonaID",
                table: "EjerciciosFisicos",
                column: "PersonaID");

            migrationBuilder.AddForeignKey(
                name: "FK_EjerciciosFisicos_Personas_PersonaID",
                table: "EjerciciosFisicos",
                column: "PersonaID",
                principalTable: "Personas",
                principalColumn: "PersonaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lugares_Personas_PersonaID",
                table: "Lugares",
                column: "PersonaID",
                principalTable: "Personas",
                principalColumn: "PersonaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjerciciosFisicos_Personas_PersonaID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lugares_Personas_PersonaID",
                table: "Lugares");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Lugares_PersonaID",
                table: "Lugares");

            migrationBuilder.DropIndex(
                name: "IX_EjerciciosFisicos_PersonaID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropColumn(
                name: "PersonaID",
                table: "Lugares");

            migrationBuilder.DropColumn(
                name: "PersonaID",
                table: "EjerciciosFisicos");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Lugares",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
