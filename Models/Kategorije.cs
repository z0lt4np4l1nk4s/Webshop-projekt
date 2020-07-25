using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Kategorije
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string ImeKategorije { get; set; }
        public virtual ICollection<Proizvodi> Proizvodi { get; set; }

    }
}