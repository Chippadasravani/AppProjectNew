using System.ComponentModel.DataAnnotations;
using AppProjectNew.Models;
using AppProjectNew.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppProjectNew.UnitTests
{
    [TestClass]
    public class ProductInputValidatorTests
    {
        [TestMethod]
        public void TryCreate_ValidInput_TrimsFieldsAndCreatesProduct()
        {
            var succeeded = ProductInputValidator.TryCreate(
                "  Desk lamp  ", "  LAMP-01  ", "  Brass finish  ", "19.95", "4",
                out var product, out var error);

            Assert.IsTrue(succeeded);
            Assert.AreEqual(string.Empty, error);
            Assert.AreEqual("Desk lamp", product.Name);
            Assert.AreEqual("LAMP-01", product.Sku);
            Assert.AreEqual("Brass finish", product.Description);
            Assert.AreEqual(19.95m, product.Price);
            Assert.AreEqual(4, product.StockQuantity);
        }

        [TestMethod]
        public void TryCreate_ZeroPriceAndStock_AcceptsBoundaryValues()
        {
            var succeeded = ProductInputValidator.TryCreate(
                "Sample", "ZERO-1", null, "0", "0", out var product, out _);

            Assert.IsTrue(succeeded);
            Assert.AreEqual(0m, product.Price);
            Assert.AreEqual(0, product.StockQuantity);
            Assert.AreEqual(string.Empty, product.Description);
        }

        [TestMethod]
        public void TryCreate_EmptyNameOrSku_RejectsInput()
        {
            AssertInvalid("  ", "SKU-1", "1.00", "1", "product name");
            AssertInvalid("Product", "  ", "1.00", "1", "SKU");
        }

        [TestMethod]
        public void TryCreate_FieldsExceedingMaximumLength_RejectsInput()
        {
            AssertInvalid(new string('n', 121), "SKU-1", "1.00", "1", "product name");
            AssertInvalid("Product", new string('s', 41), "1.00", "1", "SKU");
            AssertInvalid("Product", "SKU-1", "1.00", "1", "Description", new string('d', 1001));
        }

        [DataTestMethod]
        [DataRow("-1.00")]
        [DataRow("1.234")]
        [DataRow("1,000.00")]
        [DataRow("10000000000000000.00")]
        [DataRow("not-a-price")]
        public void TryCreate_InvalidPrice_RejectsInput(string price)
        {
            AssertInvalid("Product", "SKU-1", price, "1", "price");
        }

        [DataTestMethod]
        [DataRow("-1")]
        [DataRow("1.5")]
        [DataRow("2147483648")]
        [DataRow("1,000")]
        [DataRow("not-a-number")]
        public void TryCreate_InvalidStockQuantity_RejectsInput(string stock)
        {
            AssertInvalid("Product", "SKU-1", "1.00", stock, "stock quantity");
        }

        [TestMethod]
        public void Product_DataAnnotationsRejectOutOfRangeValues()
        {
            var product = new Product
            {
                Name = "Product",
                Sku = "SKU-1",
                Price = -0.01m,
                StockQuantity = -1
            };

            var validationResults = new System.Collections.Generic.List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                product,
                new ValidationContext(product),
                validationResults,
                validateAllProperties: true);

            Assert.IsFalse(isValid);
            Assert.AreEqual(2, validationResults.Count);
        }

        private static void AssertInvalid(
            string name,
            string sku,
            string price,
            string stock,
            string expectedError,
            string description = null)
        {
            var succeeded = ProductInputValidator.TryCreate(
                name, sku, description ?? string.Empty, price, stock, out var product, out var error);

            Assert.IsFalse(succeeded);
            Assert.IsNull(product);
            StringAssert.Contains(error, expectedError);
        }
    }
}