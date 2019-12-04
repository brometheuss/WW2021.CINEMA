#Introduction

Entity Framework (EF) is an object-relational mapper that enables .NET developers to work with relational data using domain-specific objects. 
It eliminates the need for most of the data-access code that developers usually need to write.

EF Core is an object-relational mapper (ORM). Object-relational mapping is a technique that enables developers to work with data in object-oriented way by performing the work required to map between objects defined in an application's programming language and data stored in relational datasources.

Without ORM:
```csharp
using(var conn = new SqlConnection(connectionString))
using(var cmd = new SqlCommand("select * from Products", conn))
{
    var dt = new DataTable();
    using(var da = new SqlDataAdapter(cmd))
    {
        da.Fill(dt);
    }
}
```
```csharp
foreach(DataRow row in dt.Rows)
{
    int productId = Convert.ToInt32(row[0]);
    string productName = row["ProductName"].ToString();
}
```

With ORM:
```csharp
public class Product
{
    int ProductId { get; set; }
    string ProductName { get; set; }
}
int productId = myProduct.ProductId;
string productName = myProduct.ProductName;
```

ORMs are pre-written libraries of code that do this work for you. Full-featured ORMs do a lot more too. They can:
- map a domain model to database objects
- create databases and maintain the schema in line with changes to the model
- generate SQL and execute it against the database
- manage transactions
- keep track of objects that have already been retrieved

https://docs.microsoft.com/en-us/ef/core/

## Data Models

## Relations

## DbContecxt

## Quering (LINQ)
## Saving Data
## CodeFirst
## DBFirst
## ModelFirst
