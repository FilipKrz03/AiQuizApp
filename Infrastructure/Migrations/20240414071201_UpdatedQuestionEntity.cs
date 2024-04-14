using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedQuestionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_UserOwnQuizzes_UserOwnQuizId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_UserOwnQuizId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "UserOwnQuizId",
                table: "Questions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserOwnQuizId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserOwnQuizId",
                table: "Questions",
                column: "UserOwnQuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_UserOwnQuizzes_UserOwnQuizId",
                table: "Questions",
                column: "UserOwnQuizId",
                principalTable: "UserOwnQuizzes",
                principalColumn: "Id");
        }
    }
}
