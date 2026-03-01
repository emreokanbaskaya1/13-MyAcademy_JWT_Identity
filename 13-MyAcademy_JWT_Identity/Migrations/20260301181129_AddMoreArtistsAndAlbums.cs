using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _13_MyAcademy_JWT_Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreArtistsAndAlbums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 1,
                column: "CoverImageUrl",
                value: "/bepop/assets/img/c0.jpg");

            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 2,
                column: "CoverImageUrl",
                value: "/bepop/assets/img/c1.jpg");

            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 3,
                column: "CoverImageUrl",
                value: "/bepop/assets/img/c2.jpg");

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "ArtistId", "CoverImageUrl", "ReleaseYear", "Title" },
                values: new object[] { 4, 1, "/bepop/assets/img/c3.jpg", 2007, "Metamorfoz" });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Bio", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 4, "Türk pop müziğinin süperstarı", "/bepop/assets/img/b7.jpg", "Ajda Pekkan" },
                    { 5, "Anadolu rock'ın öncüsü", "/bepop/assets/img/b8.jpg", "Cem Karaca" },
                    { 6, "Alternatif rock sanatçısı", "/bepop/assets/img/b9.jpg", "Teoman" },
                    { 7, "Eurovision şampiyonu", "/bepop/assets/img/b10.jpg", "Sertab Erener" },
                    { 8, "Pop ve R&B sanatçısı", "/bepop/assets/img/b11.jpg", "Hadise" }
                });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "ArtistId", "CoverImageUrl", "ReleaseYear", "Title" },
                values: new object[,]
                {
                    { 5, 4, "/bepop/assets/img/c4.jpg", 1977, "Superstar" },
                    { 6, 5, "/bepop/assets/img/c5.jpg", 1977, "Resimdeki Gözyaşları" },
                    { 7, 6, "/bepop/assets/img/c6.jpg", 2001, "Onyedi" },
                    { 8, 7, "/bepop/assets/img/c7.jpg", 2003, "Every Way That I Can" },
                    { 9, 8, "/bepop/assets/img/c20.jpg", 2008, "Hadise" }
                });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "AlbumId", "ContentLevel", "DurationInSeconds", "FilePath", "Title" },
                values: new object[,]
                {
                    { 9, 4, 5, 215, "songs/bounce.mp3", "Bounce" },
                    { 10, 4, 4, 225, "songs/dedikodu.mp3", "Dedikodu" },
                    { 11, 5, 3, 198, "songs/superstar.mp3", "Superstar" },
                    { 12, 5, 6, 210, "songs/bambaska.mp3", "Bambaşka Biri" },
                    { 13, 6, 2, 245, "songs/tamirci.mp3", "Tamirci Çırağı" },
                    { 14, 6, 1, 280, "songs/resimdeki.mp3", "Resimdeki Gözyaşları" },
                    { 15, 7, 4, 235, "songs/istanbul.mp3", "İstanbul'da Sonbahar" },
                    { 16, 7, 5, 205, "songs/papatya.mp3", "Papatya" },
                    { 17, 8, 3, 180, "songs/everyway.mp3", "Every Way That I Can" },
                    { 18, 8, 6, 220, "songs/leave.mp3", "Leave" },
                    { 19, 9, 6, 178, "songs/dumtek.mp3", "Düm Tek Tek" },
                    { 20, 9, 4, 195, "songs/superman.mp3", "Superman" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 1,
                column: "CoverImageUrl",
                value: "/bepop/assets/img/b4.jpg");

            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 2,
                column: "CoverImageUrl",
                value: "/bepop/assets/img/b5.jpg");

            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 3,
                column: "CoverImageUrl",
                value: "/bepop/assets/img/b6.jpg");
        }
    }
}
