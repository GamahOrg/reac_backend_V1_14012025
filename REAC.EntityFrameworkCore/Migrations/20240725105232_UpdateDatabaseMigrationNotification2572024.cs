using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseMigrationNotification2572024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysNotificationPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationPlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysNotificationPlan__C0743341C99", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysNotificationPlanItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SysNotificationPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    Template = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysNotificationPlanItem__0743341C99", x => x.Id);
                    table.ForeignKey(
                        name: "FKSysNotificationPlanItemsSysNotificationPlan76101",
                        column: x => x.SysNotificationPlanId,
                        principalTable: "SysNotificationPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SysNotificationPlanItemDestination",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SysNotificationPlanItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationDestinationType = table.Column<int>(type: "int", nullable: false),
                    Template = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysNotificationPlanItemDestination__0743341C99", x => x.Id);
                    table.ForeignKey(
                        name: "FKSysSysNotificationPlanItemDestinationsPlan7610001",
                        column: x => x.SysNotificationPlanItemId,
                        principalTable: "SysNotificationPlanItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SysNotificationPlanItem_SysNotificationPlanId",
                table: "SysNotificationPlanItem",
                column: "SysNotificationPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SysNotificationPlanItemDestination_SysNotificationPlanItemId",
                table: "SysNotificationPlanItemDestination",
                column: "SysNotificationPlanItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysNotificationPlanItemDestination");

            migrationBuilder.DropTable(
                name: "SysNotificationPlanItem");

            migrationBuilder.DropTable(
                name: "SysNotificationPlan");
        }
    }
}
