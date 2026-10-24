<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="AppProjectNew.Cart" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Shopping Cart</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Products.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="page-shell">
            <header class="page-header">
                <div>
                    <p class="eyebrow">Cart</p>
                    <h1>Your shopping cart</h1>
                </div>
                <a class="secondary-link" href="Products.aspx">Continue shopping</a>
            </header>

            <section class="catalog-section" aria-labelledby="cart-title">
                <div class="section-heading catalog-heading">
                    <div>
                        <p class="eyebrow">Summary</p>
                        <h2 id="cart-title">Cart totals</h2>
                    </div>
                    <span class="section-index">ITEMS</span>
                </div>

                <div class="form-grid">
                    <div class="field">
                        <asp:Label ID="totalItemsLabel" runat="server" Text="Items in cart" CssClass="field-label" />
                        <div class="summary-value"><asp:Label ID="totalItems" runat="server" /></div>
                    </div>
                    <div class="field">
                        <asp:Label ID="totalPriceLabel" runat="server" Text="Total price" CssClass="field-label" />
                        <div class="summary-value"><asp:Label ID="totalPrice" runat="server" /></div>
                    </div>
                </div>

                <asp:Label ID="statusLabel" runat="server" CssClass="status-message" role="status" aria-live="polite" />
            </section>

            <section class="catalog-section" aria-labelledby="cart-table-title">
                <div class="section-heading catalog-heading">
                    <div>
                        <p class="eyebrow">Products</p>
                        <h2 id="cart-table-title">Selected items</h2>
                    </div>
                    <span class="section-index">CART</span>
                </div>

                <div class="table-scroll">
                    <asp:GridView ID="cartGrid" runat="server" AutoGenerateColumns="false" CssClass="product-table" GridLines="None" EmptyDataText="Your cart is empty.">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Product" HtmlEncode="true" />
                            <asp:BoundField DataField="Price" HeaderText="Unit price" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                            <asp:TemplateField HeaderText="Line total">
                                <ItemTemplate>
                                    <%# ((decimal)Eval("Price") * (int)Eval("Quantity")).ToString("N2") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </section>
        </main>
    </form>
</body>
</html>
