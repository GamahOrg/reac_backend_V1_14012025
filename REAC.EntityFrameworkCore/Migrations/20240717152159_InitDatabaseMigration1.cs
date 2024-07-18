using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabaseMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExemptionReductionFileId",
                table: "LitigationRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExemptionReduction",
                table: "LitigationRequest",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_ExemptionReductionFileId",
                table: "LitigationRequest",
                column: "ExemptionReductionFileId");

            migrationBuilder.AddForeignKey(
                name: "FKExemptionReductionCategoryFilesExemptionReductionFiles01176101",
                table: "LitigationRequest",
                column: "ExemptionReductionFileId",
                principalTable: "SysFileManager",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKExemptionReductionCategoryFilesExemptionReductionFiles01176101",
                table: "LitigationRequest");

            migrationBuilder.DropIndex(
                name: "IX_LitigationRequest_ExemptionReductionFileId",
                table: "LitigationRequest");

            migrationBuilder.DropColumn(
                name: "ExemptionReductionFileId",
                table: "LitigationRequest");

            migrationBuilder.DropColumn(
                name: "IsExemptionReduction",
                table: "LitigationRequest");
        }
    }
}
