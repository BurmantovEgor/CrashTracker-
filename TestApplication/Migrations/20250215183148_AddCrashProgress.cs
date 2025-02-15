using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddCrashProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Operation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Progress",
                table: "Crash",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Operation");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Crash");
        }
    }
}
