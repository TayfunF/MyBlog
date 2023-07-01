using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Core.Utils;
using MyBlog.Entity.DTOs.Users;
using MyBlog.Entity.Entities;
using NToastNotify.Helpers;

namespace MyBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if (user != null)
                {
                    //Eger kullanici giris yaparken 'Beni Hatirla' yi isaretlediyse 'Startup.cs' ye gidicek ve kullanicinin bilgilerini 7 gun tutacak.
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Default", new { Area = "Admin" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-posta adresi veya şifre hatalı.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-posta adresi veya şifre hatalı.");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError(nameof(userLoginDto.Email), "Kullanıcı adı ya da şifre yanlış !");
            }

            return View(userLoginDto);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Auth", new { Area = "Admin" });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            TempData["durum"] = null;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(UserResetPasswordDto userResetPasswordDto)
        {
            if (TempData["durum"] == null)
            {
                AppUser appUser = _userManager.FindByEmailAsync(userResetPasswordDto.Email).Result;
                if (appUser != null)
                {
                    //User bilgilerinden olusan bir token olusturuyor.
                    string passwordResetToken = _userManager.GeneratePasswordResetTokenAsync(appUser).Result;
                    string passwordResetLink = Url.Action("ResetPasswordConfirm", "Auth", new
                    {
                        userId = appUser.Id,
                        token = passwordResetToken
                    }, HttpContext.Request.Scheme);

                    PasswordResetHelper.PasswordResetSendEmail(passwordResetLink, appUser.Email);
                    ViewBag.status = "success";
                    TempData["durum"] = true.ToString();
                }
                else
                {
                    ModelState.AddModelError("", "Sistemde kayıtlı e-posta adresi bulunamadı !");
                }
                return View(userResetPasswordDto);
            }
            else
            {
                return RedirectToAction("ResetPassword");
            }
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirm(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        //Bind => Modelimden kullanmak istedigim proplarim gelsin. Diger proplar gelmesin.
        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")] UserResetPasswordDto userResetPasswordDto)
        {
            string userId = TempData["userId"].ToString();
            string token = TempData["token"].ToString();
            AppUser appUser = await _userManager.FindByIdAsync(userId);

            if (appUser != null)
            {
                IdentityResult result = await _userManager.ResetPasswordAsync(appUser, token, userResetPasswordDto.PasswordNew);

                if (result.Succeeded)
                {
                    //Bunu yazmazsak kullanici eski sifresi ile dolanmaya devam eder.
                    await _userManager.UpdateSecurityStampAsync(appUser);
                    ViewBag.status = "success";
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Hata meydana geldi. Lütfen daha sonra tekrar deneyin !");
            }
            return View(userResetPasswordDto);
        }
    }
}
