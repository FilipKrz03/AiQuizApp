using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedAlgorithemsModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlgorithmTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdvanceNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlgorithmTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlgorithmAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlgorithmTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlgorithmAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlgorithmAnswers_AlgorithmTasks_AlgorithmTaskId",
                        column: x => x.AlgorithmTaskId,
                        principalTable: "AlgorithmTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlgorithmAnswers_AlgorithmTaskId",
                table: "AlgorithmAnswers",
                column: "AlgorithmTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlgorithmAnswers");

            migrationBuilder.DropTable(
                name: "AlgorithmTasks");
        }
    }
}
