using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseMigration2572024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LitigationRequestId",
                table: "WorkFlowProcessingLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowProcessingLog_LitigationRequestId",
                table: "WorkFlowProcessingLog",
                column: "LitigationRequestId");

            migrationBuilder.AddForeignKey(
                name: "FKWorkFlowPrWorkFlowProcessingLog231724",
                table: "WorkFlowProcessingLog",
                column: "LitigationRequestId",
                principalTable: "LitigationRequest",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKWorkFlowPrWorkFlowProcessingLog231724",
                table: "WorkFlowProcessingLog");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowProcessingLog_LitigationRequestId",
                table: "WorkFlowProcessingLog");

            migrationBuilder.DropColumn(
                name: "LitigationRequestId",
                table: "WorkFlowProcessingLog");
        }
    }
}
