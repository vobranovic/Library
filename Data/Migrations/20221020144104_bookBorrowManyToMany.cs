using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    public partial class bookBorrowManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Borrows_BorrowId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BorrowId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BorrowId",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "BookBorrow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BorrowId = table.Column<int>(type: "int", nullable: false),
                    BookdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookBorrow_Books_BookdId",
                        column: x => x.BookdId,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookBorrow_Borrows_BorrowId",
                        column: x => x.BorrowId,
                        principalTable: "Borrows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrow_BookdId",
                table: "BookBorrow",
                column: "BookdId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrow_BorrowId",
                table: "BookBorrow",
                column: "BorrowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBorrow");

            migrationBuilder.AddColumn<int>(
                name: "BorrowId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BorrowId",
                table: "Books",
                column: "BorrowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Borrows_BorrowId",
                table: "Books",
                column: "BorrowId",
                principalTable: "Borrows",
                principalColumn: "Id");
        }
    }
}
