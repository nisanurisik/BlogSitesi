using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSitesi.Data;
using BlogSitesi.Data.Model;
using BlogSitesi.Data.UnitOfWork;

namespace BlogSitesi.Controllers
{
    public class KategoriController : Controller
    {
        UnitOfWork unitOfWork;
        public KategoriController()
        {
            unitOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
            var kategoriler = unitOfWork.GetRepository<Kategori>().GetAll();
            return View(kategoriler);
        }

        [HttpPost]
        public JsonResult EkleJson(string ktgAd)
        { 
            Kategori ktgr = new Kategori();
            ktgr.KategoriAd = ktgAd;
            var eklenenKategori = unitOfWork.GetRepository<Kategori>().Add(ktgr);
            unitOfWork.SaveChanges();
            return Json(
                    new
                    {
                        Result = new
                        {
                            KategoriId = eklenenKategori.KategoriId,
                            KategoriAd = eklenenKategori.KategoriAd
                        },
                        JsonRequestBehavior.AllowGet
                    }
                );
        
        }
        [HttpPost]
        public JsonResult GuncelleJson(int KategoriId, string KategoriAd)
        {
            var kategori = unitOfWork.GetRepository<Kategori>().GetById(KategoriId);
            kategori.KategoriAd = KategoriAd;
            var durum = unitOfWork.SaveChanges();
            if (durum > 0) return Json("1");
            return Json("0");
        }

        [HttpPost]
        public JsonResult SilJson(int KategoriId)
        {
            unitOfWork.GetRepository<Kategori>().Delete(KategoriId);
            var durum = unitOfWork.SaveChanges();
            if (durum > 0) return Json("1");
            return Json("0");
        }
    }
}