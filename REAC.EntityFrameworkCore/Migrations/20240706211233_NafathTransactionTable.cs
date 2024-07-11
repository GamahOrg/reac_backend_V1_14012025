using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class NafathTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NafathTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestExpired = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NafathTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherCategories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NafathTransactions");

            migrationBuilder.DropTable(
                name: "OtherCategories");
        }
    }
}
