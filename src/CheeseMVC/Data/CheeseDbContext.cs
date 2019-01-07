using CheeseMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Data
{
    public class CheeseDbContext : DbContext
    {
        //These DbSets(Cheese and CheeseCategory) are related(in Same Model folder)
        //so can put their properties in the same DbContext
        //These will be Table names
        public DbSet<Cheese> Cheeses { get; set; }//Has its own Controller class
        public DbSet<CheeseCategory> Categories { get; set; }//Has its own Controller class

        public DbSet<Menu> Menus { get; set; }
        public DbSet<CheeseMenu> CheeseMenus { get; set; }

        public CheeseDbContext(DbContextOptions<CheeseDbContext> options) 
            : base(options)
        {
        }

//This sets up the Primary key for CheeseMenu Class (only need when building mant to many relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheeseMenu>()
                .HasKey(c => new { c.CheeseID, c.MenuID });
        }
    }
}
