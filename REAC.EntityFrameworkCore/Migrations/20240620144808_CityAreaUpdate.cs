using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class CityAreaUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LitigationCity_LitigationArea_AreaId",
                table: "LitigationCity");

            migrationBuilder.DropForeignKey(
                name: "FK_LitigationRequest_LitigationArea_AreaId",
                table: "LitigationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_LitigationRequest_LitigationCity_CityId",
                table: "LitigationRequest");

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationCity_LitigationArea_AreaId",
                table: "LitigationCity",
                column: "AreaId",
                principalTable: "LitigationArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

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
                name: "FK_LitigationCity_LitigationArea_AreaId",
                table: "LitigationCity");

            migrationBuilder.DropForeignKey(
                name: "FK_LitigationRequest_LitigationArea_AreaId",
                table: "LitigationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_LitigationRequest_LitigationCity_CityId",
                table: "LitigationRequest");

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationCity_LitigationArea_AreaId",
                table: "LitigationCity",
                column: "AreaId",
                principalTable: "LitigationArea",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationRequest_LitigationArea_AreaId",
                table: "LitigationRequest",
                column: "AreaId",
                principalTable: "LitigationArea",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationRequest_LitigationCity_CityId",
                table: "LitigationRequest",
                column: "CityId",
                principalTable: "LitigationCity",
                principalColumn: "Id");
        }
    }
}
