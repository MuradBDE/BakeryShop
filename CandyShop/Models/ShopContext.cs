using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyShop.Models
{
    public class ShopContext : DbContext
    {
        public DbSet<Cake> Cakes { get; set; }
        public DbSet<User> Users { get; set; }
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
