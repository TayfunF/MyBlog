using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Entity.DTOs.Articles;
using MyBlog.Entity.DTOs.Categories;
using MyBlog.Service.Services.Abstracts;

namespace MyBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _mapper = mapper;
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

        [HttpGet]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var article = await _articleService.GetArticleWithCategoryNonDeletedAsync(articleId);

            var categories = await _categoryService.GetAllCategoriesNonDeletedAsync();

            ArticleUpdateDto articleUpdateDto = _mapper.Map<ArticleUpdateDto>(article);
            articleUpdateDto.Categories = categories;

            return View(articleUpdateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ArticleUpdateDto articleUpdateDto)
        {
            await _articleService.UpdateArticleAsync(articleUpdateDto);

            var categories = await _categoryService.GetAllCategoriesNonDeletedAsync();
            articleUpdateDto.Categories = categories;

            return View(articleUpdateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid articleId)
        {
            await _articleService.DeleteSafeAsync(articleId);

            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }
    }
}
