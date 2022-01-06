using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Core.Migrations
{
    public partial class AddAuditInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditInfoId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AuditInfoes",
                columns: table => new
                {
                    AuditInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryKey = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditInfoes", x => x.AuditInfoId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditInfoId",
                table: "Users",
                column: "AuditInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuditInfoes_AuditInfoId",
                table: "Users",
                column: "AuditInfoId",
                principalTable: "AuditInfoes",
                principalColumn: "AuditInfoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuditInfoes_AuditInfoId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AuditInfoes");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuditInfoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuditInfoId",
                table: "Users");
        }
    }
}
