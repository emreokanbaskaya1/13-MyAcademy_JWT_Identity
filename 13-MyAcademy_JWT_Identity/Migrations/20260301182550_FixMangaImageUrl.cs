using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _13_MyAcademy_JWT_Identity.Migrations
{
    /// <inheritdoc />
    public partial class FixMangaImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 18,
                column: "ImageUrl",
                value: "/bepop/assets/img/maaaaang.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 18,
                column: "ImageUrl",
                value: "/bepop/assets/img/b13.jpg");
        }
    }
}
