using MyBlog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entity.Entities
{
    public class Article : EntityBase
    {
        public Article()
        {
        }

        public Article(string title, string content, Guid appuserId, string createdBy, Guid cagegoryId, Guid imageId)
        {
            Title = title;
            Content = content;
            AppUserId = appuserId;
            CategoryId = cagegoryId;
            ImageId = imageId;
            CreatedBy = createdBy;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; } = 0;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid? ImageId { get; set; }
        public Image Image { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
