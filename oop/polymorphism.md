# Polymorphism / Đa Hình

## 📖 Định Nghĩa / Definition

**Polymorphism** (nhiều hình dạng) là khả năng của một object để có nhiều dạng khác nhau. Nó cho phép bạn viết code linh hoạt hơn và dễ mở rộng.

**Polymorphism** (many forms) is the ability of an object to take many forms. It allows you to write flexible and extensible code.

---

## 💡 Các Loại Polymorphism / Types of Polymorphism

### 1. **Compile-time Polymorphism** (Static Binding)

Xác định tại thời gian biên dịch / Determined at compile time

#### Method Overloading / Nạp Chồng Method

```csharp
public class Calculator
{
    // Same method name, different parameters
    public int Add(int a, int b)
    {
        return a + b;
    }

    public double Add(double a, double b)
    {
        return a + b;
    }

    public int Add(int a, int b, int c)
    {
        return a + b + c;
    }
}

// Usage
Calculator calc = new Calculator();
Console.WriteLine(calc.Add(5, 10));           // 15
Console.WriteLine(calc.Add(5.5, 10.2));       // 15.7
Console.WriteLine(calc.Add(5, 10, 15));       // 30
```

---

### 2. **Runtime Polymorphism** (Dynamic Binding)

Xác định tại thời gian chạy / Determined at runtime

#### Method Overriding / Ghi Đè Method

```csharp
public class Animal
{
    public virtual void MakeSound()
    {
        Console.WriteLine("Animal makes a sound");
    }
}

public class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Dog barks: Woof Woof!");
    }
}

public class Cat : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Cat meows: Meow!");
    }
}

// Usage
Animal myAnimal = new Dog();
myAnimal.MakeSound();  // Output: Dog barks: Woof Woof!
                       // Gọi Dog version, không phải Animal version

Animal myOtherAnimal = new Cat();
myOtherAnimal.MakeSound();  // Output: Cat meows: Meow!
```

---

## 🎯 Virtual, Override, Abstract / Từ Khóa Quan Trọng

### **virtual** Keyword

```csharp
public class BaseClass
{
    public virtual void Display()
    {
        Console.WriteLine("Base class display");
    }
}

public class DerivedClass : BaseClass
{
    public override void Display()
    {
        Console.WriteLine("Derived class display");
    }
}
```

### **abstract** Keyword

```csharp
public abstract class Shape
{
    // Abstract method - phải implement trong derived class
    public abstract void Draw();

    // Concrete method
    public void Display()
    {
        Console.WriteLine("Displaying shape");
    }
}

public class Circle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing circle");
    }
}
```

---

## 📝 Ví Dụ Thực Tế / Real-World Example

```csharp
public abstract class PaymentProcessor
{
    public abstract void ProcessPayment(decimal amount);

    public virtual void LogTransaction(decimal amount)
    {
        Console.WriteLine($"Payment of {amount} processed");
    }
}

public class CreditCardProcessor : PaymentProcessor
{
    public override void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment: ${amount}");
    }
}

public class PayPalProcessor : PaymentProcessor
{
    public override void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment: ${amount}");
    }

    public override void LogTransaction(decimal amount)
    {
        base.LogTransaction(amount);
        Console.WriteLine("PayPal fee applied");
    }
}

// Usage
PaymentProcessor processor = new CreditCardProcessor();
processor.ProcessPayment(100);
```

---

## ✅ Best Practices

1. **Dùng virtual cho methods có thể được override**
2. **Dùng abstract class cho common functionality**
3. **Dùng interface cho contracts**
4. **Tránh deep inheritance hierarchies**
5. **Luôn document virtual methods**

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Quên 'virtual' keyword
public class Base
{
    public void Method() { }  // Cannot be overridden
}

// ✅ CORRECT
public class Base
{
    public virtual void Method() { }  // Can be overridden
}
```

---

## 🌍 Real-World Scenarios / Tình Huống Thực Tế

### Logging với Nhiều Provider

```csharp
public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}

public class FileLogger : ILogger
{
    public void Log(string message) => File.AppendAllText("app.log", message + "\n");
}

public class App
{
    private readonly ILogger _logger;
    public App(ILogger logger) => _logger = logger;
    public void Run() => _logger.Log("App started");
}
```

### Chiến Lược Tính Phí Giao Hàng

```csharp
public interface IShippingStrategy
{
    decimal Calculate(decimal weightKg, string destination);
}

public class StandardShipping : IShippingStrategy
{
    public decimal Calculate(decimal weightKg, string destination) => 5 + weightKg * 1.2m;
}

public class ExpressShipping : IShippingStrategy
{
    public decimal Calculate(decimal weightKg, string destination) => 15 + weightKg * 2.5m;
}

public class ShippingService
{
    public decimal GetFee(IShippingStrategy strategy, decimal weightKg, string destination)
    {
        return strategy.Calculate(weightKg, destination);
    }
}
```

### UI Rendering Tùy Nền Tảng

```csharp
public abstract class ViewRenderer
{
    public abstract void Render(string content);
}

public class HtmlRenderer : ViewRenderer
{
    public override void Render(string content)
    {
        Console.WriteLine($"<div>{content}</div>");
    }
}

public class MarkdownRenderer : ViewRenderer
{
    public override void Render(string content)
    {
        Console.WriteLine($"**{content}**");
    }
}
```

---

## 🎓 Interview Questions & Answers / Câu Hỏi Phỏng Vấn & Trả Lời

### 1. **Giải thích compile-time vs runtime polymorphism?**

**Trả lời:**

- **Compile-time Polymorphism (Static Binding):**
  - Xác định **tại lúc biên dịch** method nào sẽ gọi.
  - Dùng **Method Overloading** - cùng tên method, khác parameters.
  - Compiler biết chính xác gọi phiên bản nào dựa vào tham số.
  - ✅ Nhanh, an toàn kiểu, rõ ràng.

```csharp
public class Math
{
    public int Add(int a, int b) => a + b;
    public double Add(double a, double b) => a + b;
}

Math.Add(5, 10);      // Gọi int version (compile-time biết)
Math.Add(5.5, 10.2);  // Gọi double version
```

- **Runtime Polymorphism (Dynamic Binding):**
  - Xác định **tại lúc chạy** method nào sẽ gọi.
  - Dùng **Method Overriding** - dùng `virtual` + `override`.
  - Gọi method dựa trên **actual type** của object, không compile-time type.
  - ✅ Linh hoạt, extensible.

```csharp
Animal dog = new Dog();      // Compile-time: Animal, Runtime: Dog
dog.MakeSound();             // Gọi Dog.MakeSound() (runtime biết)

Animal cat = new Cat();
cat.MakeSound();             // Gọi Cat.MakeSound()
```

---

### 2. **Khi nào dùng virtual, khi nào dùng abstract?**

**Trả lời:**

| Loại         | Khi Nào                                            | Tính Chất                               |
| ------------ | -------------------------------------------------- | --------------------------------------- |
| **virtual**  | Có implementation mặc định nhưng cho phép override | Method có thân, optional override       |
| **abstract** | Không có implementation, bắt buộc implement        | Method không có thân, bắt buộc override |

**Ví dụ:**

```csharp
public abstract class Vehicle
{
    // Abstract: phải implement, không có default
    public abstract void Start();

    // Virtual: có default, nhưng có thể override
    public virtual void Stop() => Console.WriteLine("Stopped");
}

public class Car : Vehicle
{
    public override void Start() => Console.WriteLine("Car started");

    public override void Stop() => Console.WriteLine("Car stopped safely");
}
```

**Quyết định:**

- Dùng **abstract** khi subclass PHẢI định nghĩa riêng behavior (no default).
- Dùng **virtual** khi có default behavior nhưng cho phép customize.

---

### 3. **Method overloading vs method overriding là gì?**

**Trả lời:**

| Khía Cạnh        | Overloading              | Overriding                       |
| ---------------- | ------------------------ | -------------------------------- |
| **Định nghĩa**   | Cùng tên, khác parameter | Cùng signature, base vs derived  |
| **Từ khóa**      | Không cần                | Cần `virtual` + `override`       |
| **Khi xác định** | Compile-time (static)    | Runtime (dynamic)                |
| **Inheritance**  | Không cần                | Cần kế thừa                      |
| **Mục đích**     | Cùng logic, input khác   | Behavior khác theo từng subclass |

**Ví dụ:**

```csharp
// OVERLOADING - Compile-time
public class Printer
{
    public void Print(string text) => Console.WriteLine(text);
    public void Print(int number) => Console.WriteLine(number);
    public void Print(string text, int count)
    {
        for (int i = 0; i < count; i++) Console.WriteLine(text);
    }
}

// OVERRIDING - Runtime
public class Animal
{
    public virtual void MakeSound() => Console.WriteLine("Some sound");
}

public class Dog : Animal
{
    public override void MakeSound() => Console.WriteLine("Woof!");
}
```

---

### 4. **Lợi ích của polymorphism là gì?**

**Trả lời:**

1. **Flexibility / Linh hoạt:**
   - Viết code làm việc với base type, nhưng chạy behavior của derived type.

   ```csharp
   List<Animal> animals = new() { new Dog(), new Cat() };
   foreach (var animal in animals)
       animal.MakeSound();  // Mỗi con vật kêu khác nhau
   ```

2. **Extensibility / Mở rộng:**
   - Thêm loại mới mà không sửa code cũ.

   ```csharp
   public class Bird : Animal
   {
       public override void MakeSound() => Console.WriteLine("Tweet!");
   }
   // Code cũ vẫn hoạt động mà không thay đổi!
   ```

3. **Maintainability / Dễ bảo trì:**
   - Giảm coupling, code phụ thuộc abstract type, không concrete.

4. **Reusability / Tái sử dụng:**
   - Logic chung ở base class, các subclass specialized.

---

### 5. **Có thể override một static method không?**

**Trả lời:**

- **KHÔNG**, không thể override static method.
- Static method gọi dựa trên **compile-time type**, không runtime type.
- Khi override static, thực chất là **hide** method (che giấu), không thực sự override.

**Ví dụ:**

```csharp
public class Base
{
    public static void Hello() => Console.WriteLine("Base Hello");
}

public class Derived : Base
{
    public static new void Hello() => Console.WriteLine("Derived Hello");
}

Base obj = new Derived();
obj.Hello();  // Output: "Base Hello" (gọi theo compile-time type Base)

// Phải gọi trực tiếp lớp:
Derived.Hello();  // Output: "Derived Hello"
```

**Tại sao?** Static method không liên kết instance, nên không có dynamic dispatch như virtual methods.

---

## 📚 Related Topics

- [Inheritance](inheritance.md)
- [Encapsulation](encapsulation.md)
- [SOLID Principles](../solid-principles/)
