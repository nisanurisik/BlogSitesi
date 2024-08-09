
using BlogSitesi.Data.Model;
using BlogSitesi.Data.UnitOfWork;
using BlogSitesi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;

namespace BlogSitesi.Controllers
{
    public class BlogYazController : Controller
    {
        UnitOfWork unitOfWork;
        public BlogYazController()
        {
            unitOfWork = new UnitOfWork();
        }

        public ActionResult Index()
        {
            var viewModel = new BlogKategoriViewModel
            {
                Kategoriler = unitOfWork.GetRepository<Kategori>().GetAll().ToList(),
                Kullanicilar = unitOfWork.GetRepository<Kullanici>().GetAll().ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
       
        public JsonResult EkleJson(string baslik, string metin, string[] kullanicilar, string[] kategoriler, HttpPostedFileBase resim)
        {
            try
            {
                const int maxFileSize = 2 * 1024 * 1024; 
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };

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

                int kullaniciID = 0;
                if (kullanicilar != null && kullanicilar.Length > 0 && int.TryParse(kullanicilar.FirstOrDefault(), out kullaniciID))
                {
                    var kullanicim = unitOfWork.GetRepository<Kullanici>().GetById(kullaniciID);

                    Blog blog = new Blog();
                    blog.BlogAd = baslik;
                    blog.BlogMetin = metin;
                    blog.KullaniciId = kullaniciID;
                    blog.Kullanici = kullanicim;
                    blog.Kategoriler = k;
                    blog.BlogYazilmaTarihi = DateTime.Now;

                    if (resim != null && resim.ContentLength > 0)
                    {
                        if (resim.ContentLength > maxFileSize)
                        {
                            return Json("Dosya boyutu 2 MB'den küçük olmalıdır.");
                        }

                        var extension = Path.GetExtension(resim.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(extension))
                        {
                            return Json("Geçerli bir resim seçiniz!");
                        }

                        var randomFileName = $"{Guid.NewGuid()}{extension}";
                        var path = Path.Combine(Server.MapPath("~/Content/Images"), randomFileName);

                        try
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path)); 
                            resim.SaveAs(path);
                            blog.BlogResim = "/Content/Images/" + randomFileName;
                        }
                        catch (Exception ex)
                        {
                            return Json("Dosya yüklenirken bir hata oluştu: " + ex.Message);
                        }
                    }
                    else
                    {
                        return Json("Bir dosya seçiniz.");
                    }

                    unitOfWork.GetRepository<Blog>().Add(blog);
                    var durum = unitOfWork.SaveChanges();
                    if (durum > 0)
                        return Json("1");
                    else
                        return Json("0");
                }
                else
                {
                    return Json("Kullanıcı seçimi yapılmadı.");
                }
            }
            catch (Exception ex)
            {
                return Json("Sunucu tarafında bir hata oluştu: " + ex.Message);
            }
        }





    }
}

