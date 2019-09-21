using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerMaintenance.Migrations
{
    public partial class CriadoSchedules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "ItemComputers");

            migrationBuilder.CreateTable(
                name: "ScheduleItemComputers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ComputerId = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItemComputers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleItemComputers_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleItemComputers_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleMaintenanceItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ScheduleItemComputerId = table.Column<Guid>(nullable: false),
                    MaintenanceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleMaintenanceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleMaintenanceItems_Maintenances_MaintenanceId",
                        column: x => x.MaintenanceId,
                        principalTable: "Maintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleMaintenanceItems_ScheduleItemComputers_ScheduleItemComputerId",
                        column: x => x.ScheduleItemComputerId,
                        principalTable: "ScheduleItemComputers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ScheduleMaintenanceItemId = table.Column<Guid>(nullable: false),
                    MaintenanceDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_ScheduleMaintenanceItems_ScheduleMaintenanceItemId",
                        column: x => x.ScheduleMaintenanceItemId,
                        principalTable: "ScheduleMaintenanceItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItemComputers_ComputerId",
                table: "ScheduleItemComputers",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItemComputers_ItemId",
                table: "ScheduleItemComputers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleMaintenanceItems_MaintenanceId",
                table: "ScheduleMaintenanceItems",
                column: "MaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleMaintenanceItems_ScheduleItemComputerId",
                table: "ScheduleMaintenanceItems",
                column: "ScheduleItemComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ScheduleMaintenanceItemId",
                table: "Schedules",
                column: "ScheduleMaintenanceItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "ScheduleMaintenanceItems");

            migrationBuilder.DropTable(
                name: "ScheduleItemComputers");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "ItemComputers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
