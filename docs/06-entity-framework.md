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

With EF Core, data access is performed using a model. A model is made up of entity classes and a context object that represents a session with the database, allowing you to query and save data.

```csharp
public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }
    }
```
```csharp
public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
```
## Relations

#### One To Many Relationships

```csharp
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Employee> Employees { get; set; }
}
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Company Company { get; set; }
}
```
```csharp
protected override void OnModelCreating(Modelbuilder modelBuilder)
{
    modelBuilder.Entity<Company>()
        .HasMany(c => c.Employees)
        .WithOne(e => e.Company);
    
    modelBuilder.Entity<Employee>()
        .HasOne(e => e.Company)
        .WithMany(c => c.Employees);
}
```

You can use the IsRequired method on the relationship to prevent the relationship being optional:
```csharp
protected override void OnModelCreating(Modelbuilder modelBuilder)
{
    modelBuilder.Entity<Company>()
        .HasMany(c => c.Employees)
        .WithOne(e => e.Company).
        .IsRequired();
}
```

###### Cascading Referential Integrity Constraints

If the relationship is configured as optional, the default behavour of EF Core is to take no action in respect of the dependent entity if the principal is deleted.
The default behaviour of a database is to raise an error in this scenario: foreign key values must reference existing primary key values.
You can alter this behaviour through the OnDelete method which takes a DeleteBehaviour enumeration. The following example sets the foreign key value of the dependent entity to null in the event that the principal is deleted:
```csharp
protected override void OnModelCreating(Modelbuilder modelBuilder)
{
    modelBuilder.Entity<Company>()
        .HasMany(c => c.Employees)
        .WithOne(e => e.Company).
        .OnDelete(DeleteBehavior.SetNull);
}
```
This example will result in the dependent entity being deleted:
```csharp
protected override void OnModelCreating(Modelbuilder modelBuilder)
{
    modelBuilder.Entity<Company>()
        .HasMany(c => c.Employees)
        .WithOne(e => e.Company).
        .OnDelete(DeleteBehavior.Delete);
}
```

#### One To One Relationships

```csharp
public class Author
{
    public int AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public AuthorBiography Biography { get; set; }
}
public class AuthorBiography
{
    public int AuthorBiographyId { get; set; }
    public string Biography { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PlaceOfBirth { get; set; }
    public string Nationality { get; set; }
    public int AuthorRef { get; set; }
    public Author Author { get; set; }
}
```
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Author>()
        .HasOne(a => a.Biography)
        .WithOne(b => b.Author)
        .HasForeignKey<AuthorBiography>(b => b.AuthorRef);
}
```
The Has/With pattern is used to close the loop and fully define a relationship. In this case, since the relationship to be configured is a one-to-one, the HasOne method is chained with the `WithOne` method. Then the dependent entity (`AuthorBiography`) is identified by passing it in as a type parameter to the `HasForeignKey` method, which takes a lambda specifying which property in the dependent type is the foreign key.
## DbContecxt

## Quering (LINQ)
## Saving Data
## CodeFirst
## DBFirst
## ModelFirst
