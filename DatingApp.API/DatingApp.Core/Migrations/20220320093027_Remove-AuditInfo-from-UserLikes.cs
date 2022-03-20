using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Core.Migrations
{
    public partial class RemoveAuditInfofromUserLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikes_AuditInfoes_AuditInfoId",
                table: "UserLikes");

            migrationBuilder.DropIndex(
                name: "IX_UserLikes_AuditInfoId",
                table: "UserLikes");

            migrationBuilder.DropColumn(
                name: "AuditInfoId",
                table: "UserLikes");

            migrationBuilder.DropColumn(
                name: "UserLikeId",
                table: "UserLikes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditInfoId",
                table: "UserLikes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserLikeId",
                table: "UserLikes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserLikes_AuditInfoId",
                table: "UserLikes",
                column: "AuditInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikes_AuditInfoes_AuditInfoId",
                table: "UserLikes",
                column: "AuditInfoId",
                principalTable: "AuditInfoes",
                principalColumn: "AuditInfoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
