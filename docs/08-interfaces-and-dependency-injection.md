# Introduction

## Intefaces
An interface contains definitions for a group of related functionalities that a non-abstract class or a struct must implement.
An interface is like a contract. In the human world, the contract between the two or more humans binds them to act as per the contract. 
In the same way, the interface includes the declaration of one or more functionalities. 
Entities that implement the interface must define functionalities declared in the interface. 
In C#, a class or a struct can implement one or more interfaces.

```csharp
interface IEquatable<T>
{
    bool Equals(T obj);
}

public class Car : IEquatable<Car>
{
    public string Make {get; set;}
    public string Model { get; set; }
    public string Year { get; set; }

    // Implementation of IEquatable<T> interface
    public bool Equals(Car car)
    {
        return this.Make == car.Make &&
               this.Model == car.Model &&
               this.Year == car.Year;
    }
}
```

An interface is like an abstract base class with only abstract members. Any class or struct that implements the interface must implement all its members.
An interface can't be instantiated directly. Its members are implemented by any class or struct that implements the interface.
Interfaces can contain methods, properties, events, indexers, or any combination of those four member types.
Interfaces contain no implementation of methods.
A class or struct can implement multiple interfaces. A class can inherit a base class and also implement one or more interfaces.

|Abstract Classes|Interfaces|
|:----------------:|:----------------:|
|May contain implementation code|May not contain implementation code|
|A class may inherit from a single base class|A class may implement any number of interfaces|
|Members have access modifiers|Members are automatically public|
|May contain fields, properties, constructors, destructors, methods, events and indexers|May only contain properties, methods, events, and indexers|

## Inteface Segregation Principle

## Inversion of Control 

## Dependency Injection
