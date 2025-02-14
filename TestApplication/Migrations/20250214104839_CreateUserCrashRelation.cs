using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestApplication.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserCrashRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Crash",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Crash_CreatedById",
                table: "Crash",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Crash_User_CreatedById",
                table: "Crash",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crash_User_CreatedById",
                table: "Crash");

            migrationBuilder.DropIndex(
                name: "IX_Crash_CreatedById",
                table: "Crash");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Crash");
        }
    }
}
