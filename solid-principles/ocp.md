# Open/Closed Principle (OCP) / Nguyên Tắc Mở/Đóng

## 📖 Định Nghĩa / Definition

**OCP** - Software entities (classes, modules) nên **mở (open) để mở rộng** nhưng **đóng (closed) để sửa đổi**.

**OCP** - Software entities should be **open for extension** but **closed for modification**.

---

## 💡 Khái Niệm / Concept

Bạn nên mở rộng functionality **bằng cách thêm mã mới**, không phải **bằng cách sửa mã cũ**.

You should extend functionality **by adding new code**, not by **modifying existing code**.

```
❌ BAD: Mỗi lần thêm feature phải sửa class cũ
✅ GOOD: Thêm feature bằng cách tạo class mới (extension)
```

---

## 📝 Ví Dụ / Examples

### ❌ Violation (Vi Phạm OCP)

```csharp
// BAD - Phải sửa class mỗi lần thêm payment method
public class PaymentProcessor
{
    public void ProcessPayment(string paymentMethod, decimal amount)
    {
        if (paymentMethod == "CreditCard")
        {
            // Logic thẻ tín dụng
            Console.WriteLine($"Processing credit card: ${amount}");
        }
        else if (paymentMethod == "PayPal")
        {
            // Logic PayPal
            Console.WriteLine($"Processing PayPal: ${amount}");
        }
        else if (paymentMethod == "Bitcoin")
        {
            // Logic Bitcoin
            Console.WriteLine($"Processing Bitcoin: ${amount}");
        }
        // Mỗi lần thêm payment method mới, phải sửa method này!
    }
}

// Vấn đề:
// - Phải sửa code cũ (vi phạm OCP)
// - Risk: break existing code
// - Không scalable
```

### ✅ Applying OCP

```csharp
// Strategy 1: Interface
public interface IPaymentMethod
{
    void ProcessPayment(decimal amount);
}

public class CreditCardPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing credit card: ${amount}");
    }
}

public class PayPalPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing PayPal: ${amount}");
    }
}

public class BitcoinPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing Bitcoin: ${amount}");
    }
}

// PaymentProcessor - không cần sửa
public class PaymentProcessor
{
    public void ProcessPayment(IPaymentMethod paymentMethod, decimal amount)
    {
        paymentMethod.ProcessPayment(amount);
    }
}

// Sử dụng:
IPaymentMethod creditCard = new CreditCardPayment();
processor.ProcessPayment(creditCard, 100);

// Thêm payment method mới:
public class ApplePayPayment : IPaymentMethod
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing Apple Pay: ${amount}");
    }
}
// PaymentProcessor không cần sửa!
```

---

## 🎯 Real-World Example / Ví Dụ Thực Tế

### ❌ Before OCP

```csharp
public class ReportGenerator
{
    public void GenerateReport(string type)
    {
        if (type == "PDF")
        {
            // Generate PDF
            Console.WriteLine("Generating PDF report");
        }
        else if (type == "Excel")
        {
            // Generate Excel
            Console.WriteLine("Generating Excel report");
        }
        else if (type == "HTML")
        {
            // Generate HTML
            Console.WriteLine("Generating HTML report");
        }
    }
}
```

### ✅ After OCP

```csharp
public interface IReportFormatter
{
    void FormatReport(List<string> data);
}

public class PdfReportFormatter : IReportFormatter
{
    public void FormatReport(List<string> data)
    {
        Console.WriteLine("Generating PDF report");
    }
}

public class ExcelReportFormatter : IReportFormatter
{
    public void FormatReport(List<string> data)
    {
        Console.WriteLine("Generating Excel report");
    }
}

public class HtmlReportFormatter : IReportFormatter
{
    public void FormatReport(List<string> data)
    {
        Console.WriteLine("Generating HTML report");
    }
}

public class ReportGenerator
{
    public void GenerateReport(IReportFormatter formatter, List<string> data)
    {
        formatter.FormatReport(data);
    }
}
```

---

## ✅ Techniques / Kỹ Thuật

### 1. **Strategy Pattern**

```csharp
public interface IStrategy
{
    void Execute();
}

public class Strategy1 : IStrategy
{
    public void Execute() { }
}

public class Context
{
    private IStrategy _strategy;

    public void SetStrategy(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public void Execute()
    {
        _strategy.Execute();
    }
}
```

### 2. **Template Method Pattern**

```csharp
public abstract class DataProcessor
{
    public void Process()
    {
        ValidateData();
        ProcessData();  // Abstract - khác nhau ở subclass
        SaveData();
    }

    protected abstract void ProcessData();

    protected virtual void ValidateData() { }
    protected virtual void SaveData() { }
}

public class CsvProcessor : DataProcessor
{
    protected override void ProcessData()
    {
        // CSV-specific processing
    }
}
```

### 3. **Decorator Pattern**

```csharp
public interface IComponent
{
    void Operation();
}

public class ConcreteComponent : IComponent
{
    public void Operation() { }
}

public abstract class Decorator : IComponent
{
    protected IComponent _component;

    public virtual void Operation()
    {
        _component.Operation();
    }
}

public class ConcreteDecorator : Decorator
{
    public override void Operation()
    {
        base.Operation();
        // Additional behavior
    }
}
```

---

## ✅ Benefits / Lợi Ích

| Lợi Ích             | Mô Tả                           |
| ------------------- | ------------------------------- |
| **Safer Changes**   | Không cần sửa code cũ = ít risk |
| **Scalability**     | Dễ thêm feature mới             |
| **Maintainability** | Code cũ không bị modify         |
| **Testability**     | Dễ test implementation mới      |
| **Flexibility**     | Dễ swap implementations         |

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Switch statement
public class Processor
{
    public void Process(Type type)
    {
        switch(type)
        {
            case Type.A: ProcessA(); break;
            case Type.B: ProcessB(); break;
        }
    }
}

// ✅ CORRECT - Use interface/inheritance
public interface IProcessor
{
    void Process();
}

public class ProcessorA : IProcessor { }
public class ProcessorB : IProcessor { }
```

---

## 🎓 Interview Questions

1. **OCP là gì? Tại sao quan trọng?**
2. **Open for extension, closed for modification là gì?**
3. **Làm sao implement OCP?**
4. **OCP vs SRP khác gì?**
5. **Khi nào không nên áp dụng OCP?**

---

## 📚 Related Topics

- [Single Responsibility Principle](srp.md)
- [Dependency Inversion Principle](dip.md)
- [Design Patterns](../design-patterns/)
