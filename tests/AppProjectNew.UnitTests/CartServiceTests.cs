using System.Collections.Generic;
using AppProjectNew.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppProjectNew.UnitTests
{
    [TestClass]
    public class CartServiceTests
    {
        [TestMethod]
        public void TryAddProduct_IncrementsQuantityForExistingItem()
        {
            var cartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Name = "Desk lamp", Price = 9.99m, Quantity = 1 }
            };

            var product = new Product
            {
                Id = 1,
                Name = "Desk lamp",
                Price = 9.99m,
                StockQuantity = 5
            };

            var succeeded = CartService.TryAddProduct(cartItems, product, out var error);

            Assert.IsTrue(succeeded, error);
            Assert.AreEqual(2, cartItems[0].Quantity);
            Assert.AreEqual("Desk lamp", cartItems[0].Name);
        }

        [TestMethod]
        public void TryAddProduct_RejectsOrdersBeyondAvailableStock()
        {
            var cartItems = new List<CartItem>
            {
                new CartItem { ProductId = 2, Name = "Notebook", Price = 3.50m, Quantity = 2 }
            };

            var product = new Product
            {
                Id = 2,
                Name = "Notebook",
                Price = 3.50m,
                StockQuantity = 2
            };

            var succeeded = CartService.TryAddProduct(cartItems, product, out var error);

            Assert.IsFalse(succeeded);
            StringAssert.Contains(error, "There are not enough items in stock");
            Assert.AreEqual(2, cartItems[0].Quantity);
        }
    }
}
