<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calculator.aspx.cs" Inherits="Calculator" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Sum Calculator</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Calculator.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="page-shell">
            <section class="calculator" aria-labelledby="page-title">
                <p class="eyebrow">A small arithmetic tool</p>
                <h1 id="page-title">Sum Calculator</h1>
                <p class="intro">Enter two numbers to find their total.</p>
                <div class="fields">
                    <div class="field">
                        <asp:Label ID="firstNumberLabel" runat="server" AssociatedControlID="firstNumber" Text="First number" />
                        <asp:TextBox ID="firstNumber" runat="server" TextMode="Number" step="any" CssClass="number-input" />
                    </div>
                    <div class="operator" aria-hidden="true">+</div>
                    <div class="field">
                        <asp:Label ID="secondNumberLabel" runat="server" AssociatedControlID="secondNumber" Text="Second number" />
                        <asp:TextBox ID="secondNumber" runat="server" TextMode="Number" step="any" CssClass="number-input" />
                    </div>
                </div>
                <asp:Button ID="addButton" runat="server" Text="Calculate sum" OnClick="addButton_Click" CssClass="calculate-button" />
                <asp:Label ID="resultLabel" runat="server" CssClass="result" role="status" aria-live="polite" />
            </section>
        </main>
    </form>
</body>
</html>