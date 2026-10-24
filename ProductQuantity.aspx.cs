using System;
using System.Data.Common;
using System.Linq;
using System.Web.UI;
using AppProjectNew.Data;

namespace AppProjectNew
{
    public partial class ProductQuantity : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInventorySummary();
            }
        }

        private void BindInventorySummary()
        {
            try
            {
                using (var db = new ProductDbContext())
                {
                    var products = db.Products
                        .AsNoTracking()
                        .OrderBy(product => product.Name)
                        .ToList();

                    totalProducts.Text = products.Count.ToString();
                    totalStock.Text = products.Sum(product => product.StockQuantity).ToString();
                    productsGrid.DataSource = products;
                    productsGrid.DataBind();
                }

                statusLabel.Text = string.Empty;
                statusLabel.CssClass = "status-message";
            }
            catch (DbException)
            {
                productsGrid.DataSource = null;
                productsGrid.DataBind();
                totalProducts.Text = "0";
                totalStock.Text = "0";
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
