using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Service.FluentValidations;
using MyBlog.Service.Services.Abstracts;
using MyBlog.Service.Services.Concerets;
using MyBlog.Service.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            services.AddScoped<IImageHelper, ImageHelper>(); // ImageUpload Service
            services.AddScoped<IUserService,UserService>(); //User Service
            services.AddScoped<IDashboardService, DashboardService>(); //Dashboard Service

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //ClaimsPrincipal Kullaniciyi bulma servisi

            var assembly = Assembly.GetExecutingAssembly(); //AutoMapper Service
            services.AddAutoMapper(assembly); //AutoMapper Service

            services.AddControllersWithViews().AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
                opt.DisableDataAnnotationsValidation = true;
                opt.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
            });

            return services;
        }
    }
}
