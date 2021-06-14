using Microsoft.EntityFrameworkCore.Migrations;

namespace FilmsCatalog.Migrations
{
    public partial class films_creator_id_string : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_AspNetUsers_CreatorId1",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_CreatorId1",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "CreatorId1",
                table: "Films");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Films",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Films_CreatorId",
                table: "Films",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_AspNetUsers_CreatorId",
                table: "Films",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_AspNetUsers_CreatorId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_CreatorId",
                table: "Films");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId1",
                table: "Films",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Films_CreatorId1",
                table: "Films",
                column: "CreatorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_AspNetUsers_CreatorId1",
                table: "Films",
                column: "CreatorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
