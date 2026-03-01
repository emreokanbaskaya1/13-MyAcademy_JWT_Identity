using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _13_MyAcademy_JWT_Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddNewArtistsAndSongs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/bepop/assets/img/tarkan.jpg");

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/bepop/assets/img/sezenaksu.jpg");

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/bepop/assets/img/barışmanço.jpg");

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "/bepop/assets/img/teoman.jpg");

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "/bepop/assets/img/sertaperener.jpg");

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "/bepop/assets/img/hadise.jpg");

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Bio", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 9, "Arabesk müziğin efsanevi sesi", "/bepop/assets/img/müslümgürses.jpg", "Müslüm Gürses" },
                    { 10, "Türk rap müziğinin öncüsü", "/bepop/assets/img/ceza.jpg", "Ceza" },
                    { 11, "Türk rock müziğinin sevilen grubu", "/bepop/assets/img/duman.jpg", "Duman" },
                    { 12, "Türk hip-hop sahnesinin usta ismi", "/bepop/assets/img/sagopa.jpg", "Sagopa Kajmer" },
                    { 13, "Alternatif rock'ın öncü grubu", "/bepop/assets/img/morveötesi.jpg", "Mor ve Ötesi" },
                    { 14, "Türk pop müziğinin divası", "/bepop/assets/img/nil.jpg", "Nilüfer" },
                    { 15, "Pop müziğin sevilen sesi", "/bepop/assets/img/yalın.jpg", "Yalın" },
                    { 16, "Pop rock'ın sevilen grubu", "/bepop/assets/img/gripin.jpg", "Gripin" },
                    { 17, "Türk pop-rock'ın efsane üçlüsü", "/bepop/assets/img/b12.jpg", "MFÖ" },
                    { 18, "Nu-metal ve alternatif rock grubu", "/bepop/assets/img/b13.jpg", "Manga" }
                });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "ArtistId", "CoverImageUrl", "ReleaseYear", "Title" },
                values: new object[,]
                {
                    { 10, 9, "/bepop/assets/img/c8.jpg", 1988, "Kâğıt Helva" },
                    { 11, 10, "/bepop/assets/img/c9.jpg", 2004, "Rapstar" },
                    { 12, 11, "/bepop/assets/img/c10.jpg", 2020, "Darmaduman" },
                    { 13, 12, "/bepop/assets/img/c11.jpg", 2006, "Bir Pesimistin Gözyaşları" },
                    { 14, 13, "/bepop/assets/img/c12.jpg", 2004, "Dünya Yalan Söylüyor" },
                    { 15, 14, "/bepop/assets/img/c13.jpg", 1985, "Nilüfer" },
                    { 16, 15, "/bepop/assets/img/c14.jpg", 2008, "Ellerine Sağlık" },
                    { 17, 16, "/bepop/assets/img/c15.jpg", 2010, "M.S. 05.03.2010" },
                    { 18, 17, "/bepop/assets/img/c16.jpg", 1989, "Ele Güne Karşı" },
                    { 19, 18, "/bepop/assets/img/c17.jpg", 2009, "Şehr-i Hüzün" }
                });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "AlbumId", "ContentLevel", "DurationInSeconds", "FilePath", "Title" },
                values: new object[,]
                {
                    { 21, 10, 5, 310, "songs/kagithelva.mp3", "Kâğıt Helva" },
                    { 22, 10, 3, 285, "songs/affet.mp3", "Affet" },
                    { 23, 11, 2, 248, "songs/holocaust.mp3", "Holocaust" },
                    { 24, 11, 6, 230, "songs/neyimvarki.mp3", "Neyim Var Ki" },
                    { 25, 12, 4, 256, "songs/sendendahaguzel.mp3", "Senden Daha Güzel" },
                    { 26, 12, 6, 242, "songs/yurekten.mp3", "Yürekten" },
                    { 27, 13, 1, 268, "songs/baytar.mp3", "Baytar" },
                    { 28, 13, 5, 295, "songs/dusunki.mp3", "Düşün Ki" },
                    { 29, 14, 6, 218, "songs/cambaz.mp3", "Cambaz" },
                    { 30, 14, 3, 240, "songs/dunyayalan.mp3", "Dünya Yalan Söylüyor" },
                    { 31, 15, 4, 230, "songs/dunyadonuyor.mp3", "Dünya Dönüyor" },
                    { 32, 15, 6, 255, "songs/geceler.mp3", "Geceler" },
                    { 33, 16, 5, 220, "songs/ellerinesaglik.mp3", "Ellerine Sağlık" },
                    { 34, 16, 2, 238, "songs/asklaftan.mp3", "Aşk Laftan Anlamaz" },
                    { 35, 17, 6, 252, "songs/boylekahpe.mp3", "Böyle Kahpedir Dünya" },
                    { 36, 17, 3, 227, "songs/durmayagmur.mp3", "Durma Yağmur Durma" },
                    { 37, 18, 4, 210, "songs/elegunekarsi.mp3", "Ele Güne Karşı" },
                    { 38, 18, 6, 198, "songs/alidesidero.mp3", "Ali Desidero" },
                    { 39, 19, 1, 235, "songs/dunyaninsonu.mp3", "Dünyanın Sonuna Doğmuşum" },
                    { 40, 19, 5, 248, "songs/birkadin.mp3", "Bir Kadın Çizeceksin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/bepop/assets/img/b1.jpg");

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/bepop/assets/img/b2.jpg");

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/bepop/assets/img/b3.jpg");

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "/bepop/assets/img/b9.jpg");

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "/bepop/assets/img/b10.jpg");

            migrationBuilder.UpdateData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "/bepop/assets/img/b11.jpg");
        }
    }
}
