using BlogSitesi.Data;
using BlogSitesi.Data.Model;
using BlogSitesi.Data.UnitOfWork;
using BlogSitesi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BlogSitesi.Controllers
{
    public class BlogController : Controller
    {
        UnitOfWork unitOfWork;

        public BlogController()
        {
            unitOfWork = new UnitOfWork();
        }

        public ActionResult Index()
        {
            var viewModel = new BlogKategoriViewModel
            {
                Bloglar = unitOfWork.GetRepository<Blog>().GetAll().ToList(),
                Kategoriler = unitOfWork.GetRepository<Kategori>().GetAll().ToList(),
                 Kullanicilar = unitOfWork.GetRepository<Kullanici>().GetAll().ToList()
            };

            return View(viewModel);
        }

        public ActionResult BlogSil()
        {
            var viewModel = new BlogKategoriViewModel
            {
                Bloglar = unitOfWork.GetRepository<Blog>().GetAll().ToList(),
                Kategoriler = unitOfWork.GetRepository<Kategori>().GetAll().ToList(),
                Kullanicilar = unitOfWork.GetRepository<Kullanici>().GetAll().ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public JsonResult BlogSilJson(int BlogId)
        {
            unitOfWork.GetRepository<Blog>().Delete(BlogId);
            var durum = unitOfWork.SaveChanges();
            if (durum > 0) return Json("1");
            return Json("0");
        }


        public JsonResult GetBlogsByCategory(int categoryId)
        {
            var blogs = unitOfWork.GetRepository<Blog>()
                .GetAll()
                .Where(b => b.Kategoriler.Any(k => k.KategoriId == categoryId))
                .Select(b => new
                {
                    b.BlogAd,
                    b.BlogMetin,
                    KullaniciAd = b.Kullanici.KullaniciAd,
                    KullaniciSoyad = b.Kullanici.KullaniciSoyad,
                    b.BlogResim,
                    BlogYazilmaTarihi = b.BlogYazilmaTarihi.ToString("dd MMM yyyy"),
                    Kategoriler = b.Kategoriler.Select(k => k.KategoriAd).ToList()
                })
                .ToList();

            return Json(blogs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var blogRepository = unitOfWork.GetRepository<Blog>();
            var kategoriRepository = unitOfWork.GetRepository<Kategori>();
            var kullaniciRepository = unitOfWork.GetRepository<Kullanici>();

            var blogs = blogRepository.GetAll()
                                .Where(b => b.BlogAd.Contains(searchTerm)
                                         || b.BlogMetin.Contains(searchTerm)
                                         || b.Kullanici.KullaniciAd.Contains(searchTerm)
                                         || b.Kullanici.KullaniciSoyad.Contains(searchTerm)
                                         ||(string.Concat(b.Kullanici.KullaniciAd, " ", b.Kullanici.KullaniciSoyad).Contains(searchTerm)))
                                .ToList();

            var viewModel = new BlogKategoriViewModel
            {
                Bloglar = blogs,
                Kategoriler = kategoriRepository.GetAll().ToList(),
                Kullanicilar = kullaniciRepository.GetAll().ToList()
            };

            return View("Index", viewModel);
        }


    }
}
