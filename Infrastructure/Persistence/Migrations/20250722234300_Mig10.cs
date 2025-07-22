using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Mig10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IconId",
                schema: "dbo",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_IconId",
                schema: "dbo",
                table: "Services",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Files_IconId",
                schema: "dbo",
                table: "Services",
                column: "IconId",
                principalSchema: "dbo",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Files_IconId",
                schema: "dbo",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_IconId",
                schema: "dbo",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IconId",
                schema: "dbo",
                table: "Services");
        }
    }
}
