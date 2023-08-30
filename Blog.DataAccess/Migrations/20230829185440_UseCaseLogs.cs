using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.DataAccess.Migrations
{
    public partial class UseCaseLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "UseCaseLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UseCaseName = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAuthorized = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseCaseLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UseCaseLogs_Id",
                table: "UseCaseLogs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UseCaseLogs_UseCaseName",
                table: "UseCaseLogs",
                column: "UseCaseName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UseCaseLogs");

            migrationBuilder.AlterColumn<int>(
                name: "FileId",
                table: "Likes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 3);
        }
    }
}
