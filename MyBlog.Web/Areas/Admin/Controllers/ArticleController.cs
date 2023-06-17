using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Web.Areas.Admin.Controllers
{
    public class ArticleController : Controller
    {
        [Area("Admin")]
        [Route("Admin/[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
