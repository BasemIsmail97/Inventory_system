using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntityToPurchaseANdSalesOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_OrderDate",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_OrderDate",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SalesOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SalesOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "SalesOrders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_OrderDate",
                table: "SalesOrders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_OrderDate",
                table: "PurchaseOrders",
                column: "OrderDate");
        }
    }
}
