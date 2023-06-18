using AutoMapper;
using MyBlog.Data.UnitOfWorks;
using MyBlog.Entity.DTOs.Articles;
using MyBlog.Entity.Entities;
using MyBlog.Service.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Services.Concerets
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Makale Ekleme
        public async Task AddArticleAsync(ArticleAddDto articleAddDto)
        {
            var map = _mapper.Map<Article>(articleAddDto);
            await _unitOfWork.GetRepository<Article>().AddAsync(map);
            await _unitOfWork.SaveAsync();
        }

        //Silinmemis makaleleri kategorileri ile beraber getir.
        public async Task<List<ArticleDto>> GetAllArticlesWithCategoryNonDeletedAsync()
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(a => !a.IsDeleted, b => b.Category);
            var map = _mapper.Map<List<ArticleDto>>(articles);

            return map;
        }
    }
}
