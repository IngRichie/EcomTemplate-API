using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomTemplate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updatedtherefreshtokenentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_customer_profiles_CustomerProfileId",
                table: "refresh_tokens");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerProfileId",
                table: "refresh_tokens",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_customer_profiles_CustomerProfileId",
                table: "refresh_tokens",
                column: "CustomerProfileId",
                principalTable: "customer_profiles",
                principalColumn: "CustomerProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_customer_profiles_CustomerProfileId",
                table: "refresh_tokens");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerProfileId",
                table: "refresh_tokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_customer_profiles_CustomerProfileId",
                table: "refresh_tokens",
                column: "CustomerProfileId",
                principalTable: "customer_profiles",
                principalColumn: "CustomerProfileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
