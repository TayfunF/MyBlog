using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBlog.Entity.DTOs.Users
{
    public class UserResetPasswordDto
    {
        [Display(Name = "E-posta Adresi :")]
        [Required(ErrorMessage = "E-posta alanı zorunludur !")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Yeni Şifre :")]
        [Required(ErrorMessage = "Yeni şifre alanı boş bırakılamaz !")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Yeni şifre en az 4 karakter olmalıdır !")]
        public string PasswordNew { get; set; }
    }
}
