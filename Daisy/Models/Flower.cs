using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daisy.Models
{
    public class Flower
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Display(Name = "Flower Name")]
        public string FlowerName { get; set; }

        [Display(Name = "Flower Produced Date")]
        [DataType(DataType.Date)]
        public DateTime FlowerProducedDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Required]
        [StringLength(30)]
        public string Type { get; set; }

        [Range(1, 999)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(1, 9999)]
        public int Quantity { get; set; }

        [Required]
        public string Image { get; set; }

        public string ImageRef { get; set; }
    }
}
