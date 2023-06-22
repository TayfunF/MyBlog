using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Entity.DTOs.Articles;
using MyBlog.Entity.DTOs.Categories;
using MyBlog.Entity.Entities;
using MyBlog.Service.Extensions;
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
        private readonly IValidator<Article> _validator;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper, IValidator<Article> validator)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _mapper = mapper;
            _validator = validator;
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
            var map = _mapper.Map<Article>(articleAddDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await _articleService.AddArticleAsync(articleAddDto);
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
                List<CategoryDto> categories = await _categoryService.GetAllCategoriesNonDeletedAsync();
                return View(new ArticleAddDto { Categories = categories });
            }
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
