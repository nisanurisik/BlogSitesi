$(document).on("click", ".ktgEkle", async function () {
    const { value: formValues } = await Swal.fire({
        title: "Kategori Ekle",
        html:
            '<input id="ktgAd" class="swal2-input">'

    })
    if (formValues) {
        var ktgAd = $("#ktgAd").val();
        $.ajax({
            type: 'Post',
            url: '/Kategori/EkleJson',
            data: { "ktgAd": ktgAd },
            dataType: 'Json',
            success: function (data) {
                var ktgId = data.Result.KategoriId;
                var ktgAd = '<td>' + data.Result.KategorAd + '</td>';
                var guncelleButon = "<td><button class='guncelle btn btn-custom' value='" + ktgId + "'>Güncelle</button></td>";
                var silButon = "<td><button class='sil btn btn-custom' value='" + ktgId + "'>Sil</button></td>";

                $("table #example tbody").prepend('<tr>' + ktgAd + guncelleButon + silButon + '</tr>');
                Swal.fire({
                    type: 'success',
                    title: 'Kategori Eklendi',
                    text: 'İşlem Başarıyla Gerçekleştirildi!'
                });
            },
            error: function () {
                Swal.fire({
                    type: 'error',
                    title: 'Kategori Eklenemedi',
                    text: 'İşlem Gerçekleşmedi!'
                });
            }
        });
    }
});

$(document).on("click", ".guncelle", async function () {
    var KategoriId = $(this).val();
    var KategoriAdTd = $(this).parent("td").parent("tr").find("td:first");
    var KategoriAd = KategoriAdTd.html();

    const { value: formValues } = await Swal.fire({
        title: "Kategori Güncelle",
        html: '<input id="KategoriAd" value="' + KategoriAd + '" class="swal2-input">',


    })

    KategoriAd = $("#KategoriAd").val();

    $.ajax({
        type: 'Post',
        url: '/Kategori/GuncelleJson',
        data: { "KategoriId": KategoriId, "KategoriAd": KategoriAd },
        dataType: 'Json',
        success: function (data) {
            if (data == "1") {
                KategoriAdTd.html(KategoriAd);
                Swal.fire({
                    icon: 'success',
                    title: 'Kategori Güncellendi',
                    text: 'İşlem Başarıyla Gerçekleştirildi!'
                });
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Kategori Güncellenemedi',
                    text: 'İşlem Gerçekleşmedi!'
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Kategori Güncellenemedi',
                text: 'İşlem Gerçekleşmedi!'
            });
        }
    });
});

$(document).on("click", ".sil", async function () {
    var tr = $(this).parent("td").parent("tr");
    var KategoriId = $(this).val();
    $.ajax({
        type: 'Post',
        url: '/Kategori/SilJson',
        data: { "KategoriId": KategoriId },
        dataType: 'Json',
        success: function (data) {
            if (data == "1") {
                tr.remove();
                Swal.fire({
                    icon: 'success',
                    title: 'Kategori Silindi',
                    text: 'İşlem Başarıyla Gerçekleştirildi!'
                });
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Kategori Silinemedi',
                    text: 'İşlem Gerçekleşmedi!'
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Kategori Silinemedi',
                text: 'İşlem Gerçekleşmedi!'
            });
        }
    });
});

