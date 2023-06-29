using FluentValidation;
using MyBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.FluentValidations
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(a => a.Name).NotEmpty().NotNull().MinimumLength(3).MaximumLength(150).WithName("Kategori");
        }
    }
}
