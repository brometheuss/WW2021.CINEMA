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