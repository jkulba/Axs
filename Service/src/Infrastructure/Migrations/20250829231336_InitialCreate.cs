using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessRequest",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RequestCode = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    JobNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    CycleNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivityCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ApplicationName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Workstation = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UtcCreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessRequest", x => x.RequestId);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivityCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ActivityName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GroupOwner = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    AuthorizationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ActivityId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAuthorized = table.Column<bool>(type: "INTEGER", nullable: false),
                    UtcCreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedByNum = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UtcUpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedByNum = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.AuthorizationId);
                    table.ForeignKey(
                        name: "FK_Authorizations_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupAuthorizations",
                columns: table => new
                {
                    AuthorizationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivityId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAuthorized = table.Column<bool>(type: "INTEGER", nullable: false),
                    UtcCreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedByNum = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UtcUpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedByNum = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAuthorizations", x => x.AuthorizationId);
                    table.ForeignKey(
                        name: "FK_GroupAuthorizations_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupAuthorizations_UserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "UserGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupMembers",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupMembers", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserGroupMembers_UserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "UserGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequest_JobNumber",
                table: "AccessRequest",
                column: "JobNumber");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequest_RequestCode",
                table: "AccessRequest",
                column: "RequestCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequest_UserName",
                table: "AccessRequest",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityCode",
                table: "Activities",
                column: "ActivityCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_ActivityId",
                table: "Authorizations",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_JobNumber_UserId_ActivityId",
                table: "Authorizations",
                columns: new[] { "JobNumber", "UserId", "ActivityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupAuthorizations_ActivityId",
                table: "GroupAuthorizations",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAuthorizations_GroupId",
                table: "GroupAuthorizations",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAuthorizations_JobNumber_GroupId_ActivityId",
                table: "GroupAuthorizations",
                columns: new[] { "JobNumber", "GroupId", "ActivityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupName",
                table: "UserGroups",
                column: "GroupName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessRequest");

            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropTable(
                name: "GroupAuthorizations");

            migrationBuilder.DropTable(
                name: "UserGroupMembers");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "UserGroups");
        }
    }
}
