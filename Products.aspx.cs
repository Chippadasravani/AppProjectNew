using System;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using AppProjectNew.Data;
using AppProjectNew.Models;
using AppProjectNew.Validation;

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

            Product productToSave;
            string validationError;
            if (!ProductInputValidator.TryCreate(
                nameInput.Text,
                skuInput.Text,
                descriptionInput.Text,
                priceInput.Text,
                stockInput.Text,
                out productToSave,
                out validationError))
            {
                SetStatus(validationError, true);
                return;
            }

            var sku = productToSave.Sku;

            try
            {
                using (var db = new ProductDbContext())
                {
                    if (db.Products.Any(product => product.Sku == sku))
                    {
                        SetStatus("That SKU is already in use.", true);
                        BindProducts(updateStatus: false);
                        return;
                    }

                    db.Products.Add(productToSave);
                    db.SaveChanges();
                }

                nameInput.Text = string.Empty;
                skuInput.Text = string.Empty;
                priceInput.Text = string.Empty;
                stockInput.Text = string.Empty;
                descriptionInput.Text = string.Empty;
                SetStatus("Product saved.", false);
            }
            catch (DbUpdateException exception)
            {
                var sqlException = exception.GetBaseException() as SqlException;
                if (sqlException != null && ProductDatabaseErrorClassifier.IsUniqueConstraintViolation(
                    sqlException.Errors.Cast<SqlError>().Select(error => error.Number)))
                {
                    SetStatus("That SKU is already in use.", true);
                }
                else
                {
                    Trace.TraceError("Failed to save product: {0}", exception);
                    SetStatus("The product could not be saved. Check the product details and try again.", true);
                }
            }
            catch (DbException)
            {
                SetStatus("The product database could not be reached. Check the LocalDB connection.", true);
            }
            catch (EntityException)
            {
                SetStatus("The product database could not be reached. Check the LocalDB connection.", true);
            }

            BindProducts(updateStatus: false);
        }

        private void BindProducts(bool updateStatus = true)
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
                if (updateStatus)
                {
                    SetStatus("The product database could not be reached. Check the LocalDB connection.", true);
                }
            }
            catch (EntityException)
            {
                productsGrid.DataSource = null;
                productsGrid.DataBind();
                if (updateStatus)
                {
                    SetStatus("The product database could not be reached. Check the LocalDB connection.", true);
                }
            }
        }

        private void SetStatus(string message, bool isError)
        {
            statusLabel.Text = message;
            statusLabel.CssClass = isError ? "status-message status-error" : "status-message status-success";
        }
    }
}