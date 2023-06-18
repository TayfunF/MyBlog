using Microsoft.Extensions.DependencyInjection;
using MyBlog.Service.Services.Abstracts;
using MyBlog.Service.Services.Concerets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Extensions
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection LoadServiceLayerExtensions(this IServiceCollection services)
        {
            services.AddScoped<IArticleService, ArticleService>(); //Article Service
            services.AddScoped<ICategoryService, CategoryService>(); //Category Service

            var assembly = Assembly.GetExecutingAssembly(); //AutoMapper Service
            services.AddAutoMapper(assembly); //AutoMapper Service
            return services;
        }
    }
}
