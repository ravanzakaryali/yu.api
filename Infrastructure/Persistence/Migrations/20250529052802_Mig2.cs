using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "id",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Surname",
                schema: "id",
                table: "Users",
                newName: "FullName");

            migrationBuilder.AddColumn<int>(
                name: "EstimatedDays",
                schema: "dbo",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedDays",
                schema: "dbo",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "FullName",
                schema: "id",
                table: "Users",
                newName: "Surname");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "id",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
