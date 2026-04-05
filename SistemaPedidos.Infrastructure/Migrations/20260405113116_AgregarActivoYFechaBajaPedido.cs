using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPedidos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarActivoYFechaBajaPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Pedidos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Pedidos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosCaja_PedidoId",
                table: "MovimientosCaja",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosCaja_Pedidos_PedidoId",
                table: "MovimientosCaja",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosCaja_Pedidos_PedidoId",
                table: "MovimientosCaja");

            migrationBuilder.DropIndex(
                name: "IX_MovimientosCaja_PedidoId",
                table: "MovimientosCaja");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Pedidos");
        }
    }
}
