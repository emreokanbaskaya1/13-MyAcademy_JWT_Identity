using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _13_MyAcademy_JWT_Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddMusicPlatformEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationInSeconds = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentLevel = table.Column<int>(type: "int", nullable: false),
                    AlbumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSongHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: false),
                    PlayedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSongHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSongHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSongHistories_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Bio", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Türk pop müziğinin megastarı", "/bepop/assets/img/b1.jpg", "Tarkan" },
                    { 2, "Türk pop müziğinin minik serçesi", "/bepop/assets/img/b2.jpg", "Sezen Aksu" },
                    { 3, "Türk rock müziğinin efsanesi", "/bepop/assets/img/b3.jpg", "Barış Manço" }
                });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "ContentLevel", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Elite" },
                    { 2, 2, "Premium" },
                    { 3, 3, "Gold" },
                    { 4, 4, "Standard" },
                    { 5, 5, "Basic" },
                    { 6, 6, "Free" }
                });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "ArtistId", "CoverImageUrl", "ReleaseYear", "Title" },
                values: new object[,]
                {
                    { 1, 1, "/bepop/assets/img/b4.jpg", 2001, "Karma" },
                    { 2, 2, "/bepop/assets/img/b5.jpg", 1991, "Gülümse" },
                    { 3, 3, "/bepop/assets/img/b6.jpg", 1999, "Mançoloji" }
                });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "AlbumId", "ContentLevel", "DurationInSeconds", "FilePath", "Title" },
                values: new object[,]
                {
                    { 1, 1, 6, 234, "songs/simarik.mp3", "Şımarık" },
                    { 2, 1, 5, 258, "songs/kuzukuzu.mp3", "Kuzu Kuzu" },
                    { 3, 1, 4, 245, "songs/dudu.mp3", "Dudu" },
                    { 4, 2, 3, 300, "songs/gulumse.mp3", "Gülümse" },
                    { 5, 2, 2, 275, "songs/hadibakalim.mp3", "Hadi Bakalım" },
                    { 6, 3, 1, 312, "songs/gesibaglari.mp3", "Gesi Bağları" },
                    { 7, 3, 6, 290, "songs/donence.mp3", "Dönence" },
                    { 8, 3, 4, 267, "songs/allabeni.mp3", "Alla Beni Pulla Beni" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PackageId",
                table: "AspNetUsers",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistId",
                table: "Albums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_AlbumId",
                table: "Songs",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSongHistories_SongId",
                table: "UserSongHistories",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSongHistories_UserId",
                table: "UserSongHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Packages_PackageId",
                table: "AspNetUsers",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Packages_PackageId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "UserSongHistories");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PackageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "AspNetUsers");
        }
    }
}
