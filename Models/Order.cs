using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Ime")]
        [Required (ErrorMessage = "Vaše ime je obavezno")]
        public string Name { get; set; }
        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "Vaše prezime je obavezno")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Država je obavezna")]
        [Display(Name = "Država")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Grad je obavezan")]
        [Display(Name = "Grad")]
        public string City { get; set; }
        [Required(ErrorMessage = "Adresa je obavezna")]
        [Display(Name = "Adresa")]
        public string Adress { get; set; }
        public ICollection<Product> Product { get; set; }
    }
}