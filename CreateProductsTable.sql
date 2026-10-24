CREATE TABLE dbo.Products
(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    Sku NVARCHAR(40) NOT NULL,
    Description NVARCHAR(1000) NULL,
    Price DECIMAL(18,2) NOT NULL CONSTRAINT CK_Products_Price_NonNegative CHECK (Price >= 0),
    StockQuantity INT NOT NULL CONSTRAINT CK_Products_Stock_NonNegative CHECK (StockQuantity >= 0),
    CreatedUtc DATETIME2 NOT NULL,
    CONSTRAINT UQ_Products_Sku UNIQUE (Sku)
);