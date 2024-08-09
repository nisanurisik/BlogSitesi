using BlogSitesi.Data.Model;
using BlogSitesi.Data.UnitOfWork;
using BlogSitesi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BlogSitesi.Controllers
{
    public class ProfilController : Controller
    {
        UnitOfWork unitOfWork;

        public ProfilController()
        {
            unitOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
            var kullaniciAdi = Request.Cookies["kullanici"]["KullaniciAdi"];
            var kullanici = unitOfWork.GetRepository<Kullanici>().GetAll().FirstOrDefault(k => k.KullaniciAd == kullaniciAdi);
            var kullaniciBloglari = unitOfWork.GetRepository<Blog>().GetAll().Where(b => b.KullaniciId == kullanici.KullaniciId).ToList();

            var viewModel = new BlogKategoriViewModel
            {
                Bloglar = kullaniciBloglari,
                Kategoriler = unitOfWork.GetRepository<Kategori>().GetAll().ToList(),
                Kullanicilar = unitOfWork.GetRepository<Kullanici>().GetAll().ToList()
            };

            return View(viewModel);

        }
        public ActionResult Duzenle(int id)
        {
            var blog = unitOfWork.GetRepository<Blog>().GetAll().FirstOrDefault(b => b.BlogId == id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            var viewModel = new BlogKategoriViewModel
            {
                Bloglar = new List<Blog> { blog },
                Kategoriler = unitOfWork.GetRepository<Kategori>().GetAll().ToList(),
                Kullanicilar = unitOfWork.GetRepository<Kullanici>().GetAll().ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult GuncelleJson(int BlogId, string Baslik, string Metin, string[] kullanicilar, string[] kategoriler, HttpPostedFileBase Resim)
        {
            try
            {
                const int maxFileSize = 2 * 1024 * 1024; 
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };

                var blog = unitOfWork.GetRepository<Blog>().GetAll().FirstOrDefault(b => b.BlogId == BlogId);
                if (blog == null)
                {
                    return Json(new { success = false, message = "Blog bulunamadı." });
                }

                int kullaniciID = 0;
                if (kullanicilar != null && kullanicilar.Length > 0 && int.TryParse(kullanicilar.FirstOrDefault(), out kullaniciID))
                {
                    var kullanicim = unitOfWork.GetRepository<Kullanici>().GetById(kullaniciID);
                    blog.BlogAd = Baslik;
                    blog.BlogMetin = Metin;
                    blog.KullaniciId = kullaniciID;

                    if (Resim != null && Resim.ContentLength > 0)
                    {
                        if (Resim.ContentLength > maxFileSize)
                        {
                            return Json(new { success = false, message = "Dosya boyutu 2 MB'den küçük olmalıdır." });
                        }

                        var extension = Path.GetExtension(Resim.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(extension))
                        {
                            return Json(new { success = false, message = "Geçerli bir resim seçiniz!" });
                        }

                        var randomFileName = $"{Guid.NewGuid()}{extension}";
                        var path = Path.Combine(Server.MapPath("~/Uploads"), randomFileName);

                        try
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path)); 
                            Resim.SaveAs(path);
                            blog.BlogResim = "/Uploads/" + randomFileName;
                        }
                        catch (Exception ex)
                        {
                            return Json(new { success = false, message = "Dosya yüklenirken bir hata oluştu: " + ex.Message });
                        }
                    }

                    blog.Kategoriler.Clear();
                    List<Kategori> k = new List<Kategori>();
                    if (kategoriler != null)
                    {
                        foreach (var kategoriId in kategoriler)
                        {
                            var kategoriID = Convert.ToInt32(kategoriId);
                            var kategori = unitOfWork.GetRepository<Kategori>().GetById(kategoriID);
                            k.Add(kategori);
                        }
                    }
                    blog.Kategoriler = k;

                    unitOfWork.GetRepository<Blog>().Update(blog);
                    var durum = unitOfWork.SaveChanges();

                    if (durum > 0)
                    {
                        var redirectUrl = Url.Action("Index", "Profil", null, Request.Url.Scheme);
                        return Json(new { success = true, redirectUrl });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Blog güncellenemedi." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Kullanıcı seçimi yapılmadı." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sunucu tarafında bir hata oluştu: " + ex.Message });
            }
        }







        [HttpPost]
        public JsonResult BlogumuSilJson(int BlogId)
        {
            unitOfWork.GetRepository<Blog>().Delete(BlogId);
            var durum = unitOfWork.SaveChanges();
            if (durum > 0) return Json("1");
            return Json("0");
        }

    }
}