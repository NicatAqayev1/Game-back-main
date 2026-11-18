using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class Salam1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClickLogs_Casinos_CasinoId",
                table: "ClickLogs");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Casinos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_ClickLogs_Casinos_CasinoId",
                table: "ClickLogs",
                column: "CasinoId",
                principalTable: "Casinos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClickLogs_Casinos_CasinoId",
                table: "ClickLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Casinos");

            migrationBuilder.AddForeignKey(
                name: "FK_ClickLogs_Casinos_CasinoId",
                table: "ClickLogs",
                column: "CasinoId",
                principalTable: "Casinos",
                principalColumn: "Id");
        }
    }
}
