using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addingsigntureFileField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SigntureFileId",
                table: "Citizen",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_SigntureFileId",
                table: "Citizen",
                column: "SigntureFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Citizen_SysFileManager_SigntureFileId",
                table: "Citizen",
                column: "SigntureFileId",
                principalTable: "SysFileManager",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citizen_SysFileManager_SigntureFileId",
                table: "Citizen");

            migrationBuilder.DropIndex(
                name: "IX_Citizen_SigntureFileId",
                table: "Citizen");

            migrationBuilder.DropColumn(
                name: "SigntureFileId",
                table: "Citizen");
        }
    }
}
