using Microsoft.AspNetCore.Identity;
using MyBlog.Entity.DTOs.Users;
using MyBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Services.Abstracts
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersWithRoleAsync();
        Task<List<AppRole>> GetAllRolesAsync();
        Task<IdentityResult> AddUserAsync(UserAddDto userAddDto);
    }
}
