using Microsoft.AspNetCore.Http;
using MyBlog.Core.Enums;
using MyBlog.Entity.DTOs.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Services.Helpers
{
    public interface IImageHelper
    {
        Task<ImageUploadDto> Upload(string name, IFormFile imageFile, ImageType imageType, string folderName = null);
        void Delete(string imageName);
    }
}
