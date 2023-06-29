using FluentValidation;
using MyBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.FluentValidations
{
    public class UserValidator : AbstractValidator<AppUser>
    {
        public UserValidator()
        {
            RuleFor(a => a.FirstName).NotEmpty().NotNull().MinimumLength(3).MaximumLength(150).WithName("Ad");
            RuleFor(a => a.LastName).NotEmpty().NotNull().MinimumLength(3).MaximumLength(150).WithName("Soyad");
            RuleFor(a => a.Email).NotEmpty().NotNull().MinimumLength(3).MaximumLength(150).WithName("E-posta");
        }
    }
}
