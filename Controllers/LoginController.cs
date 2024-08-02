using BlogSitesi.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSitesi.Data.Model;
namespace BlogSitesi.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private readonly UnitOfWork _unitOfWork;

        public LoginController()
        {
            _unitOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GirisKontrol(string email, string sifre, bool hatirla)
        {
            //email = email.Trim();
            //sifre = sifre.Trim();
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(sifre))
                return Json("BosOlamaz");

            var kullanici = new Kullanici();
            try
            {
                kullanici = _unitOfWork.GetRepository<Kullanici>().Get(x => x.KullaniciMail == email && x.KullaniciSifre == sifre);
            }
            catch
            {

            }
            if (kullanici != null)
            {
                HttpCookie cookie = new HttpCookie("kullanici");
                cookie.Values.Add("KullaniciId", kullanici.KullaniciId.ToString());
                cookie.Values.Add("KullaniciAdi", kullanici.KullaniciAd);
                cookie.Values.Add("KullaniciSoyadi", kullanici.KullaniciSoyad);
                cookie.Values.Add("KullaniciYetki", kullanici.KullaniciYetki);

                if (hatirla) cookie.Expires = DateTime.Now.AddDays(5);
                Response.Cookies.Add(cookie);
                return Json("Başarılı");
            }
            else return Json("Hata");
        }
        public ActionResult CikisYap()
        {
            var cookie = Response.Cookies["kullanici"];
            if (cookie != null)
                cookie.Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index");

        }
    }
}