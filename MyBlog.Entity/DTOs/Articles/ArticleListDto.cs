using MyBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entity.DTOs.Articles
{
    public class ArticleListDto
    {
        public List<Article> Articles { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual int CurrentPage { get; set; } = 1; //Sayfalama 1. sayfadan başlasın
        public virtual int PageSize { get; set; } = 3; //Bir sayfada kaç makale olacağını belirliyorum
        public virtual int TotalCount { get; set; } //Toplam Sayfa sayısı kaça kadar gidecek. Örn : < 1 2 3 4 5 >
        public virtual int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize)); //Ceiling : Ne olursa olsun ondalık sayıyı bir üst sayıya yuvarlar.
        public virtual bool ShowPrevious => CurrentPage > 1; //Önceki sayfayı göstermek için. Öncesi yoksa '<' işareti göstermiycem.
        public virtual bool ShowNext => CurrentPage < TotalPages; //Sonraki sayfayı göstermek için. Sonrası yoksa '>' işareti göstermiycem.
        public bool IsAscending { get; set; } = false;

    }
}
