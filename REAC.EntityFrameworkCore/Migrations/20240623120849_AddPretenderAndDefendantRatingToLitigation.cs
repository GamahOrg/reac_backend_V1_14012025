using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddPretenderAndDefendantRatingToLitigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LitigationCitys_LitigationAreas_AreaId",
                table: "LitigationCitys");

            migrationBuilder.AlterColumn<string>(
                name: "ContractAddress",
                table: "LitigationRequest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "CityId",
                table: "LitigationRequest",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaId",
                table: "LitigationRequest",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "DefendantRating",
                table: "LitigationRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PretenderRating",
                table: "LitigationRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationCitys_LitigationAreas_AreaId",
                table: "LitigationCitys",
                column: "AreaId",
                principalTable: "LitigationAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LitigationCitys_LitigationAreas_AreaId",
                table: "LitigationCitys");

            migrationBuilder.DropColumn(
                name: "DefendantRating",
                table: "LitigationRequest");

            migrationBuilder.DropColumn(
                name: "PretenderRating",
                table: "LitigationRequest");

            migrationBuilder.AlterColumn<string>(
                name: "ContractAddress",
                table: "LitigationRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CityId",
                table: "LitigationRequest",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaId",
                table: "LitigationRequest",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationCitys_LitigationAreas_AreaId",
                table: "LitigationCitys",
                column: "AreaId",
                principalTable: "LitigationAreas",
                principalColumn: "Id");
        }
    }
}
