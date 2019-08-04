using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerMaintenance.Migrations
{
    public partial class RenameColumnToActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActiActivity",
                table: "Maintenances",
                newName: "Activity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Activity",
                table: "Maintenances",
                newName: "ActiActivity");
        }
    }
}
