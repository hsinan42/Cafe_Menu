using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Ürün adını boş geçemezsiniz")
                .MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın")
                .MaximumLength(50).WithMessage("Lütfen 50 karakterden fazla giriş yapmayın");

            RuleFor(x => x.ProductDescription).MaximumLength(150).WithMessage("Lütfen 150 karakterden fazla giriş yapmayın");

            RuleFor(x => x.ProductImage).MaximumLength(150).WithMessage("Lütfen 150 karakterden fazla giriş yapmayın");

            RuleFor(x => x.ProductPrice)
                .NotEmpty().WithMessage("Fiyat boş olamaz.")
                .GreaterThanOrEqualTo(0).WithMessage("Fiyat negatif olamaz.")
                .Must(BeValidDecimal).WithMessage("Fiyat en fazla 9999.99 olabilir.");
        }
        private bool BeValidDecimal(decimal price)
        {
            return price >= 0 && price < 10000 && Decimal.Round(price, 2) == price;
        }
    }
}
