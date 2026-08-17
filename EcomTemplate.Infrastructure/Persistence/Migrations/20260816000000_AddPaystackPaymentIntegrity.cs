using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomTemplate.Infrastructure.Persistence.Migrations
{
    public partial class AddPaystackPaymentIntegrity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProviderReference",
                table: "payments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProviderTransactionId",
                table: "payments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "payments",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "GHS");

            migrationBuilder.AddColumn<string>(
                name: "Channel",
                table: "payments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "payments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "payments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "payments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InventoryFinalized",
                table: "payments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "orders",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "GHS");

            migrationBuilder.AddColumn<string>(
                name: "PaymentReference",
                table: "orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductVariantId",
                table: "order_items",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<string>(
                name: "ProductNameSnapshot",
                table: "order_items",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SkuSnapshot",
                table: "order_items",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "LineTotal",
                table: "order_items",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_payments_ProviderReference",
                table: "payments",
                column: "ProviderReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_CustomerProfileId",
                table: "orders",
                column: "CustomerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_PaymentReference",
                table: "orders",
                column: "PaymentReference");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_ProductVariantId",
                table: "order_items",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_Sku",
                table: "product_variants",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_carts_CustomerProfileId_IsCheckedOut",
                table: "carts",
                columns: new[] { "CustomerProfileId", "IsCheckedOut" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_payments_ProviderReference", table: "payments");
            migrationBuilder.DropIndex(name: "IX_orders_CustomerProfileId", table: "orders");
            migrationBuilder.DropIndex(name: "IX_orders_PaymentReference", table: "orders");
            migrationBuilder.DropIndex(name: "IX_order_items_ProductVariantId", table: "order_items");
            migrationBuilder.DropIndex(name: "IX_product_variants_Sku", table: "product_variants");
            migrationBuilder.DropIndex(name: "IX_carts_CustomerProfileId_IsCheckedOut", table: "carts");

            migrationBuilder.DropColumn(name: "ProviderReference", table: "payments");
            migrationBuilder.DropColumn(name: "ProviderTransactionId", table: "payments");
            migrationBuilder.DropColumn(name: "Currency", table: "payments");
            migrationBuilder.DropColumn(name: "Channel", table: "payments");
            migrationBuilder.DropColumn(name: "FailureReason", table: "payments");
            migrationBuilder.DropColumn(name: "PaidAt", table: "payments");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "payments");
            migrationBuilder.DropColumn(name: "InventoryFinalized", table: "payments");
            migrationBuilder.DropColumn(name: "Currency", table: "orders");
            migrationBuilder.DropColumn(name: "PaymentReference", table: "orders");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "orders");
            migrationBuilder.DropColumn(name: "ProductVariantId", table: "order_items");
            migrationBuilder.DropColumn(name: "ProductNameSnapshot", table: "order_items");
            migrationBuilder.DropColumn(name: "SkuSnapshot", table: "order_items");
            migrationBuilder.DropColumn(name: "LineTotal", table: "order_items");
        }
    }
}
