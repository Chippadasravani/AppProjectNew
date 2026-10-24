using System;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using AppProjectNew.Data;
using AppProjectNew.Models;

namespace AppProjectNew
{
    public partial class Products : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProducts();
            }
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            decimal price;
            int stockQuantity;
            if (!decimal.TryParse(priceInput.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price) ||
                price < 0 || price > 9999999999999999.99m || decimal.Round(price, 2) != price)
            {
                SetStatus("Enter a non-negative price with no more than two decimal places.", true);
                return;
            }

            if (!int.TryParse(stockInput.Text, NumberStyles.None, CultureInfo.InvariantCulture, out stockQuantity))
            {
                SetStatus("Enter a whole number for stock quantity.", true);
                return;
            }

            var name = nameInput.Text.Trim();
            var sku = skuInput.Text.Trim();
            var description = descriptionInput.Text.Trim();
            if (name.Length == 0 || name.Length > 120 || sku.Length == 0 || sku.Length > 40 || description.Length > 1000)
            {
                SetStatus("Check product name, SKU, and description lengths.", true);
                return;
            }

            try
            {
                using (var db = new ProductDbContext())
                {
                    if (db.Products.Any(product => product.Sku == sku))
                    {
                        SetStatus("That SKU is already in use.", true);
                        BindProducts();
                        return;
                    }

                    db.Products.Add(new Product
                    {
                        Name = name,
                        Sku = sku,
                        Description = description,
                        Price = price,
                        StockQuantity = stockQuantity,
                        CreatedUtc = DateTime.UtcNow
                    });
                    db.SaveChanges();
                }

                nameInput.Text = string.Empty;
                skuInput.Text = string.Empty;
                priceInput.Text = string.Empty;
                stockInput.Text = string.Empty;
                descriptionInput.Text = string.Empty;
                SetStatus("Product saved.", false);
            }
            catch (DbUpdateException)
            {
                SetStatus("The product could not be saved. The SKU may already exist.", true);
            }
            catch (DbException)
            {
                SetStatus("The product database could not be reached. Check the LocalDB connection.", true);
            }
            catch (EntityException)
            {
                SetStatus("The product database could not be reached. Check the LocalDB connection.", true);
            }

            BindProducts();
        }

        private void BindProducts()
        {
            try
            {
                using (var db = new ProductDbContext())
                {
                    productsGrid.DataSource = db.Products
                        .AsNoTracking()
                        .OrderByDescending(product => product.CreatedUtc)
                        .Take(50)
                        .ToList();
                    productsGrid.DataBind();
                }
            }
            catch (DbException)
            {
                productsGrid.DataSource = null;
                productsGrid.DataBind();
                SetStatus("The product database could not be reached. Check the LocalDB connection.", true);
            }
            catch (EntityException)
            {
                productsGrid.DataSource = null;
                productsGrid.DataBind();
                SetStatus("The product database could not be reached. Check the LocalDB connection.", true);
            }
        }

        private void SetStatus(string message, bool isError)
        {
            statusLabel.Text = message;
            statusLabel.CssClass = isError ? "status-message status-error" : "status-message status-success";
        }
    }
}