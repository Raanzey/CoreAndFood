using CoreAndFood.Data.Models;
using CoreAndFood.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Security.Cryptography.X509Certificates;

namespace CoreAndFood.Controllers
{

    public class CategoryController : Controller
    {
        CategoryRepository categoryRepository = new CategoryRepository();
		public IActionResult Index()
        {
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;
            
            return View(categoryRepository.TList());
        }
        [HttpGet]
        public IActionResult CategoryAdd()
        {
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;
            return View();
        }
        [HttpPost]
        public IActionResult CategoryAdd(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View("CategoryAdd");
            }
            categoryRepository.TAdd(category);
            return RedirectToAction("Index");
        }
        public IActionResult CategoryGet(int id)
        {
            var x = categoryRepository.TGet(id);
            Category ct = new Category()
            {
                Name=x.Name,
                Description=x.Description,
                CategoryID=x.CategoryID
            };
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;
            return View(ct);
        }
        [HttpPost]
        public IActionResult CategoryUpdate(Category parametre)
        {
            var x = categoryRepository.TGet(parametre.CategoryID);
            x.Name = parametre.Name;
            x.Description = parametre.Description;
            x.Status = true;
            categoryRepository.TUpdate(x);
            return RedirectToAction("Index");
        }
        public IActionResult CategoryDelete(int id)
        {
            var x = categoryRepository.TGet(id);
            x.Status = false;
            categoryRepository.TUpdate(x);
            return RedirectToAction("Index");
        }    
    }
}  
