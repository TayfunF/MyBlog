using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Core.Enums;
using MyBlog.Core.Utils;
using MyBlog.Data.UnitOfWorks;
using MyBlog.Entity.DTOs.Users;
using MyBlog.Entity.Entities;
using MyBlog.Service.Extensions;
using MyBlog.Service.Services.Abstracts;
using MyBlog.Service.Services.Helpers;
using NToastNotify;
using System.Linq;

namespace tayfunfirtina.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        private readonly IValidator<AppUser> _validator;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IMapper mapper, IToastNotification toastNotification, IValidator<AppUser> validator)
        {
            _userService = userService;
            _mapper = mapper;
            _toastNotification = toastNotification;
            _validator = validator;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userService.GetAllUsersWithRoleAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await _userService.GetAllRolesAsync();
            return View(new UserAddDto { Roles = roles });
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            var map = _mapper.Map<AppUser>(userAddDto);
            var validation = await _validator.ValidateAsync(map);
            var roles = await _userService.GetAllRolesAsync();

            if (ModelState.IsValid)
            {
                var result = await _userService.AddUserAsync(userAddDto);
                if (result.Succeeded)
                {
                    _toastNotification.AddSuccessToastMessage(ToastrMessages.UserMessage.AddMessage(userAddDto.Email), new ToastrOptions { Title = "Başarılı !" });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    result.AddToIdentityModelState(this.ModelState);//Extension
                    validation.AddToModelState(this.ModelState);
                    return View(new UserAddDto { Roles = roles });
                }
            }

            return View(new UserAddDto { Roles = roles });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            var roles = await _userService.GetAllRolesAsync();

            var map = _mapper.Map<UserUpdateDto>(user);
            map.Roles = roles;

            return View(map);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            var user = await _userService.GetUserByIdAsync(userUpdateDto.Id);

            if (user != null)
            {
                var roles = await _userService.GetAllRolesAsync();
                if (ModelState.IsValid)
                {
                    var map = _mapper.Map(userUpdateDto, user);
                    var validation = await _validator.ValidateAsync(map);

                    if (validation.IsValid)
                    {
                        user.UserName = userUpdateDto.Email;
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        var result = await _userService.UpdateUserAsync(userUpdateDto);
                        if (result.Succeeded)
                        {
                            _toastNotification.AddSuccessToastMessage(ToastrMessages.UserMessage.UpdateMessage(userUpdateDto.Email), new ToastrOptions { Title = "Başarılı !" });
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }
                        else
                        {
                            result.AddToIdentityModelState(this.ModelState);//Extension
                            return View(new UserUpdateDto { Roles = roles });
                        }
                    }
                    else
                    {
                        validation.AddToModelState(this.ModelState);
                        return View(new UserUpdateDto { Roles = roles });
                    }
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (result.identityResult.Succeeded)
            {
                _toastNotification.AddSuccessToastMessage(ToastrMessages.UserMessage.DeleteMessage(result.email), new ToastrOptions { Title = "Başarılı !" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else
            {
                result.identityResult.AddToIdentityModelState(this.ModelState);//Extension
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            return View(await _userService.GetUserProfileAsync());
        }

        [HttpPost]
        public async Task<IActionResult> MyProfile(UserProfileDto userProfileDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UserProfileUpdateAsync(userProfileDto);
                if (result)
                {
                    _toastNotification.AddSuccessToastMessage("Profil güncelleme işlemi tamamlandı", new ToastrOptions { Title = "Başarılı !" });
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
                else
                {
                    var profile = await _userService.GetUserProfileAsync();
                    _toastNotification.AddErrorToastMessage("Profil güncelleme işlemi tamamlanamadı", new ToastrOptions { Title = "İşlem başarısız !" });
                    return View(profile);
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}