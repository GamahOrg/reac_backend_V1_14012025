using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLitigationRequestTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LitigationRequestTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LitigationRequestTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitigationRequestTypes", x => x.Id);
                });
        }
    }
}
