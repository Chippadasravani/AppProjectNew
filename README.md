# AppProjectNew

An ASP.NET Web Forms application with a product catalog form and a two-number sum calculator. Product records are stored with Entity Framework 6 Code First in SQL Server LocalDB.

## Requirements

- Windows
- Visual Studio 2022 with the ASP.NET and web development workload
- .NET Framework 4.8 Developer Pack

ASP.NET Web Forms runs on .NET Framework and IIS; it is not supported by the .NET SDK on Linux or macOS.

## Run locally

1. Open `AppProjectNew.csproj` in Visual Studio.
2. Restore NuGet packages when prompted. The project uses EntityFramework 6.4.4.
3. Start the project with IIS Express (F5). `Products.aspx` is the default page.

The first visit creates the `ProductCatalog` LocalDB database and `Products` table through the EF Code First initializer. Enter a product name, unique SKU, price, and stock count, then select **Save product**. Saved records appear in the product list. Invalid data and duplicate SKUs are rejected.

## Project files

- `Products.aspx` and `Products.aspx.cs` provide product creation and listing.
- `Product.cs` defines the validated product entity; `ProductDbContext.cs` maps it to the database.
- `CreateProductsTable.sql` contains the equivalent sample SQL Server table definition.
- `Calculator.aspx` and `Calculator.aspx.cs` provide the original sum calculator.
- `Web.config` stores the LocalDB connection string and Entity Framework provider setup.

To use another SQL Server instance, update the `ProductDbContext` connection string in `Web.config`.