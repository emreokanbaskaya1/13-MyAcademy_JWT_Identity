using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using _13_MyAcademy_JWT_Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace _13_MyAcademy_JWT_Identity.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<UserSongHistory> UserSongHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // AppUser - Package ilişkisi
            builder.Entity<AppUser>()
                .HasOne(u => u.Package)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.PackageId)
                .OnDelete(DeleteBehavior.SetNull);

            // Artist - Album ilişkisi
            builder.Entity<Album>()
                .HasOne(a => a.Artist)
                .WithMany(ar => ar.Albums)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);

            // Album - Song ilişkisi
            builder.Entity<Song>()
                .HasOne(s => s.Album)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserSongHistory ilişkileri
            builder.Entity<UserSongHistory>()
                .HasOne(h => h.User)
                .WithMany(u => u.UserSongHistories)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserSongHistory>()
                .HasOne(h => h.Song)
                .WithMany(s => s.UserSongHistories)
                .HasForeignKey(h => h.SongId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Data - 6 Paket
            builder.Entity<Package>().HasData(
                new Package { Id = 1, Name = "Elite", ContentLevel = 1 },
                new Package { Id = 2, Name = "Premium", ContentLevel = 2 },
                new Package { Id = 3, Name = "Gold", ContentLevel = 3 },
                new Package { Id = 4, Name = "Standard", ContentLevel = 4 },
                new Package { Id = 5, Name = "Basic", ContentLevel = 5 },
                new Package { Id = 6, Name = "Free", ContentLevel = 6 }
            );

            // Seed Data - Örnek Sanatçılar
            builder.Entity<Artist>().HasData(
                new Artist { Id = 1, Name = "Tarkan", Bio = "Türk pop müziğinin megastarı", ImageUrl = "/bepop/assets/img/tarkan.jpg" },
                new Artist { Id = 2, Name = "Sezen Aksu", Bio = "Türk pop müziğinin minik serçesi", ImageUrl = "/bepop/assets/img/sezenaksu.jpg" },
                new Artist { Id = 3, Name = "Barış Manço", Bio = "Türk rock müziğinin efsanesi", ImageUrl = "/bepop/assets/img/barışmanço.jpg" },
                new Artist { Id = 4, Name = "Ajda Pekkan", Bio = "Türk pop müziğinin süperstarı", ImageUrl = "/bepop/assets/img/b7.jpg" },
                new Artist { Id = 5, Name = "Cem Karaca", Bio = "Anadolu rock'ın öncüsü", ImageUrl = "/bepop/assets/img/b8.jpg" },
                new Artist { Id = 6, Name = "Teoman", Bio = "Alternatif rock sanatçısı", ImageUrl = "/bepop/assets/img/teoman.jpg" },
                new Artist { Id = 7, Name = "Sertab Erener", Bio = "Eurovision şampiyonu", ImageUrl = "/bepop/assets/img/sertaperener.jpg" },
                new Artist { Id = 8, Name = "Hadise", Bio = "Pop ve R&B sanatçısı", ImageUrl = "/bepop/assets/img/hadise.jpg" },
                new Artist { Id = 9, Name = "Müslüm Gürses", Bio = "Arabesk müziğin efsanevi sesi", ImageUrl = "/bepop/assets/img/müslümgürses.jpg" },
                new Artist { Id = 10, Name = "Ceza", Bio = "Türk rap müziğinin öncüsü", ImageUrl = "/bepop/assets/img/ceza.jpg" },
                new Artist { Id = 11, Name = "Duman", Bio = "Türk rock müziğinin sevilen grubu", ImageUrl = "/bepop/assets/img/duman.jpg" },
                new Artist { Id = 12, Name = "Sagopa Kajmer", Bio = "Türk hip-hop sahnesinin usta ismi", ImageUrl = "/bepop/assets/img/sagopa.jpg" },
                new Artist { Id = 13, Name = "Mor ve Ötesi", Bio = "Alternatif rock'ın öncü grubu", ImageUrl = "/bepop/assets/img/morveötesi.jpg" },
                new Artist { Id = 14, Name = "Nilüfer", Bio = "Türk pop müziğinin divası", ImageUrl = "/bepop/assets/img/nil.jpg" },
                new Artist { Id = 15, Name = "Yalın", Bio = "Pop müziğin sevilen sesi", ImageUrl = "/bepop/assets/img/yalın.jpg" },
                new Artist { Id = 16, Name = "Gripin", Bio = "Pop rock'ın sevilen grubu", ImageUrl = "/bepop/assets/img/gripin.jpg" },
                new Artist { Id = 17, Name = "MFÖ", Bio = "Türk pop-rock'ın efsane üçlüsü", ImageUrl = "/bepop/assets/img/b12.jpg" },
                new Artist { Id = 18, Name = "Manga", Bio = "Nu-metal ve alternatif rock grubu", ImageUrl = "/bepop/assets/img/maaaaang.jpg" }
            );

            // Seed Data - Örnek Albümler
            builder.Entity<Album>().HasData(
                new Album { Id = 1, Title = "Karma", CoverImageUrl = "/bepop/assets/img/c0.jpg", ReleaseYear = 2001, ArtistId = 1 },
                new Album { Id = 2, Title = "Gülümse", CoverImageUrl = "/bepop/assets/img/c1.jpg", ReleaseYear = 1991, ArtistId = 2 },
                new Album { Id = 3, Title = "Mançoloji", CoverImageUrl = "/bepop/assets/img/c2.jpg", ReleaseYear = 1999, ArtistId = 3 },
                new Album { Id = 4, Title = "Metamorfoz", CoverImageUrl = "/bepop/assets/img/c3.jpg", ReleaseYear = 2007, ArtistId = 1 },
                new Album { Id = 5, Title = "Superstar", CoverImageUrl = "/bepop/assets/img/c4.jpg", ReleaseYear = 1977, ArtistId = 4 },
                new Album { Id = 6, Title = "Resimdeki Gözyaşları", CoverImageUrl = "/bepop/assets/img/c5.jpg", ReleaseYear = 1977, ArtistId = 5 },
                new Album { Id = 7, Title = "Onyedi", CoverImageUrl = "/bepop/assets/img/c6.jpg", ReleaseYear = 2001, ArtistId = 6 },
                new Album { Id = 8, Title = "Every Way That I Can", CoverImageUrl = "/bepop/assets/img/c7.jpg", ReleaseYear = 2003, ArtistId = 7 },
                new Album { Id = 9, Title = "Hadise", CoverImageUrl = "/bepop/assets/img/c20.jpg", ReleaseYear = 2008, ArtistId = 8 },
                new Album { Id = 10, Title = "Kâğıt Helva", CoverImageUrl = "/bepop/assets/img/c8.jpg", ReleaseYear = 1988, ArtistId = 9 },
                new Album { Id = 11, Title = "Rapstar", CoverImageUrl = "/bepop/assets/img/c9.jpg", ReleaseYear = 2004, ArtistId = 10 },
                new Album { Id = 12, Title = "Darmaduman", CoverImageUrl = "/bepop/assets/img/c10.jpg", ReleaseYear = 2020, ArtistId = 11 },
                new Album { Id = 13, Title = "Bir Pesimistin Gözyaşları", CoverImageUrl = "/bepop/assets/img/c11.jpg", ReleaseYear = 2006, ArtistId = 12 },
                new Album { Id = 14, Title = "Dünya Yalan Söylüyor", CoverImageUrl = "/bepop/assets/img/c12.jpg", ReleaseYear = 2004, ArtistId = 13 },
                new Album { Id = 15, Title = "Nilüfer", CoverImageUrl = "/bepop/assets/img/c13.jpg", ReleaseYear = 1985, ArtistId = 14 },
                new Album { Id = 16, Title = "Ellerine Sağlık", CoverImageUrl = "/bepop/assets/img/c14.jpg", ReleaseYear = 2008, ArtistId = 15 },
                new Album { Id = 17, Title = "M.S. 05.03.2010", CoverImageUrl = "/bepop/assets/img/c15.jpg", ReleaseYear = 2010, ArtistId = 16 },
                new Album { Id = 18, Title = "Ele Güne Karşı", CoverImageUrl = "/bepop/assets/img/c16.jpg", ReleaseYear = 1989, ArtistId = 17 },
                new Album { Id = 19, Title = "Şehr-i Hüzün", CoverImageUrl = "/bepop/assets/img/c17.jpg", ReleaseYear = 2009, ArtistId = 18 }
            );

            // Seed Data - Örnek Şarkılar (farklı ContentLevel'lar)
            builder.Entity<Song>().HasData(
                // Tarkan - Karma
                new Song { Id = 1, Title = "Şımarık", DurationInSeconds = 234, FilePath = "songs/simarik.mp3", ContentLevel = 6, AlbumId = 1 },
                new Song { Id = 2, Title = "Kuzu Kuzu", DurationInSeconds = 258, FilePath = "songs/kuzukuzu.mp3", ContentLevel = 5, AlbumId = 1 },
                new Song { Id = 3, Title = "Dudu", DurationInSeconds = 245, FilePath = "songs/dudu.mp3", ContentLevel = 4, AlbumId = 1 },
                // Sezen Aksu - Gülümse
                new Song { Id = 4, Title = "Gülümse", DurationInSeconds = 300, FilePath = "songs/gulumse.mp3", ContentLevel = 3, AlbumId = 2 },
                new Song { Id = 5, Title = "Hadi Bakalım", DurationInSeconds = 275, FilePath = "songs/hadibakalim.mp3", ContentLevel = 2, AlbumId = 2 },
                // Barış Manço - Mançoloji
                new Song { Id = 6, Title = "Gesi Bağları", DurationInSeconds = 312, FilePath = "songs/gesibaglari.mp3", ContentLevel = 1, AlbumId = 3 },
                new Song { Id = 7, Title = "Dönence", DurationInSeconds = 290, FilePath = "songs/donence.mp3", ContentLevel = 6, AlbumId = 3 },
                new Song { Id = 8, Title = "Alla Beni Pulla Beni", DurationInSeconds = 267, FilePath = "songs/allabeni.mp3", ContentLevel = 4, AlbumId = 3 },
                // Tarkan - Metamorfoz
                new Song { Id = 9, Title = "Bounce", DurationInSeconds = 215, FilePath = "songs/bounce.mp3", ContentLevel = 5, AlbumId = 4 },
                new Song { Id = 10, Title = "Dedikodu", DurationInSeconds = 225, FilePath = "songs/dedikodu.mp3", ContentLevel = 4, AlbumId = 4 },
                // Ajda Pekkan - Superstar
                new Song { Id = 11, Title = "Superstar", DurationInSeconds = 198, FilePath = "songs/superstar.mp3", ContentLevel = 3, AlbumId = 5 },
                new Song { Id = 12, Title = "Bambaşka Biri", DurationInSeconds = 210, FilePath = "songs/bambaska.mp3", ContentLevel = 6, AlbumId = 5 },
                // Cem Karaca - Resimdeki Gözyaşları
                new Song { Id = 13, Title = "Tamirci Çırağı", DurationInSeconds = 245, FilePath = "songs/tamirci.mp3", ContentLevel = 2, AlbumId = 6 },
                new Song { Id = 14, Title = "Resimdeki Gözyaşları", DurationInSeconds = 280, FilePath = "songs/resimdeki.mp3", ContentLevel = 1, AlbumId = 6 },
                // Teoman - Onyedi
                new Song { Id = 15, Title = "İstanbul'da Sonbahar", DurationInSeconds = 235, FilePath = "songs/istanbul.mp3", ContentLevel = 4, AlbumId = 7 },
                new Song { Id = 16, Title = "Papatya", DurationInSeconds = 205, FilePath = "songs/papatya.mp3", ContentLevel = 5, AlbumId = 7 },
                // Sertab Erener - Every Way That I Can
                new Song { Id = 17, Title = "Every Way That I Can", DurationInSeconds = 180, FilePath = "songs/everyway.mp3", ContentLevel = 3, AlbumId = 8 },
                new Song { Id = 18, Title = "Leave", DurationInSeconds = 220, FilePath = "songs/leave.mp3", ContentLevel = 6, AlbumId = 8 },
                // Hadise - Hadise
                new Song { Id = 19, Title = "Düm Tek Tek", DurationInSeconds = 178, FilePath = "songs/dumtek.mp3", ContentLevel = 6, AlbumId = 9 },
                new Song { Id = 20, Title = "Superman", DurationInSeconds = 195, FilePath = "songs/superman.mp3", ContentLevel = 4, AlbumId = 9 },
                // Müslüm Gürses - Kâğıt Helva
                new Song { Id = 21, Title = "Kâğıt Helva", DurationInSeconds = 310, FilePath = "songs/kagithelva.mp3", ContentLevel = 5, AlbumId = 10 },
                new Song { Id = 22, Title = "Affet", DurationInSeconds = 285, FilePath = "songs/affet.mp3", ContentLevel = 3, AlbumId = 10 },
                // Ceza - Rapstar
                new Song { Id = 23, Title = "Holocaust", DurationInSeconds = 248, FilePath = "songs/holocaust.mp3", ContentLevel = 2, AlbumId = 11 },
                new Song { Id = 24, Title = "Neyim Var Ki", DurationInSeconds = 230, FilePath = "songs/neyimvarki.mp3", ContentLevel = 6, AlbumId = 11 },
                // Duman - Darmaduman
                new Song { Id = 25, Title = "Senden Daha Güzel", DurationInSeconds = 256, FilePath = "songs/sendendahaguzel.mp3", ContentLevel = 4, AlbumId = 12 },
                new Song { Id = 26, Title = "Yürekten", DurationInSeconds = 242, FilePath = "songs/yurekten.mp3", ContentLevel = 6, AlbumId = 12 },
                // Sagopa Kajmer - Bir Pesimistin Gözyaşları
                new Song { Id = 27, Title = "Baytar", DurationInSeconds = 268, FilePath = "songs/baytar.mp3", ContentLevel = 1, AlbumId = 13 },
                new Song { Id = 28, Title = "Düşün Ki", DurationInSeconds = 295, FilePath = "songs/dusunki.mp3", ContentLevel = 5, AlbumId = 13 },
                // Mor ve Ötesi - Dünya Yalan Söylüyor
                new Song { Id = 29, Title = "Cambaz", DurationInSeconds = 218, FilePath = "songs/cambaz.mp3", ContentLevel = 6, AlbumId = 14 },
                new Song { Id = 30, Title = "Dünya Yalan Söylüyor", DurationInSeconds = 240, FilePath = "songs/dunyayalan.mp3", ContentLevel = 3, AlbumId = 14 },
                // Nilüfer - Nilüfer
                new Song { Id = 31, Title = "Dünya Dönüyor", DurationInSeconds = 230, FilePath = "songs/dunyadonuyor.mp3", ContentLevel = 4, AlbumId = 15 },
                new Song { Id = 32, Title = "Geceler", DurationInSeconds = 255, FilePath = "songs/geceler.mp3", ContentLevel = 6, AlbumId = 15 },
                // Yalın - Ellerine Sağlık
                new Song { Id = 33, Title = "Ellerine Sağlık", DurationInSeconds = 220, FilePath = "songs/ellerinesaglik.mp3", ContentLevel = 5, AlbumId = 16 },
                new Song { Id = 34, Title = "Aşk Laftan Anlamaz", DurationInSeconds = 238, FilePath = "songs/asklaftan.mp3", ContentLevel = 2, AlbumId = 16 },
                // Gripin - M.S. 05.03.2010
                new Song { Id = 35, Title = "Böyle Kahpedir Dünya", DurationInSeconds = 252, FilePath = "songs/boylekahpe.mp3", ContentLevel = 6, AlbumId = 17 },
                new Song { Id = 36, Title = "Durma Yağmur Durma", DurationInSeconds = 227, FilePath = "songs/durmayagmur.mp3", ContentLevel = 3, AlbumId = 17 },
                // MFÖ - Ele Güne Karşı
                new Song { Id = 37, Title = "Ele Güne Karşı", DurationInSeconds = 210, FilePath = "songs/elegunekarsi.mp3", ContentLevel = 4, AlbumId = 18 },
                new Song { Id = 38, Title = "Ali Desidero", DurationInSeconds = 198, FilePath = "songs/alidesidero.mp3", ContentLevel = 6, AlbumId = 18 },
                // Manga - Şehr-i Hüzün
                new Song { Id = 39, Title = "Dünyanın Sonuna Doğmuşum", DurationInSeconds = 235, FilePath = "songs/dunyaninsonu.mp3", ContentLevel = 1, AlbumId = 19 },
                new Song { Id = 40, Title = "Bir Kadın Çizeceksin", DurationInSeconds = 248, FilePath = "songs/birkadin.mp3", ContentLevel = 5, AlbumId = 19 }
            );
        }
    }
}
