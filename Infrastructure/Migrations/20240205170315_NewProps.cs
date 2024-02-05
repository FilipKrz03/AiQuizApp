using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProperAnswerNumber",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "ProperAnswerLetter",
                table: "Questions",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnswerLetter",
                table: "Answers",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProperAnswerLetter",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AnswerLetter",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "ProperAnswerNumber",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
