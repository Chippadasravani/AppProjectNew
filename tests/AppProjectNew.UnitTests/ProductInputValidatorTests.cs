using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AppProjectNew.Data;
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
        public void TryCreate_ExactUpperBounds_AcceptsInput()
        {
            var succeeded = ProductInputValidator.TryCreate(
                new string('n', 120),
                new string('s', 40),
                new string('d', 1000),
                "9999999999999999.99",
                "1",
                out var product,
                out var error);

            Assert.IsTrue(succeeded, error);
            Assert.AreEqual(9999999999999999.99m, product.Price);
            Assert.AreEqual(120, product.Name.Length);
            Assert.AreEqual(40, product.Sku.Length);
            Assert.AreEqual(1000, product.Description.Length);
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

        [TestMethod]
        public void Product_PriceAnnotationIsCultureInvariant()
        {
            var originalCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
                var product = new Product
                {
                    Name = "Product",
                    Sku = "SKU-1",
                    Price = 19.95m,
                    StockQuantity = 1
                };

                var validationResults = new System.Collections.Generic.List<ValidationResult>();
                var isValid = Validator.TryValidateObject(
                    product,
                    new ValidationContext(product),
                    validationResults,
                    validateAllProperties: true);

                Assert.IsTrue(isValid);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }

        [TestMethod]
        public void TryCreate_MaxIntStock_AcceptsUpperBound()
        {
            var succeeded = ProductInputValidator.TryCreate(
                "Product",
                "SKU-1",
                string.Empty,
                "1.00",
                int.MaxValue.ToString(CultureInfo.InvariantCulture),
                out var product,
                out var error);

            Assert.IsTrue(succeeded, error);
            Assert.IsNotNull(product);
            Assert.AreEqual(int.MaxValue, product.StockQuantity);
        }

        [TestMethod]
        public void ProductDbContext_ConfiguresUniqueSkuIndex()
        {
            var modelBuilder = new DbModelBuilder();
            modelBuilder.Entity<Product>().Property(product => product.Sku)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Products_Sku") { IsUnique = true }));

            var model = modelBuilder.Build(new DbProviderInfo("System.Data.SqlClient", "2012"));
            var storeModel = model.GetType().GetProperty("StoreModel")?.GetValue(model);
            Assert.IsNotNull(storeModel);

            var entityTypes = storeModel.GetType()
                .GetProperty("EntityTypes")
                ?.GetValue(storeModel) as IEnumerable;

            Assert.IsNotNull(entityTypes);

            foreach (var entity in entityTypes)
            {
                var entityName = entity.GetType().GetProperty("Name")?.GetValue(entity) as string;
                if (entityName != nameof(Product))
                {
                    continue;
                }

                var properties = entity.GetType().GetProperty("Properties")?.GetValue(entity) as IEnumerable;
                Assert.IsNotNull(properties);

                foreach (var property in properties)
                {
                    var propertyName = property.GetType().GetProperty("Name")?.GetValue(property) as string;
                    if (propertyName != nameof(Product.Sku))
                    {
                        continue;
                    }

                    var metadataProperties = property.GetType().GetProperty("MetadataProperties")?.GetValue(property) as IEnumerable;
                    Assert.IsNotNull(metadataProperties);

                    foreach (var metadataProperty in metadataProperties)
                    {
                        var metadataName = metadataProperty.GetType().GetProperty("Name")?.GetValue(metadataProperty) as string;
                        if (string.IsNullOrEmpty(metadataName) || !metadataName.Contains("Index", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var value = metadataProperty.GetType().GetProperty("Value")?.GetValue(metadataProperty);
                        Assert.IsNotNull(value);
                        Assert.AreEqual(typeof(IndexAnnotation).FullName, value.GetType().FullName);

                        var indexes = value.GetType().GetProperty("Indexes")?.GetValue(value) as IEnumerable;
                        Assert.IsNotNull(indexes);

                        var indexAttributes = indexes.Cast<object>().ToList();
                        Assert.AreEqual(1, indexAttributes.Count);
                        Assert.AreEqual("IX_Products_Sku", indexAttributes[0].GetType().GetProperty("Name")?.GetValue(indexAttributes[0]));
                        Assert.AreEqual(true, indexAttributes[0].GetType().GetProperty("IsUnique")?.GetValue(indexAttributes[0]));
                        return;
                    }
                }
            }

            Assert.Fail("Expected a unique index annotation for the SKU property.");
        }

        [DataTestMethod]
        [DataRow(2601, true)]
        [DataRow(2627, true)]
        [DataRow(547, false)]
        [DataRow(8152, false)]
        public void ProductDatabaseErrorClassifier_OnlyMatchesUniqueConstraintErrors(int errorNumber, bool expected)
        {
            Assert.AreEqual(expected, ProductDatabaseErrorClassifier.IsUniqueConstraintViolation(new[] { errorNumber }));
        }

        [TestMethod]
        public void ProductDatabaseErrorClassifier_HandlesNullEmptyAndMixedErrorNumbers()
        {
            Assert.IsFalse(ProductDatabaseErrorClassifier.IsUniqueConstraintViolation(null));
            Assert.IsFalse(ProductDatabaseErrorClassifier.IsUniqueConstraintViolation(Array.Empty<int>()));
            Assert.IsTrue(ProductDatabaseErrorClassifier.IsUniqueConstraintViolation(new[] { 547, 2601, 8152 }));
            Assert.IsTrue(ProductDatabaseErrorClassifier.IsUniqueConstraintViolation(new[] { 999, 2627 }));
            Assert.IsFalse(ProductDatabaseErrorClassifier.IsUniqueConstraintViolation(new[] { 547, 8152 }));
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