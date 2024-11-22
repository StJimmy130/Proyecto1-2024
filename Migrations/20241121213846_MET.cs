using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_2024.Migrations
{
    /// <inheritdoc />
    public partial class MET : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MET",
                table: "TipoEjercicios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MET",
                table: "TipoEjercicios");
        }
    }
}
