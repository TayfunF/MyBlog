using AutoMapper;
using Microsoft.AspNetCore.Http;
using MyBlog.Data.UnitOfWorks;
using MyBlog.Entity.DTOs.Categories;
using MyBlog.Entity.Entities;
using MyBlog.Service.Extensions;
using MyBlog.Service.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Services.Concerets
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
        }

        //Kategori Ekleme
        public async Task AddCategoryAsync(CategoryAddDto categoryAddDto)
        {
            var userEmail = _user.GetLoggedInEmail();
            var category = new Category(categoryAddDto.Name, userEmail);
            await _unitOfWork.GetRepository<Category>().AddAsync(category);
            await _unitOfWork.SaveAsync();
        }

        //Kategori Silme (Cop Kutusuna Tasima Islemi)
        public async Task<string> DeleteSafeAsync(Guid categoryId)
        {
            var userEmail = _user.GetLoggedInEmail();
            var category = await _unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);

            category.IsDeleted = true;
            category.DeletedDate = DateTime.Now;
            category.DeletedBy = userEmail;

            await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();

            return category.Name;
        }

        //Silinmemis kategorileri getir
        public async Task<List<CategoryDto>> GetAllCategoriesNonDeletedAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(x => !x.IsDeleted);
            var map = _mapper.Map<List<CategoryDto>>(categories);

            return map;
        }

        //Istenen Kategoriyi Getir
        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            return await _unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);

        }

        //Kategori Guncelle
        public async Task<string> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto)
        {
            var userEmail = _user.GetLoggedInEmail();
            var category = await _unitOfWork.GetRepository<Category>().GetAsync(x => !x.IsDeleted && x.Id == categoryUpdateDto.Id);

            category.Name = categoryUpdateDto.Name;
            category.ModifiedDate = DateTime.Now;
            category.ModifiedBy = userEmail;

            await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();

            return category.Name;
        }
    }
}
