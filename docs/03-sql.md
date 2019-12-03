# Introduction
In this section we will talk about SQL and Relational database.

SQL (pronounced "ess-que-el") stands for Structured Query Language. 
SQL is used to communicate with a database. 
According to ANSI (American National Standards Institute), it is the standard language for relational database management systems.

## RDBMS

RDBMS stands for Relational Database Management System. 
RDBMS is the basis for SQL, and for all modern database systems like MS SQL Server, IBM DB2, Oracle, MySQL, and Microsoft Access.
A Relational database management system (RDBMS) is a database management system (DBMS) that is based on the relational model as introduced by E. F. Codd.
A software system used to maintain relational databases is a relational database management system (RDBMS). 
Many relational database systems have an option of using the SQL (Structured Query Language) for querying and maintaining the database.

The relational model (RM) for database management is an approach to managing data using a structure and language consistent with first-order predicate logic, 
first described in 1969 by English computer scientist Edgar F. Codd, where all data is represented in terms of tuples, grouped into relations. 
A database organized in terms of the relational model is a relational database.

The purpose of the relational model is to provide a declarative method for specifying data and queries: users directly state what information the database 
contains and what information they want from it, and let the database management system software take care of describing data structures for storing the data 
and retrieval procedures for answering queries.

This model organizes data into one or more tables (or "relations") of columns and rows, with a unique key identifying each row. Rows are also called records or tuples.
Columns are also called attributes. Generally, each table/relation represents one "entity type" (such as customer or product). 
The rows represent instances of that type of entity (such as "Lee" or "chair") and the columns representing values attributed to that instance (such as address or price).

Each row in a table has its own unique key. Rows in a table can be linked to rows in other tables by adding a column for the unique key 
of the linked row (such columns are known as foreign keys). 
Codd showed that data relationships of arbitrary complexity can be represented by a simple set of concepts.
The primary keys within a database are used to define the relationships among the tables. When a PK migrates to another table, it becomes a foreign key in the other table.

Relationships are a logical connection between different tables, established on the basis of interaction among these tables.

https://en.wikipedia.org/wiki/Relational_database
https://en.wikipedia.org/wiki/Relational_model
https://www.tutorialspoint.com/sql/sql-rdbms-concepts.htm

## SQL vs NoSQL

A NoSQL (originally referring to "non SQL" or "non relational") database provides a mechanism for storage and retrieval of data 
that is modeled in means other than the tabular relations used in relational databases. Motivations for this approach include: simplicity of design, 
simpler "horizontal" scaling to clusters of machines (which is a problem for relational databases), finer control over availability 
and limiting the object-relational impedance mismatch. The data structures used by NoSQL databases (e.g. key-value, wide column, graph, or document) are 
different from those used by default in relational databases, making some operations faster in NoSQL. The particular suitability of a given 
NoSQL database depends on the problem it must solve. Sometimes the data structures used by NoSQL databases are also viewed as "more flexible" 
than relational database tables.

NoSQL databases: NoSQL databases, on the other hand, have dynamic schemas for unstructured data, and data is stored in many ways: 
They can be column-oriented, document-oriented, graph-based or organized as a KeyValue store. 
This flexibility means that:
- You can create documents without having to first define their structure
- Each document can have its own unique structure
- The syntax can vary from database to database, and
- You can add fields as you go.

#### The Scalability
In most situations, SQL databases are vertically scalable, which means that you can increase the load on a single server by increasing things like CPU, RAM or SSD. 
NoSQL databases, on the other hand, are horizontally scalable. This means that you handle more traffic by sharding, or adding more servers in your NoSQL database. 
It’s like adding more floors to the same building versus adding more buildings to the neighborhood. The latter can ultimately become larger and more powerful, 
making NoSQL databases the preferred choice for large or ever-changing data sets.


