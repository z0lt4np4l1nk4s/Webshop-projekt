using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class WebshopDBContext : DbContext
    {
        public DbSet<Proizvodi> Proizvod { get; set; }
        public DbSet<Kategorije> Kategorija { get; set; }
    }
}