using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPedidos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CrearPagoDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PagosDelivery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FormaPago = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosDelivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagosDelivery_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PagosDeliveryDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagoDeliveryId = table.Column<int>(type: "int", nullable: false),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    MontoEnvio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosDeliveryDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagosDeliveryDetalle_PagosDelivery_PagoDeliveryId",
                        column: x => x.PagoDeliveryId,
                        principalTable: "PagosDelivery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PagosDeliveryDetalle_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PagosDelivery_DeliveryId",
                table: "PagosDelivery",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosDeliveryDetalle_PagoDeliveryId",
                table: "PagosDeliveryDetalle",
                column: "PagoDeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosDeliveryDetalle_PedidoId",
                table: "PagosDeliveryDetalle",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PagosDeliveryDetalle");

            migrationBuilder.DropTable(
                name: "PagosDelivery");
        }
    }
}
