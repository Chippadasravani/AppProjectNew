using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace AppProjectNew
{
    public partial class Cart : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCart();
            }
        }

        private void BindCart()
        {
            var cartItems = GetCart();
            cartGrid.DataSource = cartItems;
            cartGrid.DataBind();

            totalItems.Text = cartItems.Sum(item => item.Quantity).ToString();
            totalPrice.Text = cartItems.Sum(item => item.Price * item.Quantity).ToString("N2");
            statusLabel.Text = cartItems.Count == 0 ? "Your cart is empty." : string.Empty;
            statusLabel.CssClass = cartItems.Count == 0 ? "status-message status-error" : "status-message";
        }

        private static List<CartItem> GetCart()
        {
            var cart = HttpContext.Current.Session["CartItems"] as List<CartItem>;
            return cart ?? new List<CartItem>();
        }
    }
}
