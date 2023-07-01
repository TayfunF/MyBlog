# Herkese Merhaba!

Bu projede bir blog sitesinin kodlamasını gerçekleştiriyorum. Sitede kategorilere ayrılmış makaleler bulunmakta. Kategori ve makale yönetimini yapabilmek için bir admin panelim var. Admin paneline kullanıcı girişi yapılarak ulaşılıyor ve giriş yapıldıktan sonra yetkilendirmeler dahilinde sayfalara erişim sağlanabiliyor. 

Kullandığım teknolojiler, araçlar, yardımcı nesneleri aşağıda listeledim;

 - Projeyi oluşturabilmek için  ide **Visual Studio 2022**
 - Veri tabanı ve idesi : **Mssql** ve **Sql Server Management Studio
   2019**
 - Programlama, işaret dili  **C# , HTML-CSS-JS**
 - Framework  **.Net 6.0.18**
 - Proje Yapısı **N-Katmanlı Mimari (Core-Entity-Data-Service-Web)**
 - Tasarım Kalıpları **Generic Repository, MVC, Unit Of Work ve Services
   kullanımları.**
 - Yaklaşım **Code First Yapısı ile Migration**
 - Verileri ekleme ve düzenleme  **Seed & Configuration ile Migration**
 - Dto (ViewModel) yapısı için **AutoMapper (12.0.1)**
 - Hata mesajlarının türkçeleştirilmesi **ErrorDescriber**
 - Doğrulama işlemleri  **Fluent Validation (11.3.0)**
 - Resim Yükleme işlemleri için **ImageHelper**
 - Scaffolding yapısı ile **Admin Area**
 - Giriş ve yetkilendirme işlemleri **Identity Server**
 - Beni hatırla özelliğinin kullanımı **Identity Server**
 - Şifremi Unuttum ile kullanıcının e-posta adresine token gönderilmesi
   ve yeni şifre belirleme sürecinin kullanımı **Identity Server**
 - Verileri listelemek için **Custom Pagination**, **Filter** ve
   **DataTableJs **
 - Kullanıcı bilgilendirmeler için **Toastr Notification**
 - Sayfalardaki karmaşıklığı azaltmak ve birden fazla model dönülmesi
   gereken sayfaları yönetebilmek için **ViewComponents, Layout**

---

> Projeye devam ediyorum. Kullanıcı tarafında hakkımda, iletişim, yorum vb. işlemler, admin paneli tarafında şifre değiştirme sayfası, istatistiksel grafikler ve loglama işlemlerinin takibi gibi pek çok yapı eklenebilir. Zamanla eklemeyi düşünüyorum.

---

Mottomuz:

```mermaid
graph LR
A[Araştır] -- Öğren --> B{Uygula}
A[Araştır] -- Tekrar Et --> B{Uygula}