#### The Structure
SQL databases are table-based, while NoSQL databases are either document-based, key-value pairs, graph databases or wide-column stores. 
This makes relational SQL databases a better option for applications that require multi-row transactions - such as an accounting system - or f
or legacy systems that were built for a relational structure.

Some examples of SQL databases include MySQL, Oracle, PostgreSQL, and Microsoft SQL Server. NoSQL database examples include MongoDB, BigTable, 
Redis, RavenDB Cassandra, HBase, Neo4j and CouchDB.

**Microsoft SQL Server** is an excellent choice for small-to-medium-sized organizations that need a high-quality, 
professionally-managed database system with excellent support, but don't require the cost or scalability of an enterprise solution like Oracle.

**MongoDB** is a good choice for businesses that have rapid growth or databases with no clear schema definitions (i.e., you have a lot of unstructured data). 
If you cannot define a schema for your database, if you find yourself denormalizing data schemas, or if your data requirements and schemas 
are constantly evolving - as is often the case with mobile apps, real-time analytics, content management systems, etc. - MongoDB can be a strong choice for you.

https://www.xplenty.com/blog/the-sql-vs-nosql-difference/

## Insert, Update and Delete

#### Insert

The INSERT INTO statement is used to add new data to a database.
The INSERT INTO statement adds a new record to a table.
INSERT INTO can contain values for some or all of its columns.
INSERT INTO can be combined with a SELECT to insert records.

Syntax:
```sql
INSERT INTO table-name (column-names) 
VALUES (values) 
```
https://www.dofactory.com/sql/insert

#### Update

The UPDATE statement updates data values in a database.
UPDATE can update one or more records in a table.
Use the WHERE clause to UPDATE only specific records.

Syntax:
```sql
UPDATE table-name 
   SET column-name = value, column-name = value, ...
 WHERE condition
 ```

 https://www.dofactory.com/sql/update

 #### Delete

DELETE permanently removes records from a table.
DELETE can delete one or more records in a table.
Use the WHERE clause to DELETE only specific records.

Syntax:
```sql
DELETE table-name 
 WHERE condition
 ```
 https://www.dofactory.com/sql/delete
 
## Simple Query

Syntax:
```sql
SELECT column-names
  FROM table-name
 WHERE condition
```
## Group By 

- The GROUP BY clause groups records into summary rows.
- GROUP BY returns one records for each group.
- GROUP BY typically also involves aggregates: COUNT, MAX, SUM, AVG, etc.
- GROUP BY can group by one or more columns.

Syntax: 
```sql
SELECT column-names
  FROM table-name
 WHERE condition
 GROUP BY column-names
 ```
https://www.dofactory.com/sql/group-by
## Join
 
