using FluentValidation;

namespace CoreAndFood.Data.Models.Validators
{
    public class CategoryValidators : AbstractValidator<Category>
    {
        public CategoryValidators()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty").NotNull().WithMessage("Name cannot be empty");
            RuleFor(x => x.Name).MinimumLength(4).WithMessage("Must contain at least 4 characters").MaximumLength(20).WithMessage("Must contain at most 20 characters");
        }
    }
}
