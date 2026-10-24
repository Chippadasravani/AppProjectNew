using System.Collections.Generic;
using System.Linq;
using AppProjectNew.Models;

namespace AppProjectNew
{
    public static class CartService
    {
        public static bool TryAddProduct(List<CartItem> cartItems, Product product, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (product == null)
            {
                errorMessage = "That product could not be found.";
                return false;
            }

            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == product.Id);
            var newQuantity = (existingItem?.Quantity ?? 0) + 1;

            if (newQuantity > product.StockQuantity)
            {
                errorMessage = "There are not enough items in stock to add another unit.";
                return false;
            }

            if (existingItem == null)
            {
                cartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
                return true;
            }

            existingItem.Quantity = newQuantity;
            return true;
        }
    }
}
