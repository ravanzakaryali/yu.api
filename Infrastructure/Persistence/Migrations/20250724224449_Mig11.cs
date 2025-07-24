using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Mig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClothingItemService");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EstimateHours",
                table: "ClothingItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "EstimateHours",
                table: "ClothingItems");

            migrationBuilder.CreateTable(
                name: "ClothingItemService",
                columns: table => new
                {
                    ClothingItemsId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClothingItemService", x => new { x.ClothingItemsId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_ClothingItemService_ClothingItems_ClothingItemsId",
                        column: x => x.ClothingItemsId,
                        principalTable: "ClothingItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClothingItemService_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalSchema: "dbo",
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClothingItemService_ServicesId",
                table: "ClothingItemService",
                column: "ServicesId");
        }
    }
}
