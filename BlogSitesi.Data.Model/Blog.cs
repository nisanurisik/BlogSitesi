using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSitesi.Data.Model
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        public string BlogAd { get; set; }
        public string BlogMetin { get; set; }
        public string BlogResim { get; set; }
        public DateTime BlogYazilmaTarihi { get; set; }

        [ForeignKey("Kullanici")]
        public int KullaniciId { get; set; }
        public virtual Kullanici Kullanici { get; set; }

        public virtual ICollection<Yorum> Yorumlar { get; set; }
        public virtual ICollection<Kategori> Kategoriler { get; set; }

        public static implicit operator List<object>(Blog v)
        {
            throw new NotImplementedException();
        }
    }
}
