using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GraphId = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    PrincipalName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UtcCreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedByNum = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UtcUpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedByNum = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_UserId", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupMembers_UserId",
                table: "UserGroupMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_UserId",
                table: "Authorizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GraphId",
                table: "Users",
                column: "GraphId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PrincipalName",
                table: "Users",
                column: "PrincipalName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_Users_UserId",
                table: "Authorizations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupMembers_Users_UserId",
                table: "UserGroupMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_Users_UserId",
                table: "Authorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupMembers_Users_UserId",
                table: "UserGroupMembers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserGroupMembers_UserId",
                table: "UserGroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_Authorizations_UserId",
                table: "Authorizations");
        }
    }
}
