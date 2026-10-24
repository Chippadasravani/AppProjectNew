<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductQuantity.aspx.cs" Inherits="AppProjectNew.ProductQuantity" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Product Quantity</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Products.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="page-shell">
            <header class="page-header">
                <div>
                    <p class="eyebrow">Inventory</p>
                    <h1>Product quantity overview</h1>
                </div>
                <a class="secondary-link" href="Products.aspx">Back to catalog</a>
            </header>

            <section class="catalog-section" aria-labelledby="summary-title">
                <div class="section-heading catalog-heading">
                    <div>
                        <p class="eyebrow">Snapshot</p>
                        <h2 id="summary-title">Inventory totals</h2>
                    </div>
                    <span class="section-index">LIVE STOCK</span>
                </div>

                <div class="form-grid">
                    <div class="field">
                        <asp:Label ID="totalProductsLabel" runat="server" Text="Total products" CssClass="field-label" />
                        <div class="summary-value"><asp:Label ID="totalProducts" runat="server" /></div>
                    </div>
                    <div class="field">
                        <asp:Label ID="totalStockLabel" runat="server" Text="Total quantity" CssClass="field-label" />
                        <div class="summary-value"><asp:Label ID="totalStock" runat="server" /></div>
                    </div>
                </div>

                <asp:Label ID="statusLabel" runat="server" CssClass="status-message" role="status" aria-live="polite" />
            </section>

            <section class="catalog-section" aria-labelledby="table-title">
                <div class="section-heading catalog-heading">
                    <div>
                        <p class="eyebrow">Product list</p>
                        <h2 id="table-title">Current stock by item</h2>
                    </div>
                    <span class="section-index">ALL ITEMS</span>
                </div>

                <div class="table-scroll">
                    <asp:GridView ID="productsGrid" runat="server" AutoGenerateColumns="false" CssClass="product-table" GridLines="None" EmptyDataText="No products were found in the database.">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Product" HtmlEncode="true" />
                            <asp:BoundField DataField="Sku" HeaderText="SKU" HtmlEncode="true" />
                            <asp:BoundField DataField="StockQuantity" HeaderText="In stock" />
                            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:N2}" />
                        </Columns>
                    </asp:GridView>
                </div>
            </section>
        </main>
    </form>
</body>
</html>
