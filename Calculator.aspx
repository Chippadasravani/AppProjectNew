<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calculator.aspx.cs" Inherits="Calculator" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Sum of Two Numbers</title>
</head>
<body>
    <form id="form1" runat="server">
        <main>
            <h1>Sum of Two Numbers</h1>
            <p>
                <asp:Label ID="firstNumberLabel" runat="server" AssociatedControlID="firstNumber" Text="First number:" />
                <asp:TextBox ID="firstNumber" runat="server" TextMode="Number" />
            </p>
            <p>
                <asp:Label ID="secondNumberLabel" runat="server" AssociatedControlID="secondNumber" Text="Second number:" />
                <asp:TextBox ID="secondNumber" runat="server" TextMode="Number" />
            </p>
            <p>
                <asp:Button ID="addButton" runat="server" Text="Add" OnClick="addButton_Click" />
            </p>
            <asp:Label ID="resultLabel" runat="server" />
        </main>
    </form>
</body>
</html>