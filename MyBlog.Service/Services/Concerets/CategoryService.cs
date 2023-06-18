using AutoMapper;
using MyBlog.Data.UnitOfWorks;
using MyBlog.Entity.DTOs.Categories;
using MyBlog.Entity.Entities;
using MyBlog.Service.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Services.Concerets
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesNonDeletedAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(x => !x.IsDeleted);
            var map = _mapper.Map<List<CategoryDto>>(categories);

            return map;
        }
    }
}
