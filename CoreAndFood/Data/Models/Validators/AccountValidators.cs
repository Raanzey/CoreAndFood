using FluentValidation;

namespace CoreAndFood.Data.Models.Validators
{
	public class AccountValidators : AbstractValidator<Account>
	{
		public AccountValidators() 
		{
			RuleFor(x => x.UserName)
				.NotEmpty().WithMessage("Name cannot be empty")
				.NotNull().WithMessage("Name cannot be empty")
				.MaximumLength(20).WithMessage("Must contain at most 20 characters")
				.MinimumLength(3).WithMessage("Must contain at least 3 characters");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Name cannot be empty")
				.NotNull().WithMessage("Name cannot be empty")
				.EmailAddress().WithMessage("Please enter in Email format");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Name cannot be empty")
				.NotNull().WithMessage("Name cannot be empty")
				.MaximumLength(20).WithMessage("Must contain at most 20 characters")
				.MinimumLength(4).WithMessage("Must contain at least 4 characters");

			RuleFor(x => x.Role)
				.NotEmpty().WithMessage("Name cannot be empty")
				.NotNull().WithMessage("Name cannot be empty");	
		}

	}
}
