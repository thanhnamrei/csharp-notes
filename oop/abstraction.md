# Abstraction / Trừu Tượng Hóa

## 📖 Định Nghĩa / Definition

**Abstraction** là nguyên tắc che giấu độ phức tạp và chỉ hiển thị các tính năng cần thiết. Nó giúp bạn tập trung vào "cái gì" thay vì "như thế nào".

**Abstraction** is the principle of hiding complexity and showing only essential features. It helps focus on "what" rather than "how".

---

## 💡 Abstract Classes vs Interfaces

| Tính Năng        | Abstract Class                | Interface                              |
| ---------------- | ----------------------------- | -------------------------------------- |
| Constructor      | ✅ Yes                        | ❌ No                                  |
| Fields           | ✅ Yes                        | ❌ No                                  |
| Access Modifiers | ✅ public, private, protected | ✅ public (default)                    |
| Implementation   | ✅ Partial                    | ❌ No (C# 8.0+ default implementation) |
| Inheritance      | Single                        | Multiple                               |
| Use Case         | "IS-A" relationship           | "CAN-DO" capability                    |

---

## 📝 Abstract Classes / Lớp Trừu Tượng

### Syntax & Example

```csharp
// ❌ Cannot instantiate abstract class
public abstract class Vehicle
{
    // Abstract method - must be implemented by derived class
    public abstract void StartEngine();

    // Abstract property
    public abstract string Model { get; }

    // Concrete method - default implementation
    public void Stop()
    {
        Console.WriteLine("Engine stopped");
    }
}

public class Car : Vehicle
{
    public override string Model { get; } = "Toyota";

    public override void StartEngine()
    {
        Console.WriteLine("Car engine started");
    }
}

// Usage
// Vehicle vehicle = new Vehicle();  // ❌ ERROR
Vehicle car = new Car();  // ✅ OK
car.StartEngine();
```

---

## 📝 Interfaces / Giao Diện

### Syntax & Example

```csharp
public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount);
    bool IsPaymentSuccessful { get; }
}

public class CreditCardProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} via credit card");
    }

    public bool IsPaymentSuccessful { get; } = true;
}

// Usage
IPaymentProcessor processor = new CreditCardProcessor();
processor.ProcessPayment(100);
```

---

## 🎯 Abstract Class Example / Ví Dụ Thực Tế

```csharp
public abstract class Shape
{
    // Abstract method
    public abstract double GetArea();

    // Abstract property
    public abstract string Name { get; }

    // Concrete method
    public void PrintInfo()
    {
        Console.WriteLine($"{Name} - Area: {GetArea()}");
    }
}

public class Circle : Shape
{
    private double _radius;

    public Circle(double radius)
    {
        _radius = radius;
    }

    public override string Name => "Circle";

    public override double GetArea()
    {
        return Math.PI * _radius * _radius;
    }
}

public class Rectangle : Shape
{
    private double _width, _height;

    public Rectangle(double width, double height)
    {
        _width = width;
        _height = height;
    }

    public override string Name => "Rectangle";

    public override double GetArea()
    {
        return _width * _height;
    }
}

// Usage
Shape shape1 = new Circle(5);
shape1.PrintInfo();  // Circle - Area: 78.54

Shape shape2 = new Rectangle(4, 5);
shape2.PrintInfo();  // Rectangle - Area: 20
```

---

## 🎯 Interface Example / Ví Dụ Interface

```csharp
public interface ILogger
{
    void Log(string message);
    void LogError(string error);
}

public interface IRepository<T>
{
    T GetById(int id);
    void Add(T entity);
    void Delete(int id);
}

public class FileLogger : ILogger
{
    public void Log(string message)
    {
        File.AppendAllText("log.txt", message + "\n");
    }

    public void LogError(string error)
    {
        File.AppendAllText("errors.txt", error + "\n");
    }
}

public class UserRepository : IRepository<User>
{
    public User GetById(int id) => new User { Id = id };
    public void Add(User entity) => Console.WriteLine("User added");
    public void Delete(int id) => Console.WriteLine("User deleted");
}
```

---

## 🎯 Abstract Members / Các Thành Viên Trừu Tượng

### Abstract Methods

```csharp
public abstract class DataProcessor
{
    public abstract void Process();
    public abstract string GetResult();
}
```

### Abstract Properties

```csharp
public abstract class Employee
{
    public abstract string Id { get; }
    public abstract decimal Salary { get; set; }
}
```

### Abstract Events (C# 8.0+)

```csharp
public abstract class EventPublisher
{
    public abstract event EventHandler OnEvent;
}
```

---

## ✅ When to Use Abstract / Khi Nào Dùng

### Use Abstract Class When:

- Bạn muốn chia sẻ common code
- Bạn cần non-public members
- Bạn muốn define state (fields)
- Relationships are IS-A

### Use Interface When:

- Bạn muốn định nghĩa contract
- Bạn muốn multiple inheritance
- Bạn cần public methods chỉ
- Relationships are CAN-DO

---

## ✅ Best Practices

1. **Keep abstract classes focused** - Không quá nhiều abstract members
2. **Provide default implementations** - Khi có thể
3. **Use interfaces for contracts** - Định nghĩa hành vi
4. **Document abstract members** - Giải thích rõ ràng
5. **Avoid deep hierarchies** - Giữ cây inheritance nông

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Tất cả đều abstract
public abstract class Base
{
    public abstract void Method1();
    public abstract void Method2();
    public abstract void Method3();
}

// ✅ CORRECT - Có default implementation
public abstract class Base
{
    public abstract void Method1();  // Specific to derived

    public virtual void Method2()    // Common behavior
    {
        Console.WriteLine("Default implementation");
    }
}
```

---

## 🌍 Real-World Scenarios / Tình Huống Thực Tế

### 1) Email Sender Abstraction

```csharp
public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body);
}

