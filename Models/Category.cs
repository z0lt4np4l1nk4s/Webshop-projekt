using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Ime kategorije")]
        public string ImeKategorije { get; set; }
        [Display(Name = "Slike")]
        public string SlikaPath { get; set; }
        [NotMapped]
        public HttpPostedFileBase SlikaFile { get; set; }
        public virtual ICollection<Product> Proizvodi { get; set; }

    }
}