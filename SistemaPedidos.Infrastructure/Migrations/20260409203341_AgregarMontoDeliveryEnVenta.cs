using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPedidos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMontoDeliveryEnVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MontoDelivery",
                table: "Ventas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MontoDelivery",
                table: "Ventas");
        }
    }
}