public class SendGridEmailSender : IEmailSender
{
    public Task SendAsync(string to, string subject, string body)
    {
        // Call SendGrid API
        return Task.CompletedTask;
    }
}

public class SmtpEmailSender : IEmailSender
{
    public Task SendAsync(string to, string subject, string body)
    {
        // Use SmtpClient
        return Task.CompletedTask;
    }
}
```

### 2) Background Job Template

```csharp
public abstract class BackgroundJob
{
    public async Task RunAsync()
    {
        await BeforeAsync();
        await ExecuteAsync();
        await AfterAsync();
    }

    protected virtual Task BeforeAsync() => Task.CompletedTask;
    protected abstract Task ExecuteAsync();
    protected virtual Task AfterAsync() => Task.CompletedTask;
}

public class SendEmailJob : BackgroundJob
{
    protected override Task ExecuteAsync()
    {
        // Send emails here
        return Task.CompletedTask;
    }
}
```

### 3) Storage Provider Contract

```csharp
public interface IBlobStorage
{
    Task UploadAsync(string path, Stream content);
    Task<Stream> DownloadAsync(string path);
}

public class AzureBlobStorage : IBlobStorage
{
    public Task UploadAsync(string path, Stream content) { return Task.CompletedTask; }
    public Task<Stream> DownloadAsync(string path) { return Task.FromResult<Stream>(new MemoryStream()); }
}

public class S3Storage : IBlobStorage
{
    public Task UploadAsync(string path, Stream content) { return Task.CompletedTask; }
    public Task<Stream> DownloadAsync(string path) { return Task.FromResult<Stream>(new MemoryStream()); }
}
```

---

## 🎓 Interview Questions

1. **Abstract class vs Interface là gì? Khi nào dùng?**
2. **Có thể instantiate abstract class không?**
3. **Lợi ích của abstraction là gì?**
4. **Có thể có abstract method mà không có implementation không?**
5. **C# hỗ trợ multiple inheritance không? Giải pháp?**

---

## 📚 Related Topics

- [Inheritance](inheritance.md)
- [Polymorphism](polymorphism.md)
- [Encapsulation](encapsulation.md)
- [SOLID Principles](../solid-principles/)
