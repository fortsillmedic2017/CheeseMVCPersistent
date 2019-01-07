using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    //Start with every Controler So can have an object 
    //to ref the CheeseDBContext Class that works with the database
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbcontext)
        {
            context = dbcontext;
        }

        //Show List of All Menus
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.title = "List Of All Menus";
            IList<Menu> allMenus = context.Menus.ToList();

            return View(allMenus);
        }

        // allow users to add new menus (empty menus) via a form. 
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Title = "Menu";
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }
        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };
                context.Menus.Add(newMenu);
                context.SaveChanges();
                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }

        //In our next set of tasks we create functionality 
        //that allows the user to view the contents of a given menu.
        //Will Use ViewModel to package up all the Data.

        [HttpGet]
        public IActionResult ViewMenu(int id)
        {         
            //We'll also need the items associated with the menu:
            List<CheeseMenu> items = context
                             .CheeseMenus
                             .Include(item => item.Cheese)
                             .Where(cm => cm.MenuID == id)
                             .ToList();

            // retrieve the Menu object with the given ID using context
             Menu menu = context.Menus.Single(m => m.ID == id);

            //Use items and the menu object you found above, to build a ViewMenuViewModel 
            //and pass it into the view.
            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Menu = menu,
                Items = items
            };
            return View(viewMenuViewModel);

        }
             
        //Use to get the Items we want to add
        //   /Menu/Add/3
        [HttpGet]
        public IActionResult AddItem(int id)
        {
            //Get/create object (menu) from List using LINQ to choose what want 
            //from list by it's ID property (use context)
            Menu menu = context.Menus.Single(m => m.ID == id);
           
            //Get/create List<Cheese>object (cheeses) (use context)
            // To get a list of all cheeses in system
            List<Cheese> cheeses = context.Cheeses.ToList();

            //Create an instance of AddMenuItemViewModel with the given 
            //Menue objectas well as the list of all Cheese items in the database.

            return View (new AddMenuItemViewModel(menu, cheeses));
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {                
                   var cheeseID = addMenuItemViewModel.CheeseID;
                   var menuID = addMenuItemViewModel.MenuID;

                    IList<CheeseMenu> existingItems = context
                        .CheeseMenus
                        .Where(cm => cm.CheeseID == cheeseID)
                        .Where(cm => cm.MenuID == menuID)
                        .ToList();
                if (existingItems.Count == 0)
                {
                    CheeseMenu menuItem = new CheeseMenu
                    {
                        Cheese = context.Cheeses.Single(c => c.ID == cheeseID),
                        Menu = context.Menus.Single(m => m.ID == menuID)
                    };

                    context.CheeseMenus.Add(menuItem);
                    context.SaveChanges();
                }

                return Redirect(String.Format("/Menu/ViewMenu/{0}", addMenuItemViewModel.MenuID));
            };

            return View(addMenuItemViewModel);
        }


        public IActionResult Remove()
        {
            ViewBag.title = "Remove Menu";
            ViewBag.menus = context.Menus.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] menuIds)
        {
            foreach (int menuId in menuIds)
            {
                Menu theMenu = context.Menus.Single(c => c.ID == menuId);

                context.Menus.Remove(theMenu);
            }

            context.SaveChanges();

            return Redirect("/");
        }

    }
}