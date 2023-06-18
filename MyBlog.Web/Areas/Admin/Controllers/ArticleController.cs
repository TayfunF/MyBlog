using Microsoft.AspNetCore.Mvc;
using MyBlog.Entity.DTOs.Articles;
using MyBlog.Service.Services.Abstracts;

namespace MyBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        public ArticleController(IArticleService articleService, ICategoryService categoryService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _articleService.GetAllArticlesWithCategoryNonDeletedAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryService.GetAllCategoriesNonDeletedAsync();
            return View(new ArticleAddDto { Categories = categories });
        }

        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddDto articleAddDto)
        {
            await _articleService.AddArticleAsync(articleAddDto);
            RedirectToAction("Index", "Article", new { Area = "Admin" });

            var categories = await _categoryService.GetAllCategoriesNonDeletedAsync();
            return View(new ArticleAddDto { Categories = categories });
        }
    }
}
