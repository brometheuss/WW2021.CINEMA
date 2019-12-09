#Introduction

C# syntax is highly expressive, yet it is also simple and easy to learn. The curly-brace syntax of C# will be instantly recognizable to anyone familiar with C, C++ or Java.
Developers who know any of these languages are typically able to begin to work productively in C# within a very short time. 
C# syntax simplifies many of the complexities of C++ and provides powerful features such as nullable value types, enumerations, delegates, 
lambda expressions and direct memory access, which are not found in Java. C# supports generic methods and types, which provide increased type safety and performance,
and iterators, which enable implementers of collection classes to define custom iteration behaviors that are simple to use by client code. 

As an object-oriented language, C# supports the concepts of encapsulation, inheritance, and polymorphism. All variables and methods, including the Main method, 
the application's entry point, are encapsulated within class definitions. A class may inherit directly from one parent class, but it may implement any number of interfaces. 
Methods that override virtual methods in a parent class require the override keyword as a way to avoid accidental redefinition. 

## OOP

### OOP represent

*   Abstraction – Refers to the process of exposing only the relevant and essential data to the user without showing unnecessary information.
*   Polymorphism – Allows you to use an entity in multiple forms.
*   Encapsulation – Prevents the data from unwanted access by binding of code and data in a single unit called object.
*   Inheritance – Promotes the reusability of code and eliminate the use of redundant code. 
    It is the property through which a child class obtains all the features defined in its parent class. 

## SOLID

### SOLID represent

*   S – Single-responsibility principle – A class should have one and only one reason to change, meaning that a class should have only one job.
*   O – Open-closed principle – Objects or entities should be open for extension, but closed for modification.
*   L – Liskov substitution principle – Let q(x) be a property provable about objects of x of type T. 
    Then q(y) should be provable for objects y of type S where S is a subtype of T.
*   I – Interface segregation principle – A client should never be forced to implement an interface that it doesn’t use 
    or clients shouldn’t be forced to depend on methods they do not use.
*   D – Dependency Inversion Principle – Entities must depend on abstractions not on concretions. 
    It states that the high level module must not depend on the low level module, but they should depend on abstractions.

## Class and Object

### What is class?

A class describes all the attributes of object, as well as the methods that implement the behavior of member objects. 
It is a comprehensive data type, which represent a blue print of objects. It is a template of object. 

### What is object?

Objects are instances of classes. An object is an entity that has attributes, behavior and identity. 
Attributes and behavior of an object are defined by the class definition. 

### Relationship between class and an object?

A class acts as a blue-print that defines properties, states and behaviors that are common to a number of objects.
An object is an instance of the class.

## Constructor and Destructor

###	Explain the concept of constructor?
Constructor is a special method of a class, which is called automatically when the instance of class is created. 
It is created with the same name as the class and initializes all class members whenever you access the class. Main features of constructor are:
*	Constructors do not have any return type.
*   Constructor can be overloaded.

### Explain the concept of destructor?

A destructor is a special method for a class and is invoked automatically when an object is finally destroyed. 
The name of destructor is same as the class name but with prefix tilde (~). Destructor is used to free the dynamic allocated memory. Main features of destructor are:
*   Destructors do not have any return type.
*   Destructors are always public.
*   Destructors cannot be overloaded. 

## Properties

### Give a brief description of properties in C#?

In C#, a property is a way to expose an internal data element of a class. 
You can create a property by defining an externally available name and then writing the “set” and “get” property accessors.

## Method overriding/overloading

### What are methods?

Method is a block of code that contains a series of statements and represents the behavior of a class. 
While declaring a method we need to specify the access specifier, return type, name of the method, and the method parameters.

### What is method overloading?

Method Overloading is the common way of implementing polymorphism. It is the ability to redefine a function in more than one form. 
A user can implement function overloading by defining two or more functions in a class sharing the same name. 
C# can distinguish the methods with different method signatures.

Method overloading can be done by changing:
*   The number of parameters.
*   The data types of the parameters of methods.
*   The Order of the parameters of methods.

```csharp
// C# program to demonstrate the function 
// overloading by changing the Number 
// of parameters 

class GFG { 

	// adding two integer values. 
	public int Add(int x, int y) 
	{ 
		int sum = x + y; 
		return sum; 
	} 

	// adding three integer values. 
	public int Add(int x, int y, int z) 
	{ 
		int sum = x + y + z; 
		return sum; 
	} 

	// Main Method 
	public static void Main(String[] args) 
	{ 

		// Creating Object 
		GFG ob = new GFG(); 

		int sum1 = ob.Add(1, 2); 
		Console.WriteLine("sum of the two "
						+ "integer value : " + sum1); 

		int sum2 = ob.Add(1, 2, 3); 
		Console.WriteLine("sum of the three "
						+ "integer value : " + sum2);
    }
}
```

### What is method overriding?

Method Overriding is a technique that allows the invoking of functions from another class (base class) in the derived class. 
Creating a method in the derived class with the same signature as a method in the base class is called as method overriding.
In simple words, Overriding is a feature that allows a subclass or child class to provide a specific implementation of a method that is already provided by
one of its super-classes or parent classes. When a method in a subclass has the same name, 
same parameters or signature and same return type(or sub-type) as a method in its super-class, 
then the method in the subclass is said to override the method in the super-class.

Keywords in method overriding:
*   Virtual - base class method which is going to be override.
*   Override - sub class method which is overriding base class method.
*   Base - By using base keyword, we can call a base class method that has been overridden by another method in derived class.

```csharp
// C# program to demonstrate the function 
// overriding 

    class baseClass  
    {  
        public virtual void Greetings()  
        {  
            Console.WriteLine("baseClass Saying Hello!");  
        }  
    }  
    class subClass : baseClass  
    {  
        public override void Greetings()  
        {  
            base.Greetings();  
            Console.WriteLine("subClass Saying Hello!");  
        }  
    }  
    class Program  
    {  
        static void Main(string[] args)  
        {  
            baseClass obj1 = new baseClass();  
            obj1.Greetings();

            subClass obj2 = new subClass();  
            obj2.Greetings();

            Console.ReadLine();  
        }  
    }  
```

## Lambda Expression

### What are Lambda expressions?

Lambda expressions in C# are used like anonymous functions, with the difference that in Lambda expressions you don’t need to specify the type of the value
that you input thus making it more flexible to use.
The **‘=>’** is the lambda operator which is used in all lambda expressions. 
The Lambda expression is divided into two parts, the left side is the input and the right is the expression.

We have two types of Lambda expressions:
*   Expression Lambda - Consists of the input and the expression
```csharp
input => expression;
```
*   statement Lambda - Consists of the input and a set of statements to be executed
```csharp
input => { statements };
```

### Example 

```csharp
// C# program to illustrate the 
// Lambda Expression 
using System; 
using System.Collections.Generic; 
using System.Linq; 

namespace Lambda_Exressions { 
class Program { 
	static void Main(string[] args) 
	{ 
		// List to store numbers 
		List<int> numbers = new List<int>() {36, 71, 12, 
							15, 29, 18, 27, 17, 9, 34}; 

		// foreach loop to dislay the list 
		Console.Write("The list : "); 
		foreach(var value in numbers) 
		{ 
			Console.Write("{0} ", value); 
		} 
		Console.WriteLine(); 

		// Using lambda expression 
		// to calculate square of 
		// each value in the list 
		var square = numbers.Select(x => x * x); 

		// foreach loop to display squares 
		Console.Write("Squares : "); 
		foreach(var value in square) 
		{ 
			Console.Write("{0} ", value); 
		} 
		Console.WriteLine(); 

		// Using Lambda exression to 
		// find all numbers in the list 
		// divisible by 3 
		List<int> divBy3 = numbers.FindAll(x => (x % 3) == 0); 

		// foreach loop to display divBy3 
		Console.Write("Numbers Divisible by 3 : "); 
		foreach(var value in divBy3) 
		{ 
			Console.Write("{0} ", value); 
		} 
		Console.WriteLine(); 
	} 
} 
} 
```

