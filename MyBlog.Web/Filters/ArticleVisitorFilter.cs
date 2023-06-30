using Microsoft.AspNetCore.Mvc.Filters;
using MyBlog.Data.UnitOfWorks;
using MyBlog.Entity.Entities;

namespace MyBlog.Web.Filters
{
    public class ArticleVisitorFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleVisitorFilter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var visitors = _unitOfWork.GetRepository<Visitor>().GetAllAsync().Result;

            string getIp = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            string getUserAgent = context.HttpContext.Request.Headers["User-Agent"];

            Visitor visitor = new Visitor(getIp, getUserAgent);

            if (visitors.Any(x => x.IpAddress == visitor.IpAddress))
            {
                return next();
            }
            else
            {
                _unitOfWork.GetRepository<Visitor>().AddAsync(visitor);
                _unitOfWork.Save();
            }

            return next();
        }
    }
}
