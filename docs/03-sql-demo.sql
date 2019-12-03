-- Simple Query
SELECT * FROM dbo.Customer;

SELECT * FROM dbo.Customer
WHERE FirstName = 'Maria';

-- Group By
SELECT COUNT(*), City FROM dbo.Customer
GROUP BY City;

-- Join

SELECT O.OrderNumber, CONVERT(date,O.OrderDate) AS Date, 
       P.ProductName, I.Quantity, I.UnitPrice 
  FROM [Order] O 
  JOIN OrderItem I ON O.Id = I.OrderId 
  JOIN Product P ON P.Id = I.ProductId
ORDER BY O.OrderNumber;

-- Views
GO
CREATE VIEW demo
AS 
SELECT O.OrderNumber, O.OrderDate, I.Quantity, I.UnitPrice 
  FROM [Order] O 
  JOIN OrderItem I ON O.Id = I.OrderId 
  JOIN Product P ON P.Id = I.ProductId

SELECT * FROM demo

-- Stored Procedures
GO
CREATE PROCEDURE DEMOPROC
AS
SELECT O.OrderNumber, O.OrderDate, I.Quantity, I.UnitPrice 
  FROM [Order] O 
  JOIN OrderItem I ON O.Id = I.OrderId 
  JOIN Product P ON P.Id = I.ProductId

EXEC DEMOPROC

-- Triggers

CREATE TRIGGER demo_trigger_1
ON product
AFTER INSERT, DELETE
AS
BEGIN
SET NOCOUNT ON;
    INSERT INTO Product_Audit( 
        ProductName,
        SupplierId,
        UnitPrice,
        Package,
        IsDiscontinued,
		[Date],
		Operation
    )
    SELECT
        ProductName,
        SupplierId,
        UnitPrice,
        Package,
        IsDiscontinued,
        GETDATE(),
        'INS'
    FROM
        inserted i
    UNION ALL
    SELECT
        ProductName,
        SupplierId,
        UnitPrice,
        Package,
        IsDiscontinued,
        GETDATE(),
        'DEL'
    FROM
        deleted d;
END

-- first delete references in orderItem
DELETE FROM Product
WHERE Id=1