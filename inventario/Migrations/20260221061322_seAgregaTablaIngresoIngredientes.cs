using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventario.Migrations
{
    /// <inheritdoc />
    public partial class seAgregaTablaIngresoIngredientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IngresoIngredientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    MotivoEntrada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_Usuario = table.Column<int>(type: "int", nullable: false),
                    Id_ProductoPerecedero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngresoIngredientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngresoIngredientes_Ingredientes_Id_ProductoPerecedero",
                        column: x => x.Id_ProductoPerecedero,
                        principalTable: "Ingredientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngresoIngredientes_Usuarios_Id_Usuario",
                        column: x => x.Id_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngresoIngredientes_Id_ProductoPerecedero",
                table: "IngresoIngredientes",
                column: "Id_ProductoPerecedero");

            migrationBuilder.CreateIndex(
                name: "IX_IngresoIngredientes_Id_Usuario",
                table: "IngresoIngredientes",
                column: "Id_Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngresoIngredientes");
        }
    }
}
