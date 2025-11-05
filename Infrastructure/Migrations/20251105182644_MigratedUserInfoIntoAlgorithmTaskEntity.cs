using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigratedUserInfoIntoAlgorithmTaskEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOwnAlgorithmTasks");

            migrationBuilder.AddColumn<int>(
                name: "CreationStatus",
                table: "AlgorithmTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AlgorithmTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlgorithmTasks_UserId",
                table: "AlgorithmTasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlgorithmTasks_AspNetUsers_UserId",
                table: "AlgorithmTasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlgorithmTasks_AspNetUsers_UserId",
                table: "AlgorithmTasks");

            migrationBuilder.DropIndex(
                name: "IX_AlgorithmTasks_UserId",
                table: "AlgorithmTasks");

            migrationBuilder.DropColumn(
                name: "CreationStatus",
                table: "AlgorithmTasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AlgorithmTasks");

            migrationBuilder.CreateTable(
                name: "UserOwnAlgorithmTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOwnAlgorithmTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOwnAlgorithmTasks_AlgorithmTasks_Id",
                        column: x => x.Id,
                        principalTable: "AlgorithmTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOwnAlgorithmTasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOwnAlgorithmTasks_UserId",
                table: "UserOwnAlgorithmTasks",
                column: "UserId");
        }
    }
}
