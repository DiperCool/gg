using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    public partial class TeamStatsPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Users_UserId",
                table: "PlayerStats");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "PlayerStats",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "PlayerStats",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_TeamId",
                table: "PlayerStats",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Teams_TeamId",
                table: "PlayerStats",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Users_UserId",
                table: "PlayerStats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Teams_TeamId",
                table: "PlayerStats");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Users_UserId",
                table: "PlayerStats");

            migrationBuilder.DropIndex(
                name: "IX_PlayerStats_TeamId",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "PlayerStats");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "PlayerStats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Users_UserId",
                table: "PlayerStats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
