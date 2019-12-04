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

Clients should not be forced to depend upon interfaces that they do not use.

This principle is very much related to the Single Responsibility Principle. 
What it really means is that you should always design your abstractions in a way that the clients that are using the exposed methods do not get the whole pie instead. 
That also include imposing the clients with the burden of implementing methods that they don’t actually need.

```csharp
public interface IVehicle
{
    void Drive();
    void Fly();
}
```
```csharp
public class MultiFunctionalCar : IVehicle
{
    public void Drive()
    {
        //actions to start driving car
        Console.WriteLine("Drive a multifunctional car");
    }
 
    public void Fly()
    {
        //actions to start flying
        Console.WriteLine("Fly a multifunctional car");
    }
}
```
```csharp
public class Car : IVehicle
{
    public void Drive()
    {
        //actions to drive a car
        Console.WriteLine("Driving a car");
    }
 
    public void Fly()
    {
        throw new NotImplementedException();
  
```
```csharp
public class Airplane : IVehicle
{
    public void Drive()
    {
        throw new NotImplementedException();
    }
 
    public void Fly()
    {
        //actions to fly a plane
        Console.WriteLine("Flying a plane");
    }
}
```
SOLUTION:
```csharp
public interface ICar
{
    void Drive();
}
```
```csharp
public interface IAirplane
{
    void Fly();
}
```

```csharp
public class Car : ICar
{
    public void Drive()
    {
        //actions to drive a car
        Console.WriteLine("Driving a car");
    }
}
```
```csharp
public class Airplane : IAirplane
{
    public void Fly()
    {
        //actions to fly a plane
        Console.WriteLine("Flying a plane");
    }
}
```
```csharp
public class MultiFunctionalCar : ICar, IAirplane
{
    public void Drive()
    {
        //actions to start driving car
        Console.WriteLine("Drive a multifunctional car");
    }
 
    public void Fly()
    {
        //actions to start flying
        Console.WriteLine("Fly a multifunctional car");
    }
}
```
```csharp
public interface IMultiFunctionalVehicle : ICar, IAirplane
{
}
```
```csharp
public class MultiFunctionalCar : IMultiFunctionalVehicle
{
    public void Drive()
    {
        //actions to start driving car
        Console.WriteLine("Drive a multifunctional car");
    }
 
    public void Fly()
    {
        //actions to start flying
        Console.WriteLine("Fly a multifunctional car");
    }
}
```

We can see from the example above, that smaller interface is a lot easier to implement due to not having to implement methods that our class doesn’t need.
Another benefit is that the Interface Segregation Principle increases the readability and maintainability of our code. 
We are reducing our class implementation only to required actions without any additional or unnecessary code.

To sum this article up, we should put an effort into creating smaller interfaces while developing our project. 
Yes, we may end up with a lot of different interfaces in the end but from our point of view, this is much better 
than having a few large interfaces that can force us to implement non-required methods in our classes.

## Inversion of Control 

Inversion of Control (IoC) is a design principle (although, some people refer to it as a pattern). As the name suggests, it is used to 
invert different kinds of controls in object-oriented design to achieve loose coupling. Here, controls refer to any additional responsibilities a
class has, other than its main responsibility.  This include control over the flow of an application, and control over the flow of an 
object creation or dependent object creation and binding.
IoC is all about inverting the control.

```csharp
public class A
{
    B b;

    public A()
    {
        b = new B();
    }

    public void Task1() {
        // do something here..
        b.SomeMethod();
        // do something here..
    }

}

public class B {

    public void SomeMethod() { 
        //doing something..
    }
}
```
In the above example, class A calls b.SomeMethod() to complete its task1. 
Class A cannot complete its task without class B and so you can say that "Class A is dependent on class B" or "class B is a dependency of class A".

```csharp
public class A
{
    B b;

    public A()
    {
        b = Factory.GetObjectOfB ();
    }

    public void Task1() {
        // do something here..
        b.SomeMethod();
        // do something here..
    }
}

public class Factory
{
    public static B GetObjectOfB() 
    {
        return new B();
    }
}
```
As you can see above, class A uses Factory class to get an object of class B. Thus, we have inverted the dependent object creation from class A to Factory. 
Class A no longer creates an object of class B, instead it uses the factory class to get the object of class B.
In an object-oriented design, classes should be designed in a loosely coupled way. Loosely coupled means changes in one class should not force other classes to change, 
so the whole application can become maintainable and extensible. 
Let's understand this by using typical n-tier architecture as depicted by the following figure:
![a](https://dev.azure.com/divanisevic/_git/WinterWorkShop.Cinema/docs/images/demo-architecture.png)

## Dependency Injection
