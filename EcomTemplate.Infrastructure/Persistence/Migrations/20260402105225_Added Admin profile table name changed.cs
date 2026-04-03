using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomTemplate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdminprofiletablenamechanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_AdminProfile_AdminProfileAdminId",
                table: "refresh_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminProfile",
                table: "AdminProfile");

            migrationBuilder.RenameTable(
                name: "AdminProfile",
                newName: "admin_profile");

            migrationBuilder.AddPrimaryKey(
                name: "PK_admin_profile",
                table: "admin_profile",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_admin_profile_AdminProfileAdminId",
                table: "refresh_tokens",
                column: "AdminProfileAdminId",
                principalTable: "admin_profile",
                principalColumn: "AdminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_admin_profile_AdminProfileAdminId",
                table: "refresh_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_admin_profile",
                table: "admin_profile");

            migrationBuilder.RenameTable(
                name: "admin_profile",
                newName: "AdminProfile");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminProfile",
                table: "AdminProfile",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_AdminProfile_AdminProfileAdminId",
                table: "refresh_tokens",
                column: "AdminProfileAdminId",
                principalTable: "AdminProfile",
                principalColumn: "AdminId");
        }
    }
}
