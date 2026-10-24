# Sum Calculator

A small ASP.NET Web Forms application that adds two numbers. It accepts decimal and negative values, validates both inputs on the server, and displays the result on the same page.

## Requirements

- Windows
- Visual Studio 2022 with the ASP.NET and web development workload
- .NET Framework 4.8 Developer Pack

ASP.NET Web Forms runs on .NET Framework and IIS; it is not supported by the .NET SDK on Linux or macOS.

## Run locally

1. Open `AppProjectNew.csproj` in Visual Studio.
2. Set `Calculator.aspx` as the start page if Visual Studio does not select it automatically.
3. Start the project with IIS Express (F5).

The project opens directly at the calculator page. Enter two values and select **Calculate sum**. Invalid input produces a validation message instead of a result.

## Project files

- `Calculator.aspx` contains the Web Forms UI.
- `Calculator.aspx.cs` validates the inputs and calculates the sum.
- `Calculator.aspx.designer.cs` declares the server controls used by the page.
- `Calculator.css` contains the responsive page styles.
- `Web.config` configures the .NET Framework target and calculator start page.