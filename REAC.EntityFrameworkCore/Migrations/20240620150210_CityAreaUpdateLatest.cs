using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class CityAreaUpdateLatest : Migration
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_LitigationCity",
                table: "LitigationCity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LitigationArea",
                table: "LitigationArea");

            migrationBuilder.RenameTable(
                name: "LitigationCity",
                newName: "LitigationCitys");

            migrationBuilder.RenameTable(
                name: "LitigationArea",
                newName: "LitigationAreas");

            migrationBuilder.RenameIndex(
                name: "IX_LitigationCity_AreaId",
                table: "LitigationCitys",
                newName: "IX_LitigationCitys_AreaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LitigationCitys",
                table: "LitigationCitys",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LitigationAreas",
                table: "LitigationAreas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationCitys_LitigationAreas_AreaId",
                table: "LitigationCitys",
                column: "AreaId",
                principalTable: "LitigationAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationRequest_LitigationAreas_AreaId",
                table: "LitigationRequest",
                column: "AreaId",
                principalTable: "LitigationAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LitigationRequest_LitigationCitys_CityId",
                table: "LitigationRequest",
                column: "CityId",
                principalTable: "LitigationCitys",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LitigationCitys_LitigationAreas_AreaId",
                table: "LitigationCitys");

            migrationBuilder.DropForeignKey(
                name: "FK_LitigationRequest_LitigationAreas_AreaId",
                table: "LitigationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_LitigationRequest_LitigationCitys_CityId",
                table: "LitigationRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LitigationCitys",
                table: "LitigationCitys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LitigationAreas",
                table: "LitigationAreas");

            migrationBuilder.RenameTable(
                name: "LitigationCitys",
                newName: "LitigationCity");

            migrationBuilder.RenameTable(
                name: "LitigationAreas",
                newName: "LitigationArea");

            migrationBuilder.RenameIndex(
                name: "IX_LitigationCitys_AreaId",
                table: "LitigationCity",
                newName: "IX_LitigationCity_AreaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LitigationCity",
                table: "LitigationCity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LitigationArea",
                table: "LitigationArea",
                column: "Id");

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
