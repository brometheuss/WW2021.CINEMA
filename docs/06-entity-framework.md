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

#### Many To Many Relationships

A many-to-many relationship occurs between entities when a one-to-many relationship between them works both ways. A book can appear in many categories and a category can contain many books. This type of relationship is represented in a database by a join table (also known among other things as a bridging, junction or linking table).

```csharp
public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public Author Author { get; set; }
    public ICollection<BookCategory> BookCategories { get; set; }
}  
public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public ICollection<BookCategory> BookCategories { get; set; }
}  
public class BookCategory
{
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
```
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<BookCategory>()
        .HasKey(bc => new { bc.BookId, bc.CategoryId });  
    modelBuilder.Entity<BookCategory>()
        .HasOne(bc => bc.Book)
        .WithMany(b => b.BookCategories)
        .HasForeignKey(bc => bc.BookId);  
    modelBuilder.Entity<BookCategory>()
        .HasOne(bc => bc.Category)
        .WithMany(c => c.BookCategories)
        .HasForeignKey(bc => bc.CategoryId);
}
```
The **primary key** for the join table is a **composite key** comprising both of the **foreign key** values. In addition, both sides of the many-to-many relationship are configured using the `HasOne`, `WithMany` and `HasForeignKey` Fluent API methods.

## DbContecxt

The Entity Framework Core DbContext class represents a session with a database and provides an API for communicating with the database with the following capabilities:
- Database Connections
- Data operations such as querying and persistance
- Change Tracking
- Model building
- Data Mapping
- Object caching
- Transaction management

```csharp
public class SchoolContext : DbContext
{
    //entities
    public DbSet<Student> Books { get; set; }
    public DbSet<Category> Categories { get; set; }

    public SchoolContext()
    {
  
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=database;Integrated Security=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
    
} 
```

In the example above, the `SchoolContext` class is derived from the `DbContext` class and contains the `DbSet<TEntity>` properties of `Student` and `Course` type. It also overrides the `OnConfiguring` and `OnModelCreating` methods. We must create an instance of `SchoolContext` to connect to the database and save or retrieve `Student` or `Course` data.
The `OnConfiguring()` method allows us to select and configure the data source to be used with a context using `DbContextOptionsBuilder`.

## Quering (LINQ)

Entity Framework Core uses Language Integrated Query (LINQ) to query data from the database. LINQ allows you to use C# (or your .NET language of choice) to write strongly typed queries. It uses your derived context and entity classes to reference database objects. EF Core passes a representation of the LINQ query to the database provider.

##### Loading All Data
```csharp
using (var context = new BloggingContext())
{
    var blogs = context.Blogs.ToList();
}
```
##### Loading a single entity
```csharp
using (var context = new BloggingContext())
{
    var blog = context.Blogs
        .Single(b => b.BlogId == 1);
}
```
##### Filtering
```csharp
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Where(b => b.Url.Contains("dotnet"))
        .ToList();
}
```

## Saving Data

Each context instance has a `ChangeTracker` that is responsible for keeping track of changes that need to be written to the database. As you make changes to instances of your entity classes, these changes are recorded in the `ChangeTracker` and then written to the database when you call `SaveChanges`. The database provider is responsible for translating the changes into database-specific operations (for example, `INSERT`, `UPDATE`, and `DELETE` commands for a relational database).

##### Basic Save
Addind data:
```csharp
using (var context = new BloggingContext())
{
    var blog = new Blog { Url = "http://sample.com" };
    context.Blogs.Add(blog);
    context.SaveChanges();
}
```
Updating data:
```csharp
using (var context = new BloggingContext())
{
    var blog = context.Blogs.First();
    blog.Url = "http://sample.com/blog";
    context.SaveChanges();
}
```
Deleting data:
```csharp
using (var context = new BloggingContext())
{
    var blog = context.Blogs.First();
    context.Blogs.Remove(blog);
    context.SaveChanges();
}
```
https://docs.microsoft.com/en-us/ef/core/saving/

## CodeFirst

### What is Code-First?

In the Code-First approach, you focus on the domain of your application and start creating classes for your domain entity rather 
than design your database first and then create the classes which match your database design. The following figure illustrates the code-first approach. 

![picture alt](images/ef-code-first.png)

As you can see in the above figure, EF API will create the database based on your domain classes and configuration. 
This means you need to start coding first in C# and then EF will create the database from your code.

## DbFirst

### What is Database-First?

Database First approach allows developers to build software applications from their existing databases. 
You connect to an exisitng database and Visual Studio and EF build a data object model and the complete application for you with very little code. 

![picture alt](images/ef-database-first.png)

## ModelFirst

### What is Model-First?

In the model-first approach, you create entities, relationships, and inheritance hierarchies directly on the visual designer integrated in Visual Studio 
and then generate entities, the context class, and the database script from your visual model.

![picture alt](images/ef-database-first.png)
