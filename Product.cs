using System;
using System.ComponentModel.DataAnnotations;

namespace AppProjectNew.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Name { get; set; }

        [Required]
        [StringLength(40)]
        public string Sku { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Range(typeof(decimal), "0", "9999999999999999.99")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public DateTime CreatedUtc { get; set; }
    }
}