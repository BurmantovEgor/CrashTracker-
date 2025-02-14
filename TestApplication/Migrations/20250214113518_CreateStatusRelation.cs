using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestApplication.Migrations
{
    /// <inheritdoc />
    public partial class CreateStatusRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crash_Status_StatusId",
                table: "Crash");

            migrationBuilder.AddColumn<Guid>(
                name: "CrashStatusId",
                table: "Crash",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Crash_CrashStatusId",
                table: "Crash",
                column: "CrashStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crash_Status_CrashStatusId",
                table: "Crash",
                column: "CrashStatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Crash_Status_StatusId",
                table: "Crash",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crash_Status_CrashStatusId",
                table: "Crash");

            migrationBuilder.DropForeignKey(
                name: "FK_Crash_Status_StatusId",
                table: "Crash");

            migrationBuilder.DropIndex(
                name: "IX_Crash_CrashStatusId",
                table: "Crash");

            migrationBuilder.DropColumn(
                name: "CrashStatusId",
                table: "Crash");

            migrationBuilder.AddForeignKey(
                name: "FK_Crash_Status_StatusId",
                table: "Crash",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
