using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CheeseDbContext context;

        public CategoryController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.title = "Cheese Categories";
            List <CheeseCategory> allCategories = context.Categories.ToList();
            return View(allCategories);
        }
        public IActionResult Add()
        {
            ViewBag.title = "Add Categories";
            AddCategoryViewModel addCategoryViewModel = new AddCategoryViewModel();

            return View();
        }
        [HttpPost]
        public IActionResult Add(AddCategoryViewModel addCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                CheeseCategory newCategory = new CheeseCategory
                {
                    ID = addCategoryViewModel.ID,
                    Name = addCategoryViewModel.Name
                };
                context.Add(newCategory);
                context.SaveChanges();

                return Redirect("/Category");
            }
            return View(addCategoryViewModel);
        }
        public IActionResult Remove()
        {
            ViewBag.title = "Remove Category";
            ViewBag.categories = context.Categories.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] categoryIds)
        {
            foreach (int categoryId in categoryIds)
            {
                CheeseCategory theCategory = context.Categories.Single(c => c.ID == categoryId);

                context.Categories.Remove(theCategory);
            }

            context.SaveChanges();

            return Redirect("/");
        }
    
    }
}
