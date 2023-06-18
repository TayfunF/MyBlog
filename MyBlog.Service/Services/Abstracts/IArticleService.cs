using MyBlog.Entity.DTOs.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Services.Abstracts
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetAllArticlesWithCategoryNonDeletedAsync();
        Task AddArticleAsync(ArticleAddDto articleAddDto);
    }
}
