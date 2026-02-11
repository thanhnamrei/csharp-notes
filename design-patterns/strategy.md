# Strategy Pattern / Mẫu Chiến Lược

## 📖 Định Nghĩa / Definition

**Strategy** là design pattern định nghĩa **một family của algorithms**, đóng gói mỗi cái riêng biệt, và làm chúng **interchangeable** (có thể thay đổi được).

**Strategy** is a design pattern that defines a **family of algorithms**, encapsulates each one separately, and makes them **interchangeable**.

---

## 💡 Khi Nào Dùng / When to Use

✅ Sorting Algorithms - Quick Sort, Merge Sort, Bubble Sort...
✅ Payment Methods - Credit Card, PayPal, Bitcoin...
✅ Compression - ZIP, RAR, 7z...
✅ Route Finding - Shortest Path, Fastest Route...
✅ Caching Strategies - LRU, FIFO, LFU...
✅ Discount Calculations - Student, Employee, VIP discounts...

---

## 📝 Implementations / Các Cách Implement

### 1. **Classic Strategy Pattern**

```csharp
// Strategy Interface
public interface IPaymentStrategy
{
    void Pay(decimal amount);
}

// Concrete Strategies
public class CreditCardPayment : IPaymentStrategy
{
    private string _cardNumber;

    public CreditCardPayment(string cardNumber)
    {
        _cardNumber = cardNumber;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paying ${amount} with Credit Card {_cardNumber}");
    }
}

public class PayPalPayment : IPaymentStrategy
{
    private string _email;

    public PayPalPayment(string email)
    {
        _email = email;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paying ${amount} via PayPal ({_email})");
    }
}

public class CryptoPayment : IPaymentStrategy
{
    private string _walletAddress;

    public CryptoPayment(string walletAddress)
    {
        _walletAddress = walletAddress;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paying ${amount} with Crypto to wallet {_walletAddress}");
    }
}

// Context
public class ShoppingCart
{
    private IPaymentStrategy _paymentStrategy;
    private decimal _total;

    public ShoppingCart(decimal total)
    {
        _total = total;
    }

    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        _paymentStrategy = strategy;
    }

    public void Checkout()
    {
        if (_paymentStrategy == null)
            throw new InvalidOperationException("Payment strategy not set");

        _paymentStrategy.Pay(_total);
    }
}

// Usage
var cart = new ShoppingCart(99.99m);
cart.SetPaymentStrategy(new CreditCardPayment("1234-5678-9012-3456"));
cart.Checkout();

cart.SetPaymentStrategy(new PayPalPayment("user@gmail.com"));
cart.Checkout();

cart.SetPaymentStrategy(new CryptoPayment("0x742d35Cc6634C0532925a3b844Bc57e94f8455ab"));
cart.Checkout();
```

### 2. **Strategy with Parameters**

```csharp
// Strategy Interface with return value
public interface ISortingStrategy
{
    int[] Sort(int[] array);
}

public class BubbleSort : ISortingStrategy
{
    public int[] Sort(int[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (array[j] > array[j + 1])
                {
                    int temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
        return array;
    }
}

public class QuickSort : ISortingStrategy
{
    public int[] Sort(int[] array)
    {
        Console.WriteLine("Using Quick Sort");
        // Quick sort implementation
        return array;
    }
}

// Sorter Context
public class Sorter
{
    public int[] Sort(int[] array, ISortingStrategy strategy)
    {
        return strategy.Sort(array);
    }
}

// Usage
var sorter = new Sorter();
int[] data = { 5, 2, 8, 1, 9 };

sorter.Sort(data, new BubbleSort());
sorter.Sort(data, new QuickSort());
```

---

## 🎯 Real-World Example / Ví Dụ Thực Tế

### **E-Commerce Discount Strategy System**

```csharp
public interface IDiscountStrategy
{
    decimal CalculateDiscount(decimal originalPrice, int itemCount);
}

// Discount Strategies
public class NoDiscount : IDiscountStrategy
{
    public decimal CalculateDiscount(decimal originalPrice, int itemCount)
    {
        return 0;
    }
}

public class StudentDiscount : IDiscountStrategy
{
    public decimal CalculateDiscount(decimal originalPrice, int itemCount)
    {
        return originalPrice * 0.10m; // 10% discount
    }
}

public class LoyalCustomerDiscount : IDiscountStrategy
{
    public decimal CalculateDiscount(decimal originalPrice, int itemCount)
    {
        decimal discount = 0;
        if (itemCount > 5)
            discount = originalPrice * 0.15m;
        else if (itemCount > 2)
            discount = originalPrice * 0.10m;
        else
            discount = originalPrice * 0.05m;

        return discount;
    }
}

public class BulkDiscount : IDiscountStrategy
{
    public decimal CalculateDiscount(decimal originalPrice, int itemCount)
    {
        if (itemCount >= 100)
            return originalPrice * 0.25m; // 25%
        if (itemCount >= 50)
            return originalPrice * 0.20m; // 20%
        if (itemCount >= 20)
            return originalPrice * 0.10m; // 10%

        return 0;
    }
}

public class VIPDiscount : IDiscountStrategy
{
    public decimal CalculateDiscount(decimal originalPrice, int itemCount)
    {
        return originalPrice * 0.30m; // 30% VIP discount
    }
}

// Context - Customer
public class Customer
{
    public string Name { get; set; }
    public int PreviousPurchases { get; set; }
    private IDiscountStrategy _discountStrategy;

    public Customer(string name, IDiscountStrategy strategy)
    {
        Name = name;
        _discountStrategy = strategy;
    }

    public void SetDiscountStrategy(IDiscountStrategy strategy)
    {
        _discountStrategy = strategy;
    }

    public void Purchase(decimal amount, int itemCount)
    {
        decimal discount = _discountStrategy.CalculateDiscount(amount, itemCount);
        decimal finalPrice = amount - discount;

        Console.WriteLine($"\n--- Purchase Summary ---");
        Console.WriteLine($"Customer: {Name}");
        Console.WriteLine($"Original Price: ${amount:F2}");
        Console.WriteLine($"Discount: ${discount:F2} ({(discount / amount * 100):F1}%)");
        Console.WriteLine($"Final Price: ${finalPrice:F2}");
    }
}

// Usage
public class Application
{
    public static void Main()
    {
        // Regular customer
        var regular = new Customer("John", new NoDiscount());
        regular.Purchase(100, 1);

        // Student customer
        var student = new Customer("Alice", new StudentDiscount());
        student.Purchase(100, 1);

        // Loyal customer
        var loyal = new Customer("Bob", new LoyalCustomerDiscount());
        loyal.Purchase(100, 10); // 10 items

        // Wholesale purchase
        var wholesaler = new Customer("Charlie", new BulkDiscount());
        wholesaler.Purchase(1000, 60); // 60 items

        // VIP customer
        var vip = new Customer("David", new VIPDiscount());
        vip.Purchase(100, 1);

        // Customer can change strategy
        Console.WriteLine("\n--- Bob becomes a VIP ---");
        loyal.SetDiscountStrategy(new VIPDiscount());
        loyal.Purchase(100, 1);
    }
}
```

### **File Compression/Export Strategy**

```csharp
public interface IExportStrategy
{
    void Export(List<string> data, string filename);
}

public class CsvExport : IExportStrategy
{
    public void Export(List<string> data, string filename)
    {
        Console.WriteLine($"Exporting to CSV: {filename}.csv");
        // Write CSV format
    }
}

public class JsonExport : IExportStrategy
{
    public void Export(List<string> data, string filename)
    {
        Console.WriteLine($"Exporting to JSON: {filename}.json");
        // Write JSON format
    }
}

public class XmlExport : IExportStrategy
{
    public void Export(List<string> data, string filename)
    {
        Console.WriteLine($"Exporting to XML: {filename}.xml");
        // Write XML format
    }
}

public class ExcelExport : IExportStrategy
{
    public void Export(List<string> data, string filename)
    {
        Console.WriteLine($"Exporting to Excel: {filename}.xlsx");
        // Write Excel format
    }
}

// Report Generator
public class ReportGenerator
{
    private List<string> _data;
    private IExportStrategy _exportStrategy;

    public ReportGenerator(List<string> data)
    {
        _data = data;
    }

    public void SetExportFormat(IExportStrategy strategy)
    {
        _exportStrategy = strategy;
    }

    public void Generate(string filename)
    {
        if (_exportStrategy == null)
            throw new InvalidOperationException("Export strategy not set");

        _exportStrategy.Export(_data, filename);
    }
}

// Usage
var report = new ReportGenerator(new List<string> { "A", "B", "C" });

report.SetExportFormat(new CsvExport());
report.Generate("report");

report.SetExportFormat(new JsonExport());
report.Generate("report");

report.SetExportFormat(new ExcelExport());
report.Generate("report");
```

---

## ✅ Advantages / Lợi Ích

| Lợi Ích                   | Mô Tả                                   |
| ------------------------- | --------------------------------------- |
| **Flexibility**           | Dễ dàng thay đổi algorithm lúc runtime  |
| **Loose Coupling**        | Context không phụ thuộc cụ thể strategy |
| **Easy to Test**          | Test từng strategy riêng biệt           |
| **Open/Closed Principle** | Mở với extension, đóng với modification |
| **No Conditional Logic**  | Tránh if-else chains                    |

---

## ❌ Disadvantages / Nhược Điểm

| Nhược Điểm                 | Mô Tả                          |
| -------------------------- | ------------------------------ |
| **More Classes**           | Số classes tăng                |
| **Unnecessary for Simple** | Overkill nếu strategy ít       |
| **Context Awareness**      | Strategy cần biết context data |

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Using big if-else chains
public class OrderProcessor
{
    public decimal ProcessDiscount(string customerType, decimal price, int items)
    {
        if (customerType == "student")
            return price * 0.10m;
        else if (customerType == "loyal")
            return price * 0.15m;
        else if (customerType == "bulk")
            return price * 0.20m;
        else if (customerType == "vip")
            return price * 0.30m;
        // . ..more cases
        return 0;
    }
}

// ✅ CORRECT - Using Strategy pattern
public class OrderProcessor
{
    private IDiscountStrategy _discountStrategy;

    public void SetDiscountStrategy(IDiscountStrategy strategy)
    {
        _discountStrategy = strategy;
    }

    public decimal ProcessDiscount(decimal price, int items)
    {
        return _discountStrategy.CalculateDiscount(price, items);
    }
}
```

---

## 🎓 Strategy vs Other Patterns

| Pattern      | Purpose                          |
| ------------ | -------------------------------- |
| **State**    | Object behavior changes từ state |
| **Strategy** | Tùy chọn algorithm               |
| **Command**  | Encapsulate request              |

---

## 📚 Related Topics

- [Command Pattern](./command.md)
- [State Pattern](./state.md)
- [SOLID Principles](../solid-principles/)

---

## 🎓 Interview Questions

1. **Strategy pattern là gì? Khi nào dùng?**
2. **Strategy vs State - Khác gì?**
3. **Làm sao chọn strategy lúc runtime?**
4. **Cách test Strategy?**
5. **Strategy pattern giải quyết vấn đề gì?**
6. **Có thể combine multiple strategies không?**
