using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSitesi.Data.Model
{
    public class Kullanici
    {
        [Key]
        public int KullaniciId { get; set; }
        public string KullaniciAd { get; set; }
        public string KullaniciSoyad { get; set; }
        public string KullaniciTc { get; set; }
        public string KullaniciTel { get; set; }
        public DateTime KullaniciKayitTarihi { get; set; }
        public string KullaniciMail { get; set; }
        public string KullaniciSifre { get; set; }
        public string KullaniciYetki { get; set; }

        public virtual ICollection<Yorum> Yorumlar { get; set; }
        public virtual ICollection<Blog> Bloglar { get; set; }
    }
}
