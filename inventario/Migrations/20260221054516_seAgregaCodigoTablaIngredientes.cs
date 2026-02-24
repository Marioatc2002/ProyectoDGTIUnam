using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventario.Migrations
{
    /// <inheritdoc />
    public partial class seAgregaCodigoTablaIngredientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Ingredientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Ingredientes");
        }
    }
}