![Joins](https://www.dofactory.com/Images/sql-joins.png)

Syntax:
```sql
SELECT column-names
  FROM table-name1 JOIN table-name2 
    ON column-name1 = column-name2
 WHERE condition
```

https://www.dofactory.com/sql/join 

## Creating Views

Syntax:
```sql
CREATE VIEW schema_name.view_name [(column_list)]
AS
    select_statement;
```
In this syntax:
  - First, specify the name of the view after the CREATE VIEW keywords. The schema_name is the name of the schema to which the view belongs.
  - Second, specify a SELECT statement (select_statement) that defines the view after the AS keyword. The SELECT statement can refer to one or more tables.

examples....

http://www.sqlservertutorial.net/sql-server-views/sql-server-create-view/

## Stored Procedures

A stored procedure is a prepared SQL code that you can save, so the code can be reused over and over again.
So if you have an SQL query that you write over and over again, save it as a stored procedure, and then just call it to execute it.
You can also pass parameters to a stored procedure, so that the stored procedure can act based on the parameter value(s) that is passed.

Stored Procedure Syntax:
```sql
CREATE PROCEDURE procedure_name
AS
sql_statement
GO;
```
Execute Stored Procedure:
```sql
EXEC procedure_name;
```
Examples: get data, insert data...

## Triggers and Events

Creates a DML, DDL, or logon trigger. A trigger is a special type of stored procedure that automatically runs when an event occurs in the database server. 
DML triggers run when a user tries to modify data through a data manipulation language (DML) event. DML events are INSERT, UPDATE, or DELETE statements on a table or view. 
These triggers fire when any valid event fires, whether table rows are affected or not.

Syntax:
```sql
-- SQL Server Syntax  
-- Trigger on an INSERT, UPDATE, or DELETE statement to a table or view (DML Trigger)  
  
CREATE [ OR ALTER ] TRIGGER [ schema_name . ]trigger_name   
ON { table | view }   
[ WITH <dml_trigger_option> [ ,...n ] ]  
{ FOR | AFTER | INSTEAD OF }   
{ [ INSERT ] [ , ] [ UPDATE ] [ , ] [ DELETE ] }   
[ WITH APPEND ]  
[ NOT FOR REPLICATION ]   
AS { sql_statement  [ ; ] [ ,...n ] | EXTERNAL NAME <method specifier [ ; ] > }  
  
<dml_trigger_option> ::=  
    [ ENCRYPTION ]  
    [ EXECUTE AS Clause ]  
  
<method_specifier> ::=  
    assembly_name.class_name.method_name
```
```sql
-- SQL Server Syntax  
-- Trigger on an INSERT, UPDATE, or DELETE statement to a 
-- table (DML Trigger on memory-optimized tables)  
  
CREATE [ OR ALTER ] TRIGGER [ schema_name . ]trigger_name   
ON { table }   
[ WITH <dml_trigger_option> [ ,...n ] ]  
{ FOR | AFTER }   
{ [ INSERT ] [ , ] [ UPDATE ] [ , ] [ DELETE ] }   
AS { sql_statement  [ ; ] [ ,...n ] }  
  
<dml_trigger_option> ::=  
    [ NATIVE_COMPILATION ]  
    [ SCHEMABINDING ]  
    [ EXECUTE AS Clause ]
```
```sql
-- Trigger on a CREATE, ALTER, DROP, GRANT, DENY, 
-- REVOKE or UPDATE statement (DDL Trigger)  
  
CREATE [ OR ALTER ] TRIGGER trigger_name   
ON { ALL SERVER | DATABASE }   
[ WITH <ddl_trigger_option> [ ,...n ] ]  
{ FOR | AFTER } { event_type | event_group } [ ,...n ]  
AS { sql_statement  [ ; ] [ ,...n ] | EXTERNAL NAME < method specifier >  [ ; ] }  
  
<ddl_trigger_option> ::=  
    [ ENCRYPTION ]  
    [ EXECUTE AS Clause ]
```
```sql
-- Trigger on a LOGON event (Logon Trigger)  
  
CREATE [ OR ALTER ] TRIGGER trigger_name   
ON ALL SERVER   
[ WITH <logon_trigger_option> [ ,...n ] ]  
{ FOR| AFTER } LOGON    
AS { sql_statement  [ ; ] [ ,...n ] | EXTERNAL NAME < method specifier >  [ ; ] }  
  
<logon_trigger_option> ::=  
    [ ENCRYPTION ]  
    [ EXECUTE AS Clause ]
```

Example:
```sql
CREATE TRIGGER production.trg_product_audit
ON production.products
AFTER INSERT, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO production.product_audits(
        product_id, 
        product_name,
        brand_id,
        category_id,
        model_year,
        list_price, 
        updated_at, 
        operation
    )
    SELECT
        i.product_id,
        product_name,
        brand_id,
        category_id,
        model_year,
        i.list_price,
        GETDATE(),
        'INS'
    FROM
        inserted i
    UNION ALL
    SELECT
        d.product_id,
        product_name,
        brand_id,
        category_id,
        model_year,
        d.list_price,
        GETDATE(),
        'DEL'
    FROM
        deleted d;
END
```

## DEMO
