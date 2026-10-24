using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AppProjectNew.Models
{
    public sealed class InvariantDecimalRangeAttribute : ValidationAttribute
    {
        private readonly decimal minimum;
        private readonly decimal maximum;

        public InvariantDecimalRangeAttribute(string minimum, string maximum)
        {
            this.minimum = decimal.Parse(minimum, CultureInfo.InvariantCulture);
            this.maximum = decimal.Parse(maximum, CultureInfo.InvariantCulture);
            ErrorMessage = "Price is outside the supported range.";
        }

        public override bool IsValid(object value)
        {
            return value == null ||
                (value is decimal && (decimal)value >= minimum && (decimal)value <= maximum);
        }
    }

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

        [InvariantDecimalRange("0", "9999999999999999.99")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public DateTime CreatedUtc { get; set; }
    }
}