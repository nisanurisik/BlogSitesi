namespace BlogSitesi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blog",
                c => new
                    {
                        BlogId = c.Int(nullable: false, identity: true),
                        BlogAd = c.String(),
                        BlogMetin = c.String(),
                        BlogResim = c.String(),
                        BlogYazilmaTarihi = c.DateTime(nullable: false),
                        KullaniciId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlogId)
                .ForeignKey("dbo.Kullanici", t => t.KullaniciId, cascadeDelete: true)
                .Index(t => t.KullaniciId);
            
            CreateTable(
                "dbo.Kategori",
                c => new
                    {
                        KategoriId = c.Int(nullable: false, identity: true),
                        KategoriAd = c.String(),
                    })
                .PrimaryKey(t => t.KategoriId);
            
            CreateTable(
                "dbo.Kullanici",
                c => new
                    {
                        KullaniciId = c.Int(nullable: false, identity: true),
                        KullaniciAd = c.String(),
                        KullaniciSoyad = c.String(),
                        KullaniciTc = c.String(),
                        KullaniciTel = c.String(),
                        KullaniciKayitTarihi = c.DateTime(nullable: false),
                        KullaniciMail = c.String(),
                        KullaniciSifre = c.String(),
                        KullaniciYetki = c.String(),
                    })
                .PrimaryKey(t => t.KullaniciId);
            
            CreateTable(
                "dbo.Yorum",
                c => new
                    {
                        YorumId = c.Int(nullable: false, identity: true),
                        YorumAd = c.String(),
                        YorumMetin = c.String(),
                        KullaniciId = c.Int(nullable: false),
                        BlogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.YorumId)
                .ForeignKey("dbo.Kullanici", t => t.KullaniciId)
                .ForeignKey("dbo.Blog", t => t.BlogId, cascadeDelete: true)
                .Index(t => t.KullaniciId)
                .Index(t => t.BlogId);
            
            CreateTable(
                "dbo.BlogKategori",
                c => new
                    {
                        BlogId = c.Int(nullable: false),
                        KategoriId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BlogId, t.KategoriId })
                .ForeignKey("dbo.Blog", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("dbo.Kategori", t => t.KategoriId, cascadeDelete: true)
                .Index(t => t.BlogId)
                .Index(t => t.KategoriId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Yorum", "BlogId", "dbo.Blog");
            DropForeignKey("dbo.Yorum", "KullaniciId", "dbo.Kullanici");
            DropForeignKey("dbo.Blog", "KullaniciId", "dbo.Kullanici");
            DropForeignKey("dbo.BlogKategori", "KategoriId", "dbo.Kategori");
            DropForeignKey("dbo.BlogKategori", "BlogId", "dbo.Blog");
            DropIndex("dbo.BlogKategori", new[] { "KategoriId" });
            DropIndex("dbo.BlogKategori", new[] { "BlogId" });
            DropIndex("dbo.Yorum", new[] { "BlogId" });
            DropIndex("dbo.Yorum", new[] { "KullaniciId" });
            DropIndex("dbo.Blog", new[] { "KullaniciId" });
            DropTable("dbo.BlogKategori");
            DropTable("dbo.Yorum");
            DropTable("dbo.Kullanici");
            DropTable("dbo.Kategori");
            DropTable("dbo.Blog");
        }
    }
}
