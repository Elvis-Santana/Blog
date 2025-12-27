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
            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Followers",
                newName: "FollowingId");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_FollowingId",
                table: "Followers",
                column: "FollowingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Authors_FollowerId",
                table: "Followers",
                column: "FollowerId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Authors_FollowingId",
                table: "Followers",
                column: "FollowingId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Authors_FollowerId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Authors_FollowingId",
                table: "Followers");

            migrationBuilder.DropIndex(
                name: "IX_Followers_FollowingId",
                table: "Followers");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "Followers",
                newName: "AuthorId");
        }
    }
}
