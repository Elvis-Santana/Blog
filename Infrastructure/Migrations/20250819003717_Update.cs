using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Authors_IdAuthor",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "IdAuthor",
                table: "Category",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_IdAuthor",
                table: "Category",
                newName: "IX_Category_AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Posts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Authors_AuthorId",
                table: "Category",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Authors_AuthorId",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Category",
                newName: "IdAuthor");

            migrationBuilder.RenameIndex(
                name: "IX_Category_AuthorId",
                table: "Category",
                newName: "IX_Category_IdAuthor");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Authors_IdAuthor",
                table: "Category",
                column: "IdAuthor",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
