using MyBlog.Entity.DTOs.Categories;
using MyBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Services.Abstracts
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesNonDeletedAsync();
        Task<List<CategoryDto>> GetAllCategoriesDeletedAsync();
        Task AddCategoryAsync(CategoryAddDto categoryAddDto);
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        Task<string> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
        Task<string> DeleteSafeCategoryAsync(Guid categoryId);
        Task<string> DeleteUndoCategoryAsync(Guid categoryId);
        Task<List<CategoryDto>> GetAllCategoriesNonDeletedTake24Async();
    }
}
