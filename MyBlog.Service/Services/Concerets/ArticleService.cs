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
            var appUserId = Guid.Parse("2C34DA79-F839-4AA8-95DE-1D31A3B39C28");
            var imageId = Guid.Parse("F71F4B9A-AA60-461D-B398-DE31001BF214");
            var article = new Article(articleAddDto.Title, articleAddDto.Content, appUserId, articleAddDto.CategoryId, imageId);
            await _unitOfWork.GetRepository<Article>().AddAsync(article);
            await _unitOfWork.SaveAsync();
        }

        //Makale Silme. (Cop kutusuna tasima)
        //Toastr Mesajda basligi donebilmek icin Task<string> eklendi
        public async Task<string> DeleteSafeAsync(Guid articleId)
        {
            var article = await _unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);

            article.IsDeleted = true;
            article.DeletedDate = DateTime.Now;

            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.SaveAsync();

            return article.Title;
        }

        //Silinmemis makaleleri kategorileri ile beraber getir.
        public async Task<List<ArticleDto>> GetAllArticlesWithCategoryNonDeletedAsync()
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(a => !a.IsDeleted, b => b.Category);
            var map = _mapper.Map<List<ArticleDto>>(articles);

            return map;
        }

        //Silinmemis makaleyi kategorisi ile beraber getir.
        public async Task<ArticleDto> GetArticleWithCategoryNonDeletedAsync(Guid articleId)
        {
            var article = await _unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id == articleId, x => x.Category);
            var map = _mapper.Map<ArticleDto>(article);

            return map;
        }

        //Makele guncelleme.
        //Toastr Mesajda basligi donebilmek icin Task<string> eklendi
        public async Task<string> UpdateArticleAsync(ArticleUpdateDto articleUpdateDto)
        {
            var article = await _unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id == articleUpdateDto.Id, x => x.Category);

            if (article != null)
            {
                article.ModifiedBy = "undefined";
                article.ModifiedDate = DateTime.Now;

                _mapper.Map(articleUpdateDto, article);

                await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
                await _unitOfWork.SaveAsync();

                return article.Title;
            }
            else
            {
                return "";
            }
        }
    }
}
