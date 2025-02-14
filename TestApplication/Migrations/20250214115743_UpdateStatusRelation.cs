using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestApplication.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStatusRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crash_Status_StatusId",
                table: "Crash");

            migrationBuilder.DropIndex(
                name: "IX_Crash_StatusId",
                table: "Crash");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Crash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Crash",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Crash_StatusId",
                table: "Crash",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crash_Status_StatusId",
                table: "Crash",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
