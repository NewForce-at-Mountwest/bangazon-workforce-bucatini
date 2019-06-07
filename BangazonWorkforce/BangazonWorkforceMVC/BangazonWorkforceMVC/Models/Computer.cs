using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Models
{
    public class Computer
    {

        public int? Id { get; set; }

        public string Manufacturer { get; set; }

        public string Make { get; set; }

        public DateTime DecomissionDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PurchaseDate { get; set; }

    }
}
