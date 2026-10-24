using System;
using System.Globalization;
using AppProjectNew.Models;

namespace AppProjectNew.Validation
{
    public static class ProductInputValidator
    {
        private const decimal MaximumPrice = 9999999999999999.99m;

        public static bool TryCreate(
            string nameText,
            string skuText,
            string descriptionText,
            string priceText,
            string stockText,
            out Product product,
            out string errorMessage)
        {
            product = null;
            errorMessage = string.Empty;

            var name = (nameText ?? string.Empty).Trim();
            var sku = (skuText ?? string.Empty).Trim();
            var description = (descriptionText ?? string.Empty).Trim();

            if (name.Length == 0 || name.Length > 120)
            {
                errorMessage = "Enter a product name of 1 to 120 characters.";
                return false;
            }

            if (sku.Length == 0 || sku.Length > 40)
            {
                errorMessage = "Enter a SKU of 1 to 40 characters.";
                return false;
            }

            if (description.Length > 1000)
            {
                errorMessage = "Description must be 1000 characters or fewer.";
                return false;
            }

            decimal price;
            if (!decimal.TryParse(priceText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price) ||
                price < 0 || price > MaximumPrice || decimal.Round(price, 2) != price)
            {
                errorMessage = "Enter a non-negative price with no more than two decimal places.";
                return false;
            }

            int stockQuantity;
            if (!int.TryParse(stockText, NumberStyles.None, CultureInfo.InvariantCulture, out stockQuantity))
            {
                errorMessage = "Enter a whole number for stock quantity.";
                return false;
            }

            product = new Product
            {
                Name = name,
                Sku = sku,
                Description = description,
                Price = price,
                StockQuantity = stockQuantity,
                CreatedUtc = DateTime.UtcNow
            };
            return true;
        }
    }
}