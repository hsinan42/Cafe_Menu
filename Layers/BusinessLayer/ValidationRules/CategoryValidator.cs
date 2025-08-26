using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class CategoryValidator:AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori adını boş geçemezsiniz")
                .MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın")
                .MaximumLength(50).WithMessage("Lütfen 100 karakterden fazla giriş yapmayın");

            RuleFor(x => x.CategoryDescription).MaximumLength(150).WithMessage("Lütfen 150 karakterden fazla giriş yapmayın");

            RuleFor(x => x.CategoryImage).MaximumLength(150).WithMessage("Maksimum dosya yolu uzunluğu 150 karakter olmalı");
        }
    }
}
