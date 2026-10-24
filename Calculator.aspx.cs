using System;
using System.Globalization;
using System.Web.UI;

public partial class Calculator : Page
{
    protected void addButton_Click(object sender, EventArgs e)
    {
        decimal first;
        decimal second;

        if (!decimal.TryParse(firstNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out first) ||
            !decimal.TryParse(secondNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out second))
        {
            resultLabel.Text = "Enter valid numbers in both fields.";
            return;
        }

        resultLabel.Text = "Sum: " + (first + second).ToString(CultureInfo.CurrentCulture);
    }
}