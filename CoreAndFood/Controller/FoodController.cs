using CoreAndFood.Data.Models;
using CoreAndFood.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using X.PagedList;

namespace CoreAndFood.Controllers
{

	public class FoodController : Controller
    {
        FoodRepository foodRepository = new FoodRepository();
        Context c = new Context();
		public IActionResult Index(int page = 1)
        {
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;
            return View(foodRepository.TList("Category").ToPagedList(page,3));
        }
        private void PopulateCategoryList()
        {
            List<SelectListItem> categoryList = (from x in c.Categories.Where(c => c.Status == true).ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.Name,
                                                     Value = x.CategoryID.ToString()
                                                 }).ToList();
            ViewBag.category_List = categoryList;
        }
        [HttpGet]
        public IActionResult AddFood()
        {      
            PopulateCategoryList();

            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;

            return View();
        }
        [HttpPost]
        public IActionResult AddFood(Food food)
        {
            //if (!ModelState.IsValid)
            //{
            //    PopulateCategoryList();
            //    return View("AddFood");
            //}
            foodRepository.TAdd(food);
            return RedirectToAction("Index");
        }
        public IActionResult FoodDelete(int id)
        {
            foodRepository.TDelete(new Food { FoodID = id });
            return RedirectToAction("Index");
        }
        public IActionResult FoodGet(int id)
        {
            var x = foodRepository.TGet(id);
            List<SelectListItem> categoryList = (from y in c.Categories.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = y.Name,
                                                     Value = y.CategoryID.ToString()
                                                 }).ToList();
            ViewBag.category_List = categoryList;
            Food food = new Food()
            {
                FoodID = x.FoodID,
                CategoryID = x.CategoryID,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock,
                Description = x.Description,
                ImageURL = x.ImageURL
            };
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;
            return View(food);
        }
        [HttpPost]
        public IActionResult FoodUpdate(Food parametre)
        {
            parametre.Price = Convert.ToDouble(parametre.Price.ToString().Replace(",", "."), CultureInfo.InvariantCulture);

            var x = foodRepository.TGet(parametre.FoodID);
            x.Name = parametre.Name;
            x.Price = parametre.Price;
            x.Stock = parametre.Stock; 
            x.Description = parametre.Description;
            x.ImageURL = parametre.ImageURL;
            x.CategoryID = parametre.CategoryID;

            foodRepository.TUpdate(x);
            return RedirectToAction("Index");
        }
    }
}
