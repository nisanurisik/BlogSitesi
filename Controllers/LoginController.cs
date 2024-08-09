using BlogSitesi.Data.UnitOfWork;
using System.Web;
using System.Web.Mvc;
using BlogSitesi.Data.Model;
using BlogSitesi.Filters;
using System.Web.Security;
using System;

namespace BlogSitesi.Controllers
{
    public class LoginController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public LoginController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public ActionResult Index()
        {
            var user = Session["LoggedUser"] as Kullanici;
            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public JsonResult GirisKontrol(string email, string sifre, bool hatirla)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
                return Json("BosOlamaz");

            var kullanici = new Kullanici();
            try
            {
                kullanici = _unitOfWork.GetRepository<Kullanici>().Get(x => x.KullaniciMail == email && x.KullaniciSifre == sifre);
            }
            catch
            {
                return Json("Hata");
            }

            if (kullanici != null)
            {
                var existingUser = Session["LoggedUser"] as Kullanici;
                if (existingUser != null && existingUser.KullaniciId != kullanici.KullaniciId)
                {
                    FormsAuthentication.SignOut();
                    Session.Clear();
                }

                Session["LoggedUser"] = kullanici;

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

        [NoCache]
        public ActionResult CikisYap()
        {
            if (Request.Cookies["kullanici"] != null)
            {
                var cookie = new HttpCookie("kullanici")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Value = null
                };
                Response.Cookies.Add(cookie);
            }

            Session.Clear();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Login");
        }
    }
}
