using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slogan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Priorities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationNotificationTypeMaps",
                columns: table => new
                {
                    ClientApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationNotificationTypeMaps", x => new { x.ClientApplicationId, x.NotificationTypeId });
                    table.ForeignKey(
                        name: "FK_ApplicationNotificationTypeMaps_ClientApplications_ClientApplicationId",
                        column: x => x.ClientApplicationId,
                        principalTable: "ClientApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationNotificationTypeMaps_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceivedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SeenTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Secret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_ClientApplications_ClientApplicationId",
                        column: x => x.ClientApplicationId,
                        principalTable: "ClientApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Priorities_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "Priorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationHistories_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationNotificationTypeMaps_NotificationTypeId",
                table: "ApplicationNotificationTypeMaps",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationHistories_NotificationId",
                table: "NotificationHistories",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ClientApplicationId",
                table: "Notifications",
                column: "ClientApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PriorityId",
                table: "Notifications",
                column: "PriorityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationNotificationTypeMaps");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "NotificationHistories");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ClientApplications");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "Priorities");
        }
    }
}
