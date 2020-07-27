using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Proizvodi
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Kategorija")]
        public virtual Kategorije Kategorije { get; set; }
        [Required]
        public string Naziv { get; set; }
        public string Opis { get; set; }
        [Display(Name = "Proizvođač")]
        public string Proizvodac { get; set; }
        [Required]
        public string Cijena { get; set; }
        [Display(Name = "Na popustu")]
        public bool isPopust { get; set; }
        public float? Popust { get; set; }
        public int? Garancija { get; set; }
        public string RAM { get; set; }
        public string Prostor { get; set; }
        [Display(Name = "Snaga napajanja")]
        public string Snaga { get; set; }
        [Display(Name = "Veličina")]
        public string Velicina { get; set; }
        [Display(Name = "Radna frekvencija")]
        public string RadnaFrekvencija { get; set; }
        [Display(Name = "Broj jezgri")]
        public int? BrojJezgri { get; set; }
        [Display(Name = "Grafička")]
        public string IntegriranaGraficka { get; set; }
        public string Rezolucija { get; set; }
        [Display(Name = "Priključci")]
        public string Prikljucci { get; set; }
        [Display(Name = "Tip senzora")]
        public string TipSenzora { get; set; }
        [Display(Name = "Broj tipki")]
        public int? BrojTipki { get; set; }
        [Display(Name = "Tehnologija povezivanja")]
        public string TehnologijaPovezivanja { get; set; }
        [Display(Name = "Operacijski sustav")]
        public string OperacijskiSustav { get; set; }
        [Display(Name = "Vrsta tipkovnice")]
        public string VrstaTipkovnice { get; set; }
        [Display(Name = "Slika")]
        public string SlikaPath { get; set; }
        [NotMapped]
        public HttpPostedFileBase SlikaFile { get; set; }

    }
}