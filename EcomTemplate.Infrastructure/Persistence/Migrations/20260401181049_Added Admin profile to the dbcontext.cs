using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomTemplate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdminprofiletothedbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdminId",
                table: "refresh_tokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminProfileAdminId",
                table: "refresh_tokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdminProfile",
                columns: table => new
                {
                    AdminId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "integer", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminProfile", x => x.AdminId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_AdminProfileAdminId",
                table: "refresh_tokens",
                column: "AdminProfileAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_AdminProfile_AdminProfileAdminId",
                table: "refresh_tokens",
                column: "AdminProfileAdminId",
                principalTable: "AdminProfile",
                principalColumn: "AdminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_AdminProfile_AdminProfileAdminId",
                table: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "AdminProfile");

            migrationBuilder.DropIndex(
                name: "IX_refresh_tokens_AdminProfileAdminId",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "AdminProfileAdminId",
                table: "refresh_tokens");
        }
    }
}
