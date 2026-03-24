using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcomTemplate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Removedthehostelnameanduniversitynamecolumnsandthenaddedlocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryInstructions",
                table: "customer_profiles");

            migrationBuilder.DropColumn(
                name: "HostelName",
                table: "customer_profiles");

            migrationBuilder.DropColumn(
                name: "RoomNumber",
                table: "customer_profiles");

            migrationBuilder.RenameColumn(
                name: "UniversityName",
                table: "customer_profiles",
                newName: "Location");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "customer_profiles",
                newName: "UniversityName");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryInstructions",
                table: "customer_profiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostelName",
                table: "customer_profiles",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomNumber",
                table: "customer_profiles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
