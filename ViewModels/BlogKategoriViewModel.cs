using BlogSitesi.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogSitesi.ViewModels
{
    public class BlogKategoriViewModel
    {
        public List<Blog> Bloglar { get; set; }
        public List<Kategori> Kategoriler { get; set; }
        public List<Kullanici> Kullanicilar { get; set; }
    }
}