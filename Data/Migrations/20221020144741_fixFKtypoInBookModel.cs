using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    public partial class fixFKtypoInBookModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrow_Books_BookdId",
                table: "BookBorrow");

            migrationBuilder.DropIndex(
                name: "IX_BookBorrow_BookdId",
                table: "BookBorrow");

            migrationBuilder.DropColumn(
                name: "BookdId",
                table: "BookBorrow");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrow_BookId",
                table: "BookBorrow",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrow_Books_BookId",
                table: "BookBorrow",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrow_Books_BookId",
                table: "BookBorrow");

            migrationBuilder.DropIndex(
                name: "IX_BookBorrow_BookId",
                table: "BookBorrow");

            migrationBuilder.AddColumn<int>(
                name: "BookdId",
                table: "BookBorrow",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrow_BookdId",
                table: "BookBorrow",
                column: "BookdId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrow_Books_BookdId",
                table: "BookBorrow",
                column: "BookdId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
