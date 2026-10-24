<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="AppProjectNew.Products" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Product Catalog</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Products.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="page-shell">
            <header class="page-header">
                <div>
                    <p class="eyebrow">Inventory</p>
                    <h1>Product catalog</h1>
                </div>
                <a class="secondary-link" href="Calculator.aspx">Open sum calculator</a>
            </header>

            <section class="entry-section" aria-labelledby="entry-title">
                <div class="section-heading">
                    <div>
                        <p class="eyebrow">Catalog management</p>
                        <h2 id="entry-title">Add a product</h2>
                    </div>
                    <span class="section-index">01 / NEW ITEM</span>
                </div>

                <asp:ValidationSummary ID="validationSummary" runat="server" CssClass="validation-summary" HeaderText="Check the highlighted fields:" />
                <div class="form-grid">
                    <div class="field">
                        <asp:Label ID="nameLabel" runat="server" AssociatedControlID="nameInput" Text="Product name" />
                        <asp:TextBox ID="nameInput" runat="server" MaxLength="120" CssClass="text-input" />
                        <asp:RequiredFieldValidator ID="nameRequired" runat="server" ControlToValidate="nameInput" ErrorMessage="Enter a product name." Display="Dynamic" CssClass="field-error" />
                    </div>
                    <div class="field">
                        <asp:Label ID="skuLabel" runat="server" AssociatedControlID="skuInput" Text="SKU" />
                        <asp:TextBox ID="skuInput" runat="server" MaxLength="40" CssClass="text-input" />
                        <asp:RequiredFieldValidator ID="skuRequired" runat="server" ControlToValidate="skuInput" ErrorMessage="Enter a SKU." Display="Dynamic" CssClass="field-error" />
                    </div>
                    <div class="field">
                        <asp:Label ID="priceLabel" runat="server" AssociatedControlID="priceInput" Text="Price" />
                        <asp:TextBox ID="priceInput" runat="server" TextMode="Number" step="0.01" min="0" CssClass="text-input" />
                        <asp:RequiredFieldValidator ID="priceRequired" runat="server" ControlToValidate="priceInput" ErrorMessage="Enter a price." Display="Dynamic" CssClass="field-error" />
                    </div>
                    <div class="field">
                        <asp:Label ID="stockLabel" runat="server" AssociatedControlID="stockInput" Text="Stock quantity" />
                        <asp:TextBox ID="stockInput" runat="server" TextMode="Number" step="1" min="0" CssClass="text-input" />
                        <asp:RequiredFieldValidator ID="stockRequired" runat="server" ControlToValidate="stockInput" ErrorMessage="Enter a stock quantity." Display="Dynamic" CssClass="field-error" />
                    </div>
                    <div class="field field-wide">
                        <asp:Label ID="descriptionLabel" runat="server" AssociatedControlID="descriptionInput" Text="Description" />
                        <asp:TextBox ID="descriptionInput" runat="server" TextMode="MultiLine" Rows="3" MaxLength="1000" CssClass="text-input text-area" />
                    </div>
                </div>
                <div class="form-actions">
                    <asp:Button ID="saveButton" runat="server" Text="Save product" OnClick="saveButton_Click" CssClass="primary-button" />
                    <asp:Label ID="statusLabel" runat="server" CssClass="status-message" role="status" aria-live="polite" />
                </div>
            </section>

            <section class="catalog-section" aria-labelledby="catalog-title">
                <div class="section-heading catalog-heading">
                    <div>
                        <p class="eyebrow">Recently saved</p>
                        <h2 id="catalog-title">Products</h2>
                    </div>
                    <span class="section-index">LATEST 50</span>
                </div>
                <div class="table-scroll">
                    <asp:GridView ID="productsGrid" runat="server" AutoGenerateColumns="false" CssClass="product-table" GridLines="None" EmptyDataText="No products yet. Add the first one above.">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Product" HtmlEncode="true" />
                            <asp:BoundField DataField="Sku" HeaderText="SKU" HtmlEncode="true" />
                            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="StockQuantity" HeaderText="In stock" />
                            <asp:BoundField DataField="CreatedUtc" HeaderText="Added (UTC)" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                        </Columns>
                    </asp:GridView>
                </div>
            </section>
        </main>
    </form>
</body>
</html>