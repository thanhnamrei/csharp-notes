# Decorator Pattern / Mẫu Trang Trí

## 📖 Định Nghĩa / Definition

**Decorator** là design pattern cho phép **thêm responsibilities động** vào object mà không ảnh hưởng đến object khác.

**Decorator** is a design pattern that allows **adding responsibilities dynamically** to objects without affecting other objects.

---

## 💡 Khi Nào Dùng / When to Use

✅ Stream I/O - FileStream, BufferedStream, GZipStream...
✅ UI Components - Borders, Scrollbars, Shadows...
✅ Feature Toggles - Add features conditionally
✅ Middleware - Add logging, authentication, caching...
✅ Coffee Shop - Coffee + Milk + Sugar...
✅ Text Formatting - Text + Bold + Italic + Underline...

---

## 📝 Implementations / Các Cách Implement

### 1. **Classic Decorator Pattern**

```csharp
// Component Interface
public interface IComponent
{
    string GetDescription();
    decimal GetPrice();
}

// Concrete Component
public class SimpleCoffee : IComponent
{
    public string GetDescription() => "Simple Coffee";
    public decimal GetPrice() => 2.0m;
}

// Decorator Base Class
public abstract class CoffeeDecorator : IComponent
{
    protected IComponent _coffee;

    public CoffeeDecorator(IComponent coffee)
    {
        _coffee = coffee;
    }

    public virtual string GetDescription() => _coffee.GetDescription();
    public virtual decimal GetPrice() => _coffee.GetPrice();
}

// Concrete Decorators
public class MilkDecorator : CoffeeDecorator
{
    public MilkDecorator(IComponent coffee) : base(coffee) { }

    public override string GetDescription() => _coffee.GetDescription() + ", Milk";
    public override decimal GetPrice() => _coffee.GetPrice() + 0.5m;
}

public class SugarDecorator : CoffeeDecorator
{
    public SugarDecorator(IComponent coffee) : base(coffee) { }

    public override string GetDescription() => _coffee.GetDescription() + ", Sugar";
    public override decimal GetPrice() => _coffee.GetPrice() + 0.25m;
}

public class WhippedCreamDecorator : CoffeeDecorator
{
    public WhippedCreamDecorator(IComponent coffee) : base(coffee) { }

    public override string GetDescription() => _coffee.GetDescription() + ", Whipped Cream";
    public override decimal GetPrice() => _coffee.GetPrice() + 0.75m;
}

// Usage
IComponent coffee = new SimpleCoffee();
Console.WriteLine($"{coffee.GetDescription()} - ${coffee.GetPrice()}");
// Output: Simple Coffee - $2.00

coffee = new MilkDecorator(coffee);
Console.WriteLine($"{coffee.GetDescription()} - ${coffee.GetPrice()}");
// Output: Simple Coffee, Milk - $2.50

coffee = new SugarDecorator(coffee);
Console.WriteLine($"{coffee.GetDescription()} - ${coffee.GetPrice()}");
// Output: Simple Coffee, Milk, Sugar - $2.75

coffee = new WhippedCreamDecorator(coffee);
Console.WriteLine($"{coffee.GetDescription()} - ${coffee.GetPrice()}");
// Output: Simple Coffee, Milk, Sugar, Whipped Cream - $3.50
```

### 2. **Decorator with C# Streams (Real C# Example)**

```csharp
// Real C# I/O Decorator pattern
// FileStream (Base) -> BufferedStream (Decorator) -> GZipStream (Decorator)

using System.IO.Compression;

public class StreamDecoratorExample
{
    public void CompressFile(string sourceFile, string destFile)
    {
        // Compose decorators
        using (FileStream fileStream = new FileStream(destFile, FileMode.Create))
        using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
        using (BufferedStream bufferedStream = new BufferedStream(gzipStream))
        {
            bufferedStream.WriteByte(65); // Write 'A'
            // Each decorator enhances the functionality
        }
    }
}
```

---

## 🎯 Real-World Example / Ví Dụ Thực Tế

### **Text Processing & Formatting**

```csharp
public interface IText
{
    string Format();
}

// Base Component
public class PlainText : IText
{
    private string _content;

    public PlainText(string content)
    {
        _content = content;
    }

    public string Format() => _content;
}

// Decorators
public class BoldDecorator : IText
{
    private IText _text;

    public BoldDecorator(IText text) => _text = text;

    public string Format() => $"**{_text.Format()}**";
}

public class ItalicDecorator : IText
{
    private IText _text;

    public ItalicDecorator(IText text) => _text = text;

    public string Format() => $"_{_text.Format()}_";
}

public class UnderlineDecorator : IText
{
    private IText _text;

    public UnderlineDecorator(IText text) => _text = text;

    public string Format() => $"[U]{_text.Format()}[/U]";
}

public class HighlightDecorator : IText
{
    private IText _text;

    public HighlightDecorator(IText text) => _text = text;

    public string Format() => $"[H]{_text.Format()}[/H]";
}

// Usage
IText text = new PlainText("Hello World");
Console.WriteLine(text.Format()); // Hello World

text = new BoldDecorator(text);
Console.WriteLine(text.Format()); // **Hello World**

text = new ItalicDecorator(text);
Console.WriteLine(text.Format()); // _**Hello World**_

text = new UnderlineDecorator(text);
Console.WriteLine(text.Format()); // [U]_**Hello World**_[/U]

text = new HighlightDecorator(text);
Console.WriteLine(text.Format()); // [H][U]_**Hello World**_[/U][/H]
```

### **Pizza Builder with Decorators**

```csharp
using System.Collections.Generic;

public interface IPizza
{
    string GetDescription();
    decimal GetCost();
}

public class SimplePizza : IPizza
{
    public string GetDescription() => "Pizza";
    public decimal GetCost() => 10.0m;
}

public abstract class ToppingDecorator : IPizza
{
    protected IPizza _pizza;

    public ToppingDecorator(IPizza pizza) => _pizza = pizza;

    public virtual string GetDescription() => _pizza.GetDescription();
    public virtual decimal GetCost() => _pizza.GetCost();
}

// Toppings
public class CheeseTopping : ToppingDecorator
{
    public CheeseTopping(IPizza pizza) : base(pizza) { }

    public override string GetDescription() => _pizza.GetDescription() + ", Cheese";
    public override decimal GetCost() => _pizza.GetCost() + 1.0m;
}

public class PepperoniTopping : ToppingDecorator
{
    public PepperoniTopping(IPizza pizza) : base(pizza) { }

    public override string GetDescription() => _pizza.GetDescription() + ", Pepperoni";
    public override decimal GetCost() => _pizza.GetCost() + 2.0m;
}

public class MushroomTopping : ToppingDecorator
{
    public MushroomTopping(IPizza pizza) : base(pizza) { }

    public override string GetDescription() => _pizza.GetDescription() + ", Mushroom";
    public override decimal GetCost() => _pizza.GetCost() + 1.5m;
}

public class VeganCheeseTopping : ToppingDecorator
{
    public VeganCheeseTopping(IPizza pizza) : base(pizza) { }

    public override string GetDescription() => _pizza.GetDescription() + ", Vegan Cheese";
    public override decimal GetCost() => _pizza.GetCost() + 1.75m;
}

// Usage
public class PizzaShop
{
    public static void Main()
    {
        // Customer 1: Simple cheese pizza
        IPizza pizza1 = new SimplePizza();
        pizza1 = new CheeseTopping(pizza1);
        Console.WriteLine($"{pizza1.GetDescription()} - ${pizza1.GetCost()}");
        // Output: Pizza, Cheese - $11.00

        // Customer 2: Premium pizza with multiple toppings
        IPizza pizza2 = new SimplePizza();
        pizza2 = new CheeseTopping(pizza2);
        pizza2 = new PepperoniTopping(pizza2);
        pizza2 = new MushroomTopping(pizza2);
        Console.WriteLine($"{pizza2.GetDescription()} - ${pizza2.GetCost()}");
        // Output: Pizza, Cheese, Pepperoni, Mushroom - $14.50

        // Customer 3: Vegan pizza
        IPizza pizza3 = new SimplePizza();
        pizza3 = new VeganCheeseTopping(pizza3);
        pizza3 = new MushroomTopping(pizza3);
        Console.WriteLine($"{pizza3.GetDescription()} - ${pizza3.GetCost()}");
        // Output: Pizza, Vegan Cheese, Mushroom - $13.25
    }
}
```

### **HTTP Request Decorator (Middleware-like)**

```csharp
public interface IHttpClient
{
    void SendRequest(string url);
}

public class BasicHttpClient : IHttpClient
{
    public void SendRequest(string url)
    {
        Console.WriteLine($"Sending request to {url}");
    }
}

public abstract class HttpClientDecorator : IHttpClient
{
    protected IHttpClient _httpClient;

    public HttpClientDecorator(IHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public virtual void SendRequest(string url)
    {
        _httpClient.SendRequest(url);
    }
}

// Logging Decorator
public class LoggingDecorator : HttpClientDecorator
{
    public LoggingDecorator(IHttpClient httpClient) : base(httpClient) { }

    public override void SendRequest(string url)
    {
        Console.WriteLine($"[LOG] Sending request to {url} at {DateTime.Now}");
        base.SendRequest(url);
    }
}

// Authentication Decorator
public class AuthenticationDecorator : HttpClientDecorator
{
    private string _token;

    public AuthenticationDecorator(IHttpClient httpClient, string token) : base(httpClient)
    {
        _token = token;
    }

    public override void SendRequest(string url)
    {
        Console.WriteLine($"[AUTH] Adding authorization header with token: {_token}");
        base.SendRequest(url);
    }
}

// Caching Decorator
public class CachingDecorator : HttpClientDecorator
{
    private Dictionary<string, string> _cache = new();

    public CachingDecorator(IHttpClient httpClient) : base(httpClient) { }

    public override void SendRequest(string url)
    {
        if (_cache.ContainsKey(url))
        {
            Console.WriteLine($"[CACHE] Found cached response for {url}");
        }
        else
        {
            Console.WriteLine($"[CACHE] Cache miss, fetching from server");
            base.SendRequest(url);
        }
    }
}

// Usage
IHttpClient client = new BasicHttpClient();
client = new LoggingDecorator(client);
client = new AuthenticationDecorator(client, "token_12345");
client = new CachingDecorator(client);

client.SendRequest("https://api.example.com/users");
```

---

## ✅ Advantages / Lợi Ích

| Lợi Ích                   | Mô Tả                              |
| ------------------------- | ---------------------------------- |
| **Dynamic Behavior**      | Thêm responsibility lúc runtime    |
| **Single Responsibility** | Mỗi decorator có trách nhiệm riêng |
| **Composable**            | Kết hợp multiple decorators        |
| **Flexible**              | Tránh class explosion              |
| **No Modification**       | Không cần modify original class    |

---

## ❌ Disadvantages / Nhược Điểm

| Nhược Điểm          | Mô Tả                              |
| ------------------- | ---------------------------------- |
| **Complexity**      | Implement nhiều classes            |
| **Order Dependent** | Thứ tự decorator có thể quan trọng |
| **Hard to Debug**   | Khó track nested decorators        |
| **Performance**     | Multiple wrapping = performance    |

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Not following fluent pattern
public class CoffeeDecorator
{
    private IComponent _coffee;

    public CoffeeDecorator(IComponent coffee)
    {
        _coffee = coffee;
    }

    // Không return decorator - không thể chain
    public void Decorate()
    {
        // Implementation
    }
}

// ✅ CORRECT - Fluent & Composable
public abstract class CoffeeDecorator : IComponent
{
    protected IComponent _coffee;

    public CoffeeDecorator(IComponent coffee)
    {
        _coffee = coffee;
    }

    public virtual string GetDescription() => _coffee.GetDescription();
    public virtual decimal GetPrice() => _coffee.GetPrice();
}
```

---

## 🎓 Decorator vs Other Patterns

| Pattern       | Purpose                     |
| ------------- | --------------------------- |
| **Decorator** | Thêm behavior lúc runtime   |
| **Adapter**   | Làm compatible 2 interfaces |
| **Proxy**     | Control access              |
| **Wrapper**   | Làm interface dễ dùng hơn   |

---

## 📚 Related Topics

- [Proxy Pattern](./proxy.md)
- [Composition vs Inheritance](../oop/)
- [Streams in C#](../collections-linq/)

---

## 🎓 Interview Questions

1. **Decorator pattern là gì? Khi nào dùng?**
2. **Decorator vs Inheritance - Cái nào tốt hơn?**
3. **Làm sao compose multiple decorators?**
4. **C# Streams dùng Decorator pattern?**
5. **Decorator vs Proxy - Khác gì?**
6. **Có vấn đề gì nếu kết hợp quá nhiều decorators?**
