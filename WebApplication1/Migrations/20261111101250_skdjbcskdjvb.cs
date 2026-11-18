using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class skdjbcskdjvb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostbackLogs_CasinoId_TransactionId",
                table: "PostbackLogs");

            migrationBuilder.AlterColumn<string>(
                name: "PostbackType",
                table: "PostbackLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<long>(
                name: "ReversedPostbackLogId",
                table: "PostbackLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostbackLogs_CasinoId_TransactionId_PostbackType",
                table: "PostbackLogs",
                columns: new[] { "CasinoId", "TransactionId", "PostbackType" },
                unique: true,
                filter: "[IsSuccessful] = 1 AND [TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PostbackLogs_ReversedPostbackLogId",
                table: "PostbackLogs",
                column: "ReversedPostbackLogId",
                unique: true,
                filter: "[ReversedPostbackLogId] IS NOT NULL AND [IsSuccessful] = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_PostbackLogs_PostbackLogs_ReversedPostbackLogId",
                table: "PostbackLogs",
                column: "ReversedPostbackLogId",
                principalTable: "PostbackLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostbackLogs_PostbackLogs_ReversedPostbackLogId",
                table: "PostbackLogs");

            migrationBuilder.DropIndex(
                name: "IX_PostbackLogs_CasinoId_TransactionId_PostbackType",
                table: "PostbackLogs");

            migrationBuilder.DropIndex(
                name: "IX_PostbackLogs_ReversedPostbackLogId",
                table: "PostbackLogs");

            migrationBuilder.DropColumn(
                name: "ReversedPostbackLogId",
                table: "PostbackLogs");

            migrationBuilder.AlterColumn<string>(
                name: "PostbackType",
                table: "PostbackLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_PostbackLogs_CasinoId_TransactionId",
                table: "PostbackLogs",
                columns: new[] { "CasinoId", "TransactionId" },
                unique: true,
                filter: "[IsSuccessful] = 1 AND [TransactionId] IS NOT NULL");
        }
    }
}
