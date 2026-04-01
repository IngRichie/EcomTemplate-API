using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomTemplate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoletothecustomerprofileentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "customer_profiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "customer_profiles");
        }
    }
}