$(document).on("click", ".blogSil", function () {
    var blogId = $(this).val();
    var blogElement = $(this).closest(".blog-box");

    $.ajax({
        type: 'Post',
        url: '/Blog/BlogSilJson',
        data: { BlogId: blogId },
        dataType: 'json',
        success: function (data) {
            if (data == "1") {
                blogElement.remove();
                Swal.fire({
                    icon: 'success',
                    title: 'Blog Silindi',
                    text: 'İşlem Başarıyla Gerçekleştirildi!'
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Blog Silinemedi',
                    text: 'İşlem Gerçekleşmedi!'
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Blog Silinemedi',
                text: 'İşlem Gerçekleşmedi!'
            });
        }
    });
});
$(document).ready(function () {
    $(".category-link").click(function (e) {
        e.preventDefault();
        var categoryId = $(this).data("id");

        $.ajax({
            url: '/Blog/GetBlogsByCategory',
            type: 'GET',
            data: { categoryId: categoryId },
            dataType: 'json',
            success: function (data) {
                var blogList = $("#blogList");
                blogList.empty();

                $.each(data, function (index, blog) {
                    var kategoriler = blog.Kategoriler.join(", ");
                    var resimUrl = blog.BlogResim ? blog.BlogResim : '';

                    var blogHtml = `
                        <div class="col-md-12">
                            <div class="white-box blog-box">
                                <div class="row">
                                    <div class="col-sm-3">
                                        ${resimUrl ? `<img style="max-width: 100%; height: 200px; object-fit: cover;" src="${resimUrl}" alt="Blog Resmi" />` : '<p>Resim bulunamadı.</p>'}
                                    </div>
                                    <div class="col-sm-9">
                                        <h3 class="box-title">${blog.BlogAd}</h3>
                                        <p>${blog.BlogMetin}</p>
                                        <p>Yazan: ${blog.KullaniciAd} ${blog.KullaniciSoyad}</p>
                                         <p>Blog Yayınlanma Tarihi: ${blog.BlogYazilmaTarihi}</p>
                                        <p style="border: 2px solid #405D72; padding: 5px; display: inline-block; background-color: #405D72; color:antiquewhite">${kategoriler}</p>
                                    </div>
                                </div>
                            </div>
                        </div>`;
                    blogList.append(blogHtml);
                });
            },
            error: function (xhr, status, error) {
                alert("An error occurred while fetching blogs: " + xhr.responseText + "\nStatus: " + status + "\nError: " + error);
            }
        });
    });
});

$(document).on("click", "#kategoriSec", function () {
    var secilenKategoriAd = $("#kategoriler option:selected").text();
    var secilenKategoriId = $("#kategoriler option:selected").val();
    $("#eklenenKategoriler").append('<div id="' + secilenKategoriId + '" data-id="' + secilenKategoriId + '" class="col-md-1" style="background-color: #758694; color: white; border: 3px solid #758694; padding: 4px; margin-right: 5px; margin-bottom: 5px; display: inline-block;">' + secilenKategoriAd + '</div>');
});

$(document).on("click", ".kategoriSil", function () {
    var Id = $(this).attr("Id");
    var kategoriAd = $(this).html();
    $("#kategoriler").append('<option data-id="' + Id + '">' + kategoriAd + '</option>');
    $(this).remove();
});
$(document).on("click", "#kullaniciSec", function () {
    var secilenKullaniciAd = $("#kullanicilar option:selected").text();
    var secilenKullaniciId = $("#kullanicilar option:selected").val();
    $("#eklelenKullanici").append('<div id="' + secilenKullaniciId + '" data-id="' + secilenKullaniciId + '" class="col-md-1" style="background-color: #758694; color: white; border: 3px solid #758694; padding: 4px; margin-right: 5px; margin-bottom: 5px; display: inline-block;">' + secilenKullaniciAd + '</div>');
});

$(document).on("click", "#blogKaydet", function () {
    var formData = new FormData();

    formData.append("baslik", $("#baslik").val());
    formData.append("metin", $("#metin").val());

    
    var kullaniciSecildi = false;
    $("#eklelenKullanici div").each(function () {
        var id = $(this).attr("id");
        if (id) {
            formData.append("kullanicilar[]", id);
            kullaniciSecildi = true;
        }
    });

    if (!kullaniciSecildi) {
        Swal.fire({
            icon: 'error',
            title: 'Kullanıcı Seçilmedi',
            text: 'Lütfen bir kullanıcı seçin!'
        });
        return;
    }

   
    var kategoriSecildi = false;
    $("#eklenenKategoriler div").each(function () {
        var id = $(this).attr("id");
        if (id) {
            formData.append("kategoriler[]", id);
            kategoriSecildi = true;
        }
    });

    if (!kategoriSecildi) {
        Swal.fire({
            icon: 'error',
            title: 'Kategori Seçilmedi',
            text: 'Lütfen bir kategori seçin!'
        });
        return;
    }

    var fileInput = document.getElementById("resim");
    if (fileInput.files.length > 0) {
        formData.append("resim", fileInput.files[0]);
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Dosya Seçilmedi',
            text: 'Lütfen bir dosya seçin!'
        });
        return;
    }

    $.ajax({
        url: '/BlogYaz/EkleJson',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (gelenDeg) {
            if (gelenDeg == "1") {
                Swal.fire({
                    icon: 'success',
                    title: 'Blog Yazıldı',
                    text: 'İşlem Gerçekleşti!'
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Blog Yazılamadı',
                    text: gelenDeg
                });
            }
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);  
            Swal.fire({
                icon: 'error',
                title: 'Blog Yazılamadı',
                text: 'İşlem Gerçekleşmedi! Hata: ' + xhr.responseText
            });
        }
    });
});

$(document).on("click", "#giris", function () {
    $(this).html("Kontrol Ediliyor...");
    $(this).prop("disabled", true);

    var degerler = {
        email: $("#email").val(),
        sifre: $("#sifre").val(),
        hatirla: false
    };

    if ($("#checkbox-signup").is(":checked")) {
        degerler.hatirla = true;
    }

    $.ajax({
        type: 'Post',
        url: '/Login/GirisKontrol',
        data: JSON.stringify(degerler),
        dataType: 'Json',
        contentType: 'application/json;charset=utf-8',
        success: function (gelenDeg) {
            console.log(gelenDeg); // Geri dönen cevabı kontrol edin
            if (gelenDeg == "Başarılı") {
                Swal.fire({ icon: "success", title: "Giriş Başarılı", text: "Yönlendiriliyorsunuz..." });
                window.location.href = '/Blog/Index';
            }
            else if (gelenDeg == "BosOlamaz") {
                Swal.fire({ icon: "error", title: "Giriş Başarısız", text: "Gerekli Alanları Doldurun..." });
            }
            else {
                Swal.fire({ icon: "error", title: "Giriş Başarısız", text: "Hata..." });
            }
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText); // Hata mesajını kontrol edin
            Swal.fire({ icon: "error", title: "HATA", text: "Hata..." });
        },
        complete: function () {
            $("#giris").html("Giriş Yap");
            $("#giris").prop("disabled", false);
        }
    });
});
