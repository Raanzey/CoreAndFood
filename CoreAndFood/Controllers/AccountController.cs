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
    public class AccountController : BaseController
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
            var userRole = c.Accounts.FirstOrDefault(x => x.UserName == User_Name())?.Role;

			if (userRole != "Admin")
			{
				return RedirectToAction("PersonelDetails", "Account");
			}

            TempData["UserName"] = User_Name();
            TempData["accountID"] = UserID();
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

			if (Core.User.Control(NewAccount.Password) != null)
			{
				ModelState.AddModelError("Password", Core.User.Control(NewAccount.Password));
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
            TempData["newUserSucces"] = "New user created successfully";
            return View();
		}


        public IActionResult PersonelDetails()
        {
 
            var user = accountRepository.TList().FirstOrDefault(x => x.UserName == User_Name());

            if (user == null)
            {
                return NotFound();
            }

            TempData["UserName"] = User_Name();

            return View(user);
        }


        public IActionResult AccountGet(int id)
        {
            var x = accountRepository.TGet(id);
            Account account = new ()
            {
                AccountID = x.AccountID,
                UserName = x.UserName,
                Email = x.Email,
                Password = x.Password
            };
			TempData["UserName"] = User_Name();
			return View(account);
        }


        [HttpPost]
        public IActionResult AccountGet(Account parametre)
        {
            var userId = UserID();

			if (Core.User.Control(parametre.Password)!=null)
			{
				ModelState.AddModelError("Password", Core.User.Control(parametre.Password));
				return View(parametre);
			}

			var user = accountRepository.TGet(userId);
            user.UserName = parametre.UserName;
            user.Email = parametre.Email;
            user.Password = parametre.Password;

            accountRepository.TUpdate(user);
            ViewBag.successful = "Registration was successful";
			TempData["UserName"] = User_Name();
			return View();
        }



	}
}
