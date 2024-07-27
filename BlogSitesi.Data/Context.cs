using BlogSitesi.Data.Migrations;
using BlogSitesi.Data.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BlogSitesi.Data
{
    public class Context : DbContext
    {
        public Context() : base("Context")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>("Context"));
        }

        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<Blog> Bloglar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Veritabanında tabloların sonundaki s takısını kaldırdım.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Blog-Kategori ilişkisi (Many-to-Many)
            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Kategoriler)
                .WithMany(k => k.Bloglar)
                .Map(m =>
                {
                    m.MapLeftKey("BlogId");
                    m.MapRightKey("KategoriId");
                    m.ToTable("BlogKategori");
                });

            // Blog-Yorum ilişkisi (One-to-Many)
            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Yorumlar)
                .WithRequired(y => y.Blog)
                .HasForeignKey(y => y.BlogId)
                .WillCascadeOnDelete(true); // Eklenen satır

            // Kullanici-Yorum ilişkisi (One-to-Many)
            modelBuilder.Entity<Kullanici>()
                .HasMany(k => k.Yorumlar)
                .WithRequired(y => y.Kullanici)
                .HasForeignKey(y => y.KullaniciId)
                .WillCascadeOnDelete(false); // Eklenen satır

            // Kullanici-Blog ilişkisi (One-to-Many)
            modelBuilder.Entity<Kullanici>()
                .HasMany(k => k.Bloglar)
                .WithRequired(b => b.Kullanici)
                .HasForeignKey(b => b.KullaniciId)
                .WillCascadeOnDelete(true); // Eklenen satır

            base.OnModelCreating(modelBuilder);
        }
    }
}
