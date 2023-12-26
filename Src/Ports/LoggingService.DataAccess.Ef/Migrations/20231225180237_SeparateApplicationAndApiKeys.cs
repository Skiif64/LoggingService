using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoggingService.DataAccess.Ef.Migrations
{
    /// <inheritdoc />
    public partial class SeparateApplicationAndApiKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey_ApiKeyHash",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ApiKey_ApiKeyPrefix",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ApiKey_ExpireAtUtc",
                table: "Applications");

            migrationBuilder.CreateTable(
                name: "ApiKey",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApiKeyPrefix = table.Column<string>(type: "text", nullable: false),
                    ApiKeyHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    ExpireAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKey", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_ApiKeyPrefix",
                table: "ApiKey",
                column: "ApiKeyPrefix",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_ApplicationId",
                table: "ApiKey",
                column: "ApplicationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKey");

            migrationBuilder.AddColumn<byte[]>(
                name: "ApiKey_ApiKeyHash",
                table: "Applications",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "ApiKey_ApiKeyPrefix",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApiKey_ExpireAtUtc",
                table: "Applications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
