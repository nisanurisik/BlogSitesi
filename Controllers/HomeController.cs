using BlogSitesi.Data.Model;
using BlogSitesi.Data.UnitOfWork;
using BlogSitesi.Filters;
using BlogSitesi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSitesi.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork unitOfWork;
        public HomeController()
        {
            unitOfWork = new UnitOfWork();
        }
        // GET: Home

   
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

        public JsonResult KategoriListele(int categoryId)
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

    }
}