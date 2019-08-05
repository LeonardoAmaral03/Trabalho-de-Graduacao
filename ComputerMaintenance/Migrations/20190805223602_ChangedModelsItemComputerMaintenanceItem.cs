using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerMaintenance.Migrations
{
    public partial class ChangedModelsItemComputerMaintenanceItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemComputers_Computers_ComputerId",
                table: "ItemComputers");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemComputers_Items_ItemId",
                table: "ItemComputers");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceItems_Items_ItemId",
                table: "MaintenanceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceItems_Maintenances_MaintenanceId",
                table: "MaintenanceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaintenanceItems",
                table: "MaintenanceItems");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceItems_ItemId",
                table: "MaintenanceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemComputers",
                table: "ItemComputers");

            migrationBuilder.DropIndex(
                name: "IX_ItemComputers_ComputerId",
                table: "ItemComputers");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaintenanceId",
                table: "MaintenanceItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "MaintenanceItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "ItemComputers",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ComputerId",
                table: "ItemComputers",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaintenanceItems",
                table: "MaintenanceItems",
                columns: new[] { "ItemId", "MaintenanceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemComputers",
                table: "ItemComputers",
                columns: new[] { "ComputerId", "ItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ItemComputers_Computers_ComputerId",
                table: "ItemComputers",
                column: "ComputerId",
                principalTable: "Computers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemComputers_Items_ItemId",
                table: "ItemComputers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceItems_Items_ItemId",
                table: "MaintenanceItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceItems_Maintenances_MaintenanceId",
                table: "MaintenanceItems",
                column: "MaintenanceId",
                principalTable: "Maintenances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemComputers_Computers_ComputerId",
                table: "ItemComputers");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemComputers_Items_ItemId",
                table: "ItemComputers");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceItems_Items_ItemId",
                table: "MaintenanceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceItems_Maintenances_MaintenanceId",
                table: "MaintenanceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaintenanceItems",
                table: "MaintenanceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemComputers",
                table: "ItemComputers");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaintenanceId",
                table: "MaintenanceItems",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "MaintenanceItems",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "ItemComputers",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "ComputerId",
                table: "ItemComputers",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaintenanceItems",
                table: "MaintenanceItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemComputers",
                table: "ItemComputers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceItems_ItemId",
                table: "MaintenanceItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemComputers_ComputerId",
                table: "ItemComputers",
                column: "ComputerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemComputers_Computers_ComputerId",
                table: "ItemComputers",
                column: "ComputerId",
                principalTable: "Computers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemComputers_Items_ItemId",
                table: "ItemComputers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceItems_Items_ItemId",
                table: "MaintenanceItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceItems_Maintenances_MaintenanceId",
                table: "MaintenanceItems",
                column: "MaintenanceId",
                principalTable: "Maintenances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
