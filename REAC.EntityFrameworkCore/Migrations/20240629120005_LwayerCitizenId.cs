using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class LwayerCitizenId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CitizenId",
                table: "LitigationRequestLawyerAgents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestLawyerAgents_CitizenId",
                table: "LitigationRequestLawyerAgents",
                column: "CitizenId");

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationRequestLawyerAgents_Citizen_CitizenId",
                table: "LitigationRequestLawyerAgents",
                column: "CitizenId",
                principalTable: "Citizen",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LitigationRequestLawyerAgents_Citizen_CitizenId",
                table: "LitigationRequestLawyerAgents");

            migrationBuilder.DropIndex(
                name: "IX_LitigationRequestLawyerAgents_CitizenId",
                table: "LitigationRequestLawyerAgents");

            migrationBuilder.DropColumn(
                name: "CitizenId",
                table: "LitigationRequestLawyerAgents");
        }
    }
}
