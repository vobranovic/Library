using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    public partial class borrowId_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Borrows_BorrowId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "BorrowId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Borrows_BorrowId",
                table: "Books",
                column: "BorrowId",
                principalTable: "Borrows",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Borrows_BorrowId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "BorrowId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Borrows_BorrowId",
                table: "Books",
                column: "BorrowId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
