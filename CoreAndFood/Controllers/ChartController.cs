using CoreAndFood.Data;
using CoreAndFood.Data.Models;
using CoreAndFood.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CoreAndFood.Controllers
{
    public class ChartController : Controller
    {
        Context c= new Context();
        FoodRepository foodRepository = new FoodRepository();
        CategoryRepository categoryRepository = new CategoryRepository();


        public IActionResult PieChart()
        {
            return View();
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;
        }
        public IActionResult ColumnChart()
        {
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;
            return View();
        }
        public IActionResult VisualizeProductResult()
        {
            return Json(ProductList());
        }
        public List<GoogleChart> ProductList() 
        {
            List<GoogleChart> cs = new List<GoogleChart>();
            using (var c = new Context())
            {
                cs = c.Foods.Select(x => new GoogleChart
                {
                    Name = x.Name,
                    Stock = x.Stock
                }).ToList();
            }

            return cs;
        }

        public IActionResult DynamicChart()
        {
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;
            return View();
        }
        public IActionResult VisualizeProductResultDynamic() 
        {
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;
            var result = CategoryStock();
            return Json(result);
        }
        public List<DynamicChart> CategoryStock()
        {
            List<DynamicChart> result = new List<DynamicChart>();

            using (var context = new Context())
            {
                var categories = context.Categories.Include(c => c.Foods).ToList();

                foreach (var category in categories)
                {
                    int totalStock = category.Foods.Sum(f => f.Stock);

                    DynamicChart chartData = new DynamicChart
                    {
                        Name = category.Name,
                        Stock = totalStock
                    };

                    result.Add(chartData);
                }
            }

            return result;
        }

        public IActionResult Statistics()
        {
            var userName = HttpContext.User.Identity.Name;
            TempData["UserName"] = userName;

            var totalfood = c.Foods.Count();
            ViewBag.totalFood = totalfood;

            var totalcategory = c.Categories.Count();
            ViewBag.totalCategory = totalcategory;

            var fruitID = c.Categories.Where(x => x.Name == "Meyveler").Select(y => y.CategoryID).FirstOrDefault();

            var Furit = c.Foods.Where(x => x.CategoryID == fruitID).Count();
            ViewBag.furit = Furit;

            //var m = c.Foods.Where(s => true).Select(m => new
            //{
            //    data1=m
            //});

            var Vegetables = c.Foods.Where(x => x.CategoryID == c.Categories.Where(z => z.Name == "Sebzeler").Select(y => y.CategoryID).FirstOrDefault()).Count();
            ViewBag.vegetables = Vegetables;

            var Legumes = c.Foods.Where(x => x.CategoryID == c.Categories.Where(z => z.Name == "Bakliyat").Select(y => y.CategoryID).FirstOrDefault()).Count();
            ViewBag.legumes = Legumes;

            var MaxFood = c.Foods.OrderByDescending(x => x.Stock).Select(y => y.Name).FirstOrDefault();
            ViewBag.maxFood = MaxFood;

            var MinFood = c.Foods.OrderBy(x => x.Stock).Select(y => y.Name).FirstOrDefault();
            ViewBag.minFood = MinFood;

            var SumStock = c.Foods.Sum(x => x.Stock);
            ViewBag.sumStock = SumStock;

            var AvgPrice = c.Foods.Average(x => x.Price).ToString("0.00");
            ViewBag.avgPrice = AvgPrice;

            var FruitID = c.Categories.Where(x => x.Name == "Meyveler").Select(y => y.CategoryID).FirstOrDefault();
            var MaxStockFruit = c.Foods.Where(x => x.CategoryID == FruitID).Sum(y => y.Stock);
            ViewBag.maxStokFruit = MaxStockFruit;

            var VegetablesID = c.Categories.Where(x => x.Name == "Sebzeler").Select(y => y.CategoryID).FirstOrDefault();
            var MaxStockVegetables = c.Foods.Where(x => x.CategoryID == VegetablesID).Sum(y => y.Stock);
            ViewBag.maxStokVegetables = MaxStockVegetables;

            var MaxPriceFood = c.Foods.OrderByDescending(x => x.Price).Select(y => y.Name).FirstOrDefault();
            ViewBag.maxPriceFood = MaxPriceFood;

            //var category = categoryRepository.GenericGetList(x=>x.Name == "Fruit").Select(x=>x.CategoryID).FirstOrDefault();

            //var FuritId = foodRepository.GenericGetList(x => x.CategoryID == category).Count();

            //var data = foodRepository.TList().Select(x => x.Price).Average().ToString("0.00");
            //ViewBag.avgPrice = data;

            return View();
        }
    } 
}
