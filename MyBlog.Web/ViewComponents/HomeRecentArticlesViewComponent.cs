using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Services.Abstracts;

namespace MyBlog.Web.ViewComponents
{
    public class HomeRecentArticlesViewComponent : ViewComponent
    {
        private readonly IArticleService _articleService;

        public HomeRecentArticlesViewComponent(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var recentArticles = await _articleService.GetAllArticlesWithCategoryNonDeletedTake3Async();

            return View(recentArticles);
        }
    }
}
