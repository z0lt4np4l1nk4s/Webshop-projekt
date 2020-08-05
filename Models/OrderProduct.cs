using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Deployment.Internal;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class OrderProduct
    {
        [Key]
        public int ID { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}