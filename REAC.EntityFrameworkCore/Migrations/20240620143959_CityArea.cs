using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class CityArea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "SysSetting",
                type: "nvarchar(225)",
                maxLength: 225,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<Guid>(
                name: "AreaId",
                table: "LitigationRequest",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "LitigationRequest",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ContractAddress",
                table: "LitigationRequest",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LitigationArea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitigationArea", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LitigationCity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitigationCity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LitigationCity_LitigationArea_AreaId",
                        column: x => x.AreaId,
                        principalTable: "LitigationArea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_AreaId",
                table: "LitigationRequest",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_CityId",
                table: "LitigationRequest",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationCity_AreaId",
                table: "LitigationCity",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationRequest_LitigationArea_AreaId",
                table: "LitigationRequest",
                column: "AreaId",
                principalTable: "LitigationArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationRequest_LitigationCity_CityId",
                table: "LitigationRequest",
                column: "CityId",
                principalTable: "LitigationCity",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LitigationRequest_LitigationArea_AreaId",
                table: "LitigationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_LitigationRequest_LitigationCity_CityId",
                table: "LitigationRequest");

            migrationBuilder.DropTable(
                name: "LitigationCity");

            migrationBuilder.DropTable(
                name: "LitigationArea");

            migrationBuilder.DropIndex(
                name: "IX_LitigationRequest_AreaId",
                table: "LitigationRequest");

            migrationBuilder.DropIndex(
                name: "IX_LitigationRequest_CityId",
                table: "LitigationRequest");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "LitigationRequest");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "LitigationRequest");

            migrationBuilder.DropColumn(
                name: "ContractAddress",
                table: "LitigationRequest");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "SysSetting",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(225)",
                oldMaxLength: 225);
        }
    }
}
