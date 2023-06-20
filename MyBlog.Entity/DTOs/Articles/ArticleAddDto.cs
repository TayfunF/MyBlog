using MyBlog.Entity.DTOs.Categories;
using MyBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entity.DTOs.Articles
{
    public class ArticleAddDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }
        public IList<CategoryDto> Categories { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid ImageId { get; set; }
        public Image Image { get; set; }

    }
}
