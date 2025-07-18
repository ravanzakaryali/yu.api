using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeleteOrders");

            migrationBuilder.DropTable(
                name: "OrderPickupDateSetting");

            migrationBuilder.DropTable(
                name: "OrderReasons");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "House",
                table: "Addresses",
                newName: "SubDoor");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Addresses",
                newName: "Intercom");

            migrationBuilder.AddColumn<int>(
                name: "PickupDateSettingId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apartment",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullAddress",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CancelOrderReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancelOrderReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CancelOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasonId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancelOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancelOrders_CancelOrderReasons_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "CancelOrderReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CancelOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PickupDateSettingId",
                table: "Orders",
                column: "PickupDateSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_CancelOrders_OrderId",
                table: "CancelOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CancelOrders_ReasonId",
                table: "CancelOrders",
                column: "ReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PickupDateSettings_PickupDateSettingId",
                table: "Orders",
                column: "PickupDateSettingId",
                principalTable: "PickupDateSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PickupDateSettings_PickupDateSettingId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "CancelOrders");

            migrationBuilder.DropTable(
                name: "CancelOrderReasons");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PickupDateSettingId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PickupDateSettingId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "FullAddress",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "SubDoor",
                table: "Addresses",
                newName: "House");

            migrationBuilder.RenameColumn(
                name: "Intercom",
                table: "Addresses",
                newName: "City");

            migrationBuilder.AlterColumn<string>(
                name: "Apartment",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderPickupDateSetting",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    PickupDateSettingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPickupDateSetting", x => new { x.OrdersId, x.PickupDateSettingsId });
                    table.ForeignKey(
                        name: "FK_OrderPickupDateSetting_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderPickupDateSetting_PickupDateSettings_PickupDateSettingsId",
                        column: x => x.PickupDateSettingsId,
                        principalTable: "PickupDateSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeleteOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ReasonId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeleteOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeleteOrders_OrderReasons_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "OrderReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeleteOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeleteOrders_OrderId",
                table: "DeleteOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeleteOrders_ReasonId",
                table: "DeleteOrders",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPickupDateSetting_PickupDateSettingsId",
                table: "OrderPickupDateSetting",
                column: "PickupDateSettingsId");
        }
    }
}
