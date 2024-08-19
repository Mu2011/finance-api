using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stocks_StockId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_Stocks_StockId",
                table: "Portfolios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "526effb3-eed9-4d8e-8780-5fb207d992e1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d00d4e07-f8cf-4330-969a-cc955cbabc73");

            migrationBuilder.RenameTable(
                name: "Stocks",
                newName: "Tables");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tables",
                table: "Tables",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5872e32c-c8d3-490a-adab-dfd410c13b48", null, "Admin", "ADMIN" },
                    { "e17a79fa-4691-4739-9d28-3c0ef7e42321", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tables_StockId",
                table: "Comments",
                column: "StockId",
                principalTable: "Tables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_Tables_StockId",
                table: "Portfolios",
                column: "StockId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tables_StockId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_Tables_StockId",
                table: "Portfolios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tables",
                table: "Tables");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5872e32c-c8d3-490a-adab-dfd410c13b48");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e17a79fa-4691-4739-9d28-3c0ef7e42321");

            migrationBuilder.RenameTable(
                name: "Tables",
                newName: "Stocks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "526effb3-eed9-4d8e-8780-5fb207d992e1", null, "Admin", "ADMIN" },
                    { "d00d4e07-f8cf-4330-969a-cc955cbabc73", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stocks_StockId",
                table: "Comments",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_Stocks_StockId",
                table: "Portfolios",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
