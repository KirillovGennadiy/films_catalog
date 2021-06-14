using Microsoft.EntityFrameworkCore.Migrations;

namespace FilmsCatalog.Migrations
{
    public partial class relation_with_files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Files_PosterId",
                table: "Films");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Files_PosterId",
                table: "Films",
                column: "PosterId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Files_PosterId",
                table: "Films");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Files_PosterId",
                table: "Films",
                column: "PosterId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
