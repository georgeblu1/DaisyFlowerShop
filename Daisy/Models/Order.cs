using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daisy.Models
{
    public class Order
    {
        public int ID { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Display(Name = "Order By")]
        public string OrderBy { get; set; }

        [Required]
        [Display(Name = "Flower Bought")]
        public string FlowerBought { get; set; }

        [Range(1, 999999)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}
