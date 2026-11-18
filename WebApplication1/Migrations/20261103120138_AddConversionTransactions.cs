using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddConversionTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostbackLogs_CasinoId_TransactionId",
                table: "PostbackLogs");

            migrationBuilder.DropIndex(
                name: "IX_PostbackLogs_TransactionId_CasinoId",
                table: "PostbackLogs");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ClickLogs",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateTable(
                name: "ConversionTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClickLogId = table.Column<long>(type: "bigint", nullable: true),
                    CasinoId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RelatedTransactionId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RelatedConversionTransactionId = table.Column<long>(type: "bigint", nullable: true),
                    ExternalPlayerId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CommissionAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DepositCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CommissionCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversionTransactions_Casinos_CasinoId",
                        column: x => x.CasinoId,
                        principalTable: "Casinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConversionTransactions_ClickLogs_ClickLogId",
                        column: x => x.ClickLogId,
                        principalTable: "ClickLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConversionTransactions_ConversionTransactions_RelatedConversionTransactionId",
                        column: x => x.RelatedConversionTransactionId,
                        principalTable: "ConversionTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostbackLogs_CasinoId_TransactionId",
                table: "PostbackLogs",
                columns: new[] { "CasinoId", "TransactionId" },
                unique: true,
                filter: "[IsSuccessful] = 1 AND [TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionTransactions_CasinoId_TransactionId",
                table: "ConversionTransactions",
                columns: new[] { "CasinoId", "TransactionId" },
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionTransactions_ClickLogId",
                table: "ConversionTransactions",
                column: "ClickLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionTransactions_RelatedConversionTransactionId",
                table: "ConversionTransactions",
                column: "RelatedConversionTransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversionTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PostbackLogs_CasinoId_TransactionId",
                table: "PostbackLogs");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ClickLogs");

            migrationBuilder.CreateIndex(
                name: "IX_PostbackLogs_CasinoId_TransactionId",
                table: "PostbackLogs",
                columns: new[] { "CasinoId", "TransactionId" },
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PostbackLogs_TransactionId_CasinoId",
                table: "PostbackLogs",
                columns: new[] { "TransactionId", "CasinoId" },
                unique: true,
                filter: "[IsSuccessful] = 1 AND [TransactionId] IS NOT NULL");
        }
    }
}
