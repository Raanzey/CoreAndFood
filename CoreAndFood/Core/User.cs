using System.Text.RegularExpressions;

namespace CoreAndFood.Core
{
	public static class User
	{
		public static string Control(string password)
		{
			Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");
			if (!regex.IsMatch(password))
			{
				return "Your password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, one number and one special character.";

			}
			return null;
		}
	}
}
