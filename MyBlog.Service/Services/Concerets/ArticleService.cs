using AutoMapper;
using Microsoft.AspNetCore.Http;
using MyBlog.Core.Enums;
using MyBlog.Data.UnitOfWorks;
using MyBlog.Entity.DTOs.Articles;
using MyBlog.Entity.Entities;
using MyBlog.Service.Extensions;
using MyBlog.Service.Services.Abstracts;
using MyBlog.Service.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Services.Concerets
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor; //Kullaniciyi bulma islemi
        private readonly ClaimsPrincipal _user; //Kullaniciyi bulma islemi
        private readonly IImageHelper _imageHelper; //Resim ekleme icin

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
            _imageHelper = imageHelper;
        }

        //Makale Ekleme
        public async Task AddArticleAsync(ArticleAddDto articleAddDto)
        {
            var user = _user.GetLoggedInUserId();
            var userEmail = _user.GetLoggedInEmail();
            var imageUpload = await _imageHelper.Upload(articleAddDto.Title, articleAddDto.Photo, ImageType.Post);
            Image image = new Image(imageUpload.FullName, articleAddDto.Photo.ContentType, userEmail);
            await _unitOfWork.GetRepository<Image>().AddAsync(image);

            var article = new Article(articleAddDto.Title, articleAddDto.Content, user, userEmail, articleAddDto.CategoryId, image.Id);          

            await _unitOfWork.GetRepository<Article>().AddAsync(article);
            await _unitOfWork.SaveAsync();
        }

        //Makale Silme. (Cop kutusuna tasima)
        //Toastr Mesajda basligi donebilmek icin Task<string> eklendi
        public async Task<string> DeleteSafeAsync(Guid articleId)
        {
            var userEmail = _user.GetLoggedInEmail();
            var article = await _unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);

            article.IsDeleted = true;
            article.DeletedDate = DateTime.Now;
            article.DeletedBy = userEmail;

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
            var article = await _unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id == articleId, x => x.Category, i => i.Image);
            var map = _mapper.Map<ArticleDto>(article);

            return map;
        }

        //Makele guncelleme.
        //Toastr Mesajda basligi donebilmek icin Task<string> eklendi
        public async Task<string> UpdateArticleAsync(ArticleUpdateDto articleUpdateDto)
        {
            var userEmail = _user.GetLoggedInEmail();
            var article = await _unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id == articleUpdateDto.Id, x => x.Category, i => i.Image);

            if (articleUpdateDto.Photo != null)
            {
                _imageHelper.Delete(article.Image.FileName);
                var imageUpload = await _imageHelper.Upload(articleUpdateDto.Title, articleUpdateDto.Photo, ImageType.Post);
                Image image = new Image(imageUpload.FullName, articleUpdateDto.Photo.ContentType, userEmail);
                await _unitOfWork.GetRepository<Image>().AddAsync(image);
                article.ImageId = image.Id;
            }

            _mapper.Map(articleUpdateDto, article);
            article.ModifiedBy = userEmail;
            article.ModifiedDate = DateTime.Now;

            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.SaveAsync();

            return article.Title;
        }
    }
}
