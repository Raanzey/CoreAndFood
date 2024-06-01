using FluentValidation;
using System.Globalization;
using System;

namespace CoreAndFood.Data.Models.Validators
{
    public class FoodValidators : AbstractValidator<Food>
    {
        public FoodValidators()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be empty")
                .NotNull().WithMessage("Name cannot be empty")
                .MinimumLength(2).WithMessage("Must contain at least 2 characters")
                .MaximumLength(20).WithMessage("Must contain at most 15 characters");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Name cannot be empty")
                .NotNull().WithMessage("Name cannot be empty")
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .Must(BeAValidPrice).WithMessage("Price must be a valid number with up to two decimal places");

            RuleFor(x => x.Stock)
                .NotEmpty().WithMessage("Name cannot be empty")
                .NotNull().WithMessage("Name cannot be empty")
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Name cannot be empty")
                .NotNull().WithMessage("Name cannot be empty");
        }

        private bool BeAValidPrice(double price)
        {
			string priceString = price.ToString(CultureInfo.InvariantCulture);
			return decimal.TryParse(priceString, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result) &&
				   result == Math.Round(result, 2);
		}
    }
}
