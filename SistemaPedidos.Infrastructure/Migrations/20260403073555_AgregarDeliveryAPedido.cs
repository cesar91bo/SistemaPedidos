using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPedidos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDeliveryAPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryId",
                table: "Pedidos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_DeliveryId",
                table: "Pedidos",
                column: "DeliveryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Deliveries_DeliveryId",
                table: "Pedidos",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Deliveries_DeliveryId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_DeliveryId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "DeliveryId",
                table: "Pedidos");
        }
    }
}
