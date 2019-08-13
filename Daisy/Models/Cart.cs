using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daisy.Models
{
    public class Cart
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Display(Name = "Flower Name")]
        public string FlowerName { get; set; }

        [Range(1, 9999)]
        public int Quantity { get; set; }

        [Range(1, 999)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(1, 99999)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Display(Name = "Order By")]
        public string OrderBy { get; set; }
    }
}
