using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShopManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyTurnovers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecordDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CashAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CardAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ECommerceAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTurnovers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CurrentQuantity = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CriticalLimit = table.Column<decimal>(type: "numeric(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Stocks",
                columns: new[] { "Id", "CriticalLimit", "CurrentQuantity", "ItemName", "Unit" },
                values: new object[,]
                {
                    { 1, 3m, 10m, "Çilek", "Kg" },
                    { 2, 2m, 8m, "Muz", "Kg" },
                    { 3, 2m, 5m, "Waffle Hamuru", "Kg" },
                    { 4, 2m, 6m, "Dondurma (Çikolata)", "Litre" },
                    { 5, 2m, 4m, "Dondurma (Vanilya)", "Litre" },
                    { 6, 1m, 2m, "Çikolata Sos", "Litre" },
                    { 7, 50m, 200m, "Kağıt Kap (M)", "Adet" },
                    { 8, 50m, 150m, "Kağıt Kap (L)", "Adet" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyTurnovers");

            migrationBuilder.DropTable(
                name: "Stocks");
        }
    }
}
