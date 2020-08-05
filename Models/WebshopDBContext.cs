using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class WebshopDBContext : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }
    }
}