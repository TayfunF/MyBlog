using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Core.Utils;
using MyBlog.Entity.DTOs.Categories;
using MyBlog.Entity.Entities;
using MyBlog.Service.Extensions;
using MyBlog.Service.Services.Abstracts;
using NToastNotify;
using System.Runtime.CompilerServices;

namespace MyBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<Category> _validator;
        private readonly IToastNotification _toastNotification;

        public CategoryController(ICategoryService categoryService, IMapper mapper, IValidator<Category> validator, IToastNotification toastNotification)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _validator = validator;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAllCategoriesNonDeletedAsync());
        }

        [HttpGet]
        public async Task<IActionResult> DeletedCategory()
        {
            return View(await _categoryService.GetAllCategoriesDeletedAsync());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            var map = _mapper.Map<Category>(categoryAddDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await _categoryService.AddCategoryAsync(categoryAddDto);
                _toastNotification.AddSuccessToastMessage(ToastrMessages.CategoryMessage.AddMessage(categoryAddDto.Name), new ToastrOptions { Title = "Başarılı !" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }

            result.AddToModelState(this.ModelState);
            return View(categoryAddDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddWithAjax([FromBody] CategoryAddDto categoryAddDto)
        {
            var map = _mapper.Map<Category>(categoryAddDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await _categoryService.AddCategoryAsync(categoryAddDto);
                _toastNotification.AddSuccessToastMessage(ToastrMessages.CategoryMessage.AddMessage(categoryAddDto.Name), new ToastrOptions { Title = "İşlem Başarılı" });

                return Json(ToastrMessages.CategoryMessage.AddMessage(categoryAddDto.Name));
            }
            else
            {
                _toastNotification.AddErrorToastMessage(result.Errors.First().ErrorMessage, new ToastrOptions { Title = "İşlem Başarısız" });
                return Json(result.Errors.First().ErrorMessage);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid categoryId)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);
            var map = _mapper.Map<Category, CategoryUpdateDto>(category);
            return View(map);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            var map = _mapper.Map<Category>(categoryUpdateDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var name = await _categoryService.UpdateCategoryAsync(categoryUpdateDto);
                _toastNotification.AddSuccessToastMessage(ToastrMessages.CategoryMessage.UpdateMessage(name), new ToastrOptions { Title = "Başarılı !" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }

            result.AddToModelState(this.ModelState);
            return View(categoryUpdateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var title = await _categoryService.DeleteSafeCategoryAsync(categoryId);
            _toastNotification.AddWarningToastMessage(ToastrMessages.CategoryMessage.DeleteMessage(title), new ToastrOptions { Title = "Başarılı !" });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> UndoDelete(Guid categoryId)
        {
            var title = await _categoryService.DeleteUndoCategoryAsync(categoryId);
            _toastNotification.AddWarningToastMessage(ToastrMessages.CategoryMessage.UndoDeleteMessage(title), new ToastrOptions { Title = "Başarılı !" });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }
    }
}
