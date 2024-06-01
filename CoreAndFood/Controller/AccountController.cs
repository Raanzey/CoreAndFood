using CoreAndFood.Data.Models;
using CoreAndFood.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreAndFood.Controllers
{      
    public class AccountController : Controller
    {
        AccountRepository accountRepository = new AccountRepository();
        Context c = new Context();

        [AllowAnonymous]
        [HttpGet]  	
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Account UserValue)
        {
            var datavalue = c.Accounts.FirstOrDefault(x => x.UserName == UserValue.UserName && x.Password == UserValue.Password);
            if (datavalue != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserValue.UserName)
                };
                var useridentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Category");
            }
            else
            {
                ViewBag.Error = "Username or password is incorrect";
                return View();
            }           
        }

		[HttpGet]
        public async Task<IActionResult> Logout()
        {
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }   

        public IActionResult UserList()
        {
            var user_Name = User.Identity.Name;

			var userRole = c.Accounts.FirstOrDefault(x => x.UserName == user_Name)?.Role;

			if (userRole != "Admin")
			{
				return RedirectToAction("Personel", "Account");
			}
            
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;

            TempData["UserRole"] = userRole;
            return View(accountRepository.TList());
		}


        [HttpGet]
		public IActionResult NewLogin()
		{
            return View();
		}
        [HttpPost]
		public async Task<IActionResult> NewLogin(Account NewAccount)
		{

			Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");
			if (!regex.IsMatch(NewAccount.Password))
			{
				ModelState.AddModelError("Password", "Your password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, one number and one special character.");
				return View(NewAccount);
			}

			var user = c.Accounts.FirstOrDefault(x => x.UserName == NewAccount.UserName && x.Password == NewAccount.Password && x.Email == NewAccount.Email);
			if (user != null)
			{
				ViewBag.NewAccountError = "The information you enter is recorded, please check your information!";
				return View();
			}

			c.Accounts.Add(NewAccount);
			c.SaveChanges();

			return View();
		}

        public IActionResult Personel()
        {
            var userName = User.Identity.Name;
            var user = accountRepository.TList().FirstOrDefault(x => x.UserName == userName);

            if (user == null)
            {
                return NotFound();
            }

            TempData["UserName"] = userName;

            return View(user);
        }

        //public IActionResult PersonelGet() { }

        public IActionResult PersonelGet(int id)
        {
            var x = accountRepository.TGet(id);
            Account ct = new ()
            {
                AdminID = x.AdminID,
                UserName = x.UserName,
                Email = x.Email,
                Password = x.Password
            };
            return View(ct);
        }
        [HttpPost]
        public IActionResult PersonelUpdate(Account parametre)
        {

            var x = accountRepository.TGet(parametre.AdminID);
            x.UserName = parametre.UserName;
            x.Email = parametre.Email; 
            x.Password = parametre.Password;

            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");
            if (!regex.IsMatch(parametre.Password))
            {
                ModelState.AddModelError("Password", "Your password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, one number and one special character.");
                return View(parametre);
            }
            accountRepository.TUpdate(x);
            return RedirectToAction("UserList");
        }

	}
}
