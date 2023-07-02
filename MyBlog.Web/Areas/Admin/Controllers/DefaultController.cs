using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Services.Abstracts;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace MyBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class DefaultController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IDashboardService _dashboardService;
        private readonly IUserService _userService;

        public DefaultController(IArticleService articleService, IDashboardService dashboardService, IUserService userService)
        {
            _articleService = articleService;
            _dashboardService = dashboardService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.fullName = await _userService.GetUsername();

            return View(await _articleService.GetAllArticlesWithCategoryNonDeletedAsync());
        }

        [HttpGet]
        public async Task<IActionResult> YearlyArticleCounts()
        {
            var count = await _dashboardService.GetYearlyArticleCountsAsync();
            return Json(JsonConvert.SerializeObject(count));
        }

        [HttpGet]
        public async Task<IActionResult> TotalCategoryCount()
        {
            var count = await _dashboardService.TotalCategoryCountAsync();
            return Json(count);
            //return Json(JsonConvert.SerializeObject(count));
        }

        [HttpGet]
        public async Task<IActionResult> TotalArticleCount()
        {
            var count = await _dashboardService.TotalArticleCountAsync();
            return Json(count);
            //return Json(JsonConvert.SerializeObject(count));
        }
    }
}
