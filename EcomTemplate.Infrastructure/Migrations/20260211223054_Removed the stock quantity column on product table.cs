using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrocerySupermarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removedthestockquantitycolumnonproducttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
