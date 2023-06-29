using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.UnitOfWorks;
using MyBlog.Entity.DTOs.Users;
using MyBlog.Entity.Entities;
using MyBlog.Service.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Services.Concerets
{
    internal class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserDto>> GetAllUsersWithRoleAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var map = _mapper.Map<List<UserDto>>(users);

            //Rolü bulabilmek için döngü ile tüm userları bulmamız lazım ki id sine göre rol bulayım.
            foreach (var item in map)
            {
                var findUser = await _userManager.FindByIdAsync(item.Id.ToString());
                var role = string.Join("~", await _userManager.GetRolesAsync(findUser)); //superadmin~admin~normaluser..... diye ayırıyor.

                item.Role = role;
            }

            return map;
        }

        public async Task<List<AppRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> AddUserAsync(UserAddDto userAddDto)
        {
            var map = _mapper.Map<AppUser>(userAddDto);
            map.UserName = userAddDto.Email;
            var result = await _userManager.CreateAsync(map, String.IsNullOrEmpty(userAddDto.Password) ? "" : userAddDto.Password);
            if (result.Succeeded)
            {
                var findRole = await _roleManager.FindByIdAsync(userAddDto.RoleId.ToString());
                await _userManager.AddToRoleAsync(map, findRole.ToString());
                return result;
            }
            else
            {
                return result;
            }
        }
    }
}
