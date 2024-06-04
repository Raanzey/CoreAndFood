using CoreAndFood.Data.Models;
using CoreAndFood.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreAndFood.Controllers
{
	public class BaseController : Controller
	{
		Context c = new Context();
		public string User_Name()
		{
			var user_Name = HttpContext.User.Identity.Name;
			return user_Name;
		}
		public int UserID()
		{
			var user_Name = User_Name();
			var userId = c.Accounts.Where(x => x.UserName == user_Name).Select(x => x.AccountID).FirstOrDefault();

			return userId;
		}

	}
}
