using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_2024.Migrations
{
    /// <inheritdoc />
    public partial class EventosDeportivos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventoDeportivoID",
                table: "EjerciciosFisicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EventosDeportivos",
                columns: table => new
                {
                    EventoDeportivoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Eliminado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosDeportivos", x => x.EventoDeportivoID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosFisicos_EventoDeportivoID",
                table: "EjerciciosFisicos",
                column: "EventoDeportivoID");

            migrationBuilder.AddForeignKey(
                name: "FK_EjerciciosFisicos_EventosDeportivos_EventoDeportivoID",
                table: "EjerciciosFisicos",
                column: "EventoDeportivoID",
                principalTable: "EventosDeportivos",
                principalColumn: "EventoDeportivoID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjerciciosFisicos_EventosDeportivos_EventoDeportivoID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropTable(
                name: "EventosDeportivos");

            migrationBuilder.DropIndex(
                name: "IX_EjerciciosFisicos_EventoDeportivoID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropColumn(
                name: "EventoDeportivoID",
                table: "EjerciciosFisicos");
        }
    }
}
