using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerMaintenance.Migrations
{
    public partial class VoltandoDataRegistro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "ScheduleItemComputers");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "ItemComputers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "ItemComputers");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "ScheduleItemComputers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
