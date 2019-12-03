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

## Inteface Segregation Principle

## Inversion of Control 

## Dependency Injection
