using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Mig9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TagBackgroundColor",
                schema: "dbo",
                table: "Services",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "#000000");

            migrationBuilder.AddColumn<string>(
                name: "TagTextColor",
                schema: "dbo",
                table: "Services",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "#000000");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagBackgroundColor",
                schema: "dbo",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "TagTextColor",
                schema: "dbo",
                table: "Services");
        }
    }
}
