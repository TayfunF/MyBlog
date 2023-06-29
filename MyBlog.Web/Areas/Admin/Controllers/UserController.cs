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
using MyBlog.Service.Services.Helpers;
using NToastNotify;
using System.Linq;

namespace tayfunfirtina.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IToastNotification _toastNotification;
        private readonly IValidator<AppUser> _validator;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IImageHelper _imageHelper;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(UserManager<AppUser> userManager, IMapper mapper, RoleManager<AppRole> roleManager, IToastNotification toastNotification, IValidator<AppUser> validator, SignInManager<AppUser> signInManager, IImageHelper imageHelper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _toastNotification = toastNotification;
            _validator = validator;
            _signInManager = signInManager;
            _imageHelper = imageHelper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
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

            return View(map);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(new UserAddDto { Roles = roles });
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            var map = _mapper.Map<AppUser>(userAddDto);
            var validation = await _validator.ValidateAsync(map);
            var roles = await _roleManager.Roles.ToListAsync();

            if (ModelState.IsValid)
            {
                map.UserName = userAddDto.Email;
                var result = await _userManager.CreateAsync(map, String.IsNullOrEmpty(userAddDto.Password) ? "" : userAddDto.Password);
                if (result.Succeeded)
                {
                    var findRole = await _roleManager.FindByIdAsync(userAddDto.RoleId.ToString());
                    await _userManager.AddToRoleAsync(map, findRole.ToString());
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
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roles = await _roleManager.Roles.ToListAsync();

            var map = _mapper.Map<UserUpdateDto>(user);
            map.Roles = roles;

            return View(map);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            var user = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());

            if (user != null)
            {
                var userRole = string.Join("~", await _userManager.GetRolesAsync(user));
                var roles = await _roleManager.Roles.ToListAsync();
                if (ModelState.IsValid)
                {
                    var map = _mapper.Map(userUpdateDto, user);
                    var validation = await _validator.ValidateAsync(map);

                    if (validation.IsValid)
                    {
                        user.UserName = userUpdateDto.Email;
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            await _userManager.RemoveFromRoleAsync(user, userRole);
                            var findRole = await _roleManager.FindByIdAsync(userUpdateDto.RoleId.ToString());
                            await _userManager.AddToRoleAsync(user, findRole.Name);
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
            var user = await _userManager.FindByIdAsync(userId.ToString());

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _toastNotification.AddSuccessToastMessage(ToastrMessages.UserMessage.DeleteMessage(user.Email), new ToastrOptions { Title = "Başarılı !" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else
            {
                result.AddToIdentityModelState(this.ModelState);//Extension
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var getImage = await _unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == user.Id, x => x.Image);
            var map = _mapper.Map<UserProfileDto>(user);
            map.Image.FileName = getImage.Image.FileName;

            return View(map);
        }

        [HttpPost]
        public async Task<IActionResult> MyProfile(UserProfileDto userProfileDto)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (ModelState.IsValid)
            {
                var isVerified = await _userManager.CheckPasswordAsync(user, userProfileDto.CurrentPassword);
                if (isVerified && userProfileDto.NewPassword != null && userProfileDto.Photo != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, userProfileDto.CurrentPassword, userProfileDto.NewPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user, userProfileDto.NewPassword, true, false);

                        user.FirstName = userProfileDto.FirstName;
                        user.LastName = userProfileDto.LastName;
                        user.PhoneNumber = userProfileDto.PhoneNumber;

                        var imageUpload = await _imageHelper.Upload($"{userProfileDto.FirstName} {userProfileDto.LastName}", userProfileDto.Photo, ImageType.User);
                        Image image = new Image(imageUpload.FullName, userProfileDto.Photo.ContentType, user.Email);
                        await _unitOfWork.GetRepository<Image>().AddAsync(image);

                        user.ImageId = image.Id;

                        await _userManager.UpdateAsync(user);

                        await _unitOfWork.SaveAsync();

                        _toastNotification.AddSuccessToastMessage("Şifreniz ve bilgileriniz başarıyla değiştirdi.");
                        return View();
                    }
                    else
                    {
                        result.AddToIdentityModelState(this.ModelState);
                        return View();
                    }
                }
                else if (isVerified && userProfileDto.Photo != null)
                {
                    await _userManager.UpdateSecurityStampAsync(user);

                    user.FirstName = userProfileDto.FirstName;
                    user.LastName = userProfileDto.LastName;
                    user.PhoneNumber = userProfileDto.PhoneNumber;

                    var imageUpload = await _imageHelper.Upload($"{userProfileDto.FirstName} {userProfileDto.LastName}", userProfileDto.Photo, ImageType.User);
                    Image image = new Image(imageUpload.FullName, userProfileDto.Photo.ContentType, user.Email);
                    await _unitOfWork.GetRepository<Image>().AddAsync(image);

                    user.ImageId = image.Id;

                    await _userManager.UpdateAsync(user);

                    await _unitOfWork.SaveAsync();

                    _toastNotification.AddSuccessToastMessage("Bilgileriniz başarıyla değiştirdi.");
                    return View();
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Bilgileriniz güncellenirken hata oluştu.");
                    return View(userProfileDto);
                }
            }

            return View();
        }
    }
}
