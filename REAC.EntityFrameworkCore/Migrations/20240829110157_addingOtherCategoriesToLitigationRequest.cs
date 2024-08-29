using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addingOtherCategoriesToLitigationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OtherCategories",
                table: "LitigationRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_OtherCategories",
                table: "LitigationRequest",
                column: "OtherCategories");

            migrationBuilder.AddForeignKey(
                name: "FKOtherCategory580725",
                table: "LitigationRequest",
                column: "OtherCategories",
                principalTable: "OtherCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKOtherCategory580725",
                table: "LitigationRequest");

            migrationBuilder.DropIndex(
                name: "IX_LitigationRequest_OtherCategories",
                table: "LitigationRequest");

            migrationBuilder.DropColumn(
                name: "OtherCategories",
                table: "LitigationRequest");
        }
    }
}
