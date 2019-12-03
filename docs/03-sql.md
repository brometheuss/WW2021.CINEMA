# Introduction

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

## SQL vs NoSQL

## Insert, Update and Delete

## Simple Query

## Group By 

## Join
    - inner 
    - outter 

## Creating Views

## Stored Procedures

## Triggers and Events

