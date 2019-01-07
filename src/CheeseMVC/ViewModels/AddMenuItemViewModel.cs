using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        //we'll need two properties to display the form
        public Menu Menu { get; set; }
        public List<SelectListItem> Cheeses { get; set; }
        
        //We'll need two integer properties to process our form: 
        public int CheeseID { get; set; }
        public int MenuID { get; set; }
                
        
        //Defult Constructor
        //The default constructor is needed for model binding to work.
        public AddMenuItemViewModel() { }

   //Constructor when we want to render the form
        public AddMenuItemViewModel(Menu menu, IEnumerable<Cheese> cheeses)
        {
            // initialize Cheeses to an empty list 
            Cheeses = new List<SelectListItem>();

            foreach (var cheese in cheeses)
            {
                Cheeses.Add(new SelectListItem
                {
                    Value = cheese.ID.ToString(),
                    Text = cheese.Name
                });
            }
            Menu = menu;
        }
   
    }
}
