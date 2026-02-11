# Factory Pattern / Mẫu Nhà Máy

## 📖 Định Nghĩa / Definition

**Factory** là design pattern cung cấp **interface để tạo object** mà không cần chỉ định **concrete class cụ thể**.

**Factory** is a design pattern that provides an **interface to create objects** without specifying their **concrete classes**.

---

## 💡 Khi Nào Dùng / When to Use

✅ Database Connections - Tạo PostgreSQL, MySQL, SQL Server...
✅ Document Processing - Tạo PDF, Excel, Word...
✅ Payment Gateway - Tạo Stripe, PayPal, Square payment...
✅ Logger Providers - Tạo File Logger, Console Logger, Cloud Logger...
✅ UI Components - Tạo themes, skins cho UI...

---

## 📝 Implementations / Các Cách Implement

### 1. **Simple Factory** (Cơ bản)

```csharp
// Define interface
public interface IDatabase
{
    void Connect();
    void Query(string sql);
    void Disconnect();
}

// Concrete implementations
public class PostgreSQL : IDatabase
{
    public void Connect() => Console.WriteLine("Connected to PostgreSQL");
    public void Query(string sql) => Console.WriteLine($"PostgreSQL: {sql}");
    public void Disconnect() => Console.WriteLine("Disconnected from PostgreSQL");
}

public class MySQL : IDatabase
{
    public void Connect() => Console.WriteLine("Connected to MySQL");
    public void Query(string sql) => Console.WriteLine($"MySQL: {sql}");
    public void Disconnect() => Console.WriteLine("Disconnected from MySQL");
}

// Factory
public class DatabaseFactory
{
    public static IDatabase CreateDatabase(string type)
    {
        return type.ToLower() switch
        {
            "postgres" => new PostgreSQL(),
            "mysql" => new MySQL(),
            _ => throw new ArgumentException($"Unknown database type: {type}")
        };
    }
}

// Usage
IDatabase db = DatabaseFactory.CreateDatabase("postgres");
db.Connect();
db.Query("SELECT * FROM users");
```

### 2. **Abstract Factory** (Nâng cao - Tạo family của objects)

```csharp
// Family 1: SQL Databases
public interface IDatabase
{
    void Connect();
}

public class PostgreSQL : IDatabase
{
    public void Connect() => Console.WriteLine("PostgreSQL connected");
}

public class MySQL : IDatabase
{
    public void Connect() => Console.WriteLine("MySQL connected");
}

// Family 2: Migration Tools
public interface IMigrationTool
{
    void Migrate();
}

public class PostgreSQLMigration : IMigrationTool
{
    public void Migrate() => Console.WriteLine("PostgreSQL migration");
}

public class MySQLMigration : IMigrationTool
{
    public void Migrate() => Console.WriteLine("MySQL migration");
}

// Abstract Factory
public interface IDbFactory
{
    IDatabase CreateDatabase();
    IMigrationTool CreateMigration();
}

public class PostgreSQLFactory : IDbFactory
{
    public IDatabase CreateDatabase() => new PostgreSQL();
    public IMigrationTool CreateMigration() => new PostgreSQLMigration();
}

public class MySQLFactory : IDbFactory
{
    public IDatabase CreateDatabase() => new MySQL();
    public IMigrationTool CreateMigration() => new MySQLMigration();
}

// Usage
IDbFactory factory = new PostgreSQLFactory();
var db = factory.CreateDatabase();
var migration = factory.CreateMigration();

db.Connect();
migration.Migrate();
```

### 3. **Factory Method** (Dùng trong base class)

```csharp
public abstract class Document
{
    public abstract void Save();

    public void Process()
    {
        Save();
        Console.WriteLine("Document processed");
    }
}

public class PdfDocument : Document
{
    public override void Save() => Console.WriteLine("PDF saved");
}

public class ExcelDocument : Document
{
    public override void Save() => Console.WriteLine("Excel saved");
}

public abstract class DocumentFactory
{
    public abstract Document CreateDocument();

    public void ProcessDocument()
    {
        var doc = CreateDocument();
        doc.Process();
    }
}

public class PdfFactory : DocumentFactory
{
    public override Document CreateDocument() => new PdfDocument();
}

public class ExcelFactory : DocumentFactory
{
    public override Document CreateDocument() => new ExcelDocument();
}

// Usage
DocumentFactory factory = new PdfFactory();
factory.ProcessDocument(); // Output: PDF saved, Document processed
```

---

## 🎯 Real-World Example / Ví Dụ Thực Tế

### **Payment Gateway Factory**

```csharp
public interface IPaymentGateway
{
    bool ProcessPayment(decimal amount, string cardToken);
    bool RefundPayment(string transactionId, decimal amount);
}

public class StripeGateway : IPaymentGateway
{
    public bool ProcessPayment(decimal amount, string cardToken)
    {
        Console.WriteLine($"Stripe: Processing ${amount}");
        // Call Stripe API
        return true;
    }

    public bool RefundPayment(string transactionId, decimal amount)
    {
        Console.WriteLine($"Stripe: Refunding ${amount}");
        return true;
    }
}

public class PayPalGateway : IPaymentGateway
{
    public bool ProcessPayment(decimal amount, string cardToken)
    {
        Console.WriteLine($"PayPal: Processing ${amount}");
        // Call PayPal API
        return true;
    }

    public bool RefundPayment(string transactionId, decimal amount)
    {
        Console.WriteLine($"PayPal: Refunding ${amount}");
        return true;
    }
}

public class SquareGateway : IPaymentGateway
{
    public bool ProcessPayment(decimal amount, string cardToken)
    {
        Console.WriteLine($"Square: Processing ${amount}");
        // Call Square API
        return true;
    }

    public bool RefundPayment(string transactionId, decimal amount)
    {
        Console.WriteLine($"Square: Refunding ${amount}");
        return true;
    }
}

public class PaymentGatewayFactory
{
    private static readonly Dictionary<string, Func<IPaymentGateway>> Gateways =
        new()
        {
            { "stripe", () => new StripeGateway() },
            { "paypal", () => new PayPalGateway() },
            { "square", () => new SquareGateway() }
        };

    public static IPaymentGateway CreateGateway(string providerName)
    {
        if (Gateways.TryGetValue(providerName.ToLower(), out var factory))
            return factory();

        throw new ArgumentException($"Unknown payment provider: {providerName}");
    }
}

// Usage in real application
public class OrderService
{
    public void CompleteOrder(string paymentProvider, decimal amount, string cardToken)
    {
        var gateway = PaymentGatewayFactory.CreateGateway(paymentProvider);

        if (gateway.ProcessPayment(amount, cardToken))
        {
            Console.WriteLine("Order completed!");
        }
        else
        {
            Console.WriteLine("Payment failed!");
        }
    }
}

// Usage
var orderService = new OrderService();
orderService.CompleteOrder("stripe", 99.99m, "token_123");
orderService.CompleteOrder("paypal", 49.99m, "token_456");
```

---

## ✅ Advantages / Lợi Ích

| Lợi Ích                  | Mô Tả                                  |
| ------------------------ | -------------------------------------- |
| **Loose Coupling**       | Object tạo độc lập với client          |
| **Easy to Extend**       | Thêm type mới mà không sửa code cũ     |
| **Centralized Creation** | Quản lý tạo object tại một nơi         |
| **Consistent Interface** | Tất cả objects có interface giống nhau |
| **Testability**          | Dễ mock khi unit test                  |

---

## ❌ Disadvantages / Nhược Điểm

| Nhược Điểm           | Mô Tả                           |
| -------------------- | ------------------------------- |
| **Over-Engineering** | Có thể phức tạp không cần thiết |
| **Code Complexity**  | Thêm nhiều classes              |
| **Performance**      | Reflection có thể chậm          |

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Tight coupling
public class OrderService
{
    public void ProcessOrder(string type)
    {
        if (type == "stripe")
        {
            var gateway = new StripeGateway();
            gateway.ProcessPayment(100, "token");
        }
        else if (type == "paypal")
        {
            var gateway = new PayPalGateway();
            gateway.ProcessPayment(100, "token");
        }
    }
}

// ✅ CORRECT - Loose coupling
public class OrderService
{
    public void ProcessOrder(string provider)
    {
        var gateway = PaymentGatewayFactory.CreateGateway(provider);
        gateway.ProcessPayment(100, "token");
    }
}
```

---

## 📚 Related Topics

- [Abstract Factory Pattern](./abstract-factory.md)
- [Dependency Injection](../solid-principles/dip.md)
- [SOLID Principles](../solid-principles/)

---

## 🎓 Interview Questions

### 1. **Factory pattern là gì? Lợi ích của nó?**

**Câu trả lời:**

Factory Pattern là một creational design pattern cung cấp interface để tạo objects mà không cần chỉ định concrete class cụ thể. Client code không cần biết class nào được instantiate, chỉ cần gọi factory method.

**Lợi ích:**

- **Loose Coupling**: Client code không phụ thuộc vào concrete classes, chỉ phụ thuộc vào interface
- **Single Responsibility**: Logic tạo object được tách riêng vào factory class
- **Open/Closed Principle**: Dễ dàng thêm types mới mà không sửa code cũ
- **Code Reusability**: Logic tạo object được centralized, tránh duplicate code
- **Easier Testing**: Dễ mock và test vì sử dụng interface

**Ví dụ:**

```csharp
// Thay vì:
var db = new PostgreSQL(); // Tight coupling

// Dùng Factory:
var db = DatabaseFactory.CreateDatabase("postgres"); // Loose coupling
```

---

### 2. **Sự khác biệt giữa Factory và Abstract Factory?**

**Câu trả lời:**

| Khía cạnh          | Factory Pattern                     | Abstract Factory                           |
| ------------------ | ----------------------------------- | ------------------------------------------ |
| **Purpose**        | Tạo **một loại** object             | Tạo **families** của related objects       |
| **Complexity**     | Đơn giản hơn                        | Phức tạp hơn                               |
| **Return**         | Trả về 1 object                     | Trả về nhiều related objects               |
| **Example**        | Tạo Database (MySQL/PostgreSQL)     | Tạo Database + Migration + Connection Pool |
| **Implementation** | Một factory method                  | Nhiều factory methods trong interface      |
| **When to Use**    | Cần tạo objects từ cùng 1 hierarchy | Cần tạo families của objects cùng làm việc |

**Code Example:**

```csharp
// Simple Factory - Chỉ tạo 1 loại object
public class DatabaseFactory
{
    public static IDatabase Create(string type)
        => type == "mysql" ? new MySQL() : new PostgreSQL();
}

// Abstract Factory - Tạo family của objects
public interface IDbFactory
{
    IDatabase CreateDatabase();
    IMigrationTool CreateMigration();
    IConnectionPool CreateConnectionPool();
}

public class MySQLFactory : IDbFactory
{
    public IDatabase CreateDatabase() => new MySQL();
    public IMigrationTool CreateMigration() => new MySQLMigration();
    public IConnectionPool CreateConnectionPool() => new MySQLPool();
}
```

**Khi nào dùng gì?**

- **Factory**: Khi chỉ cần tạo 1 loại object (VD: chỉ cần Database)
- **Abstract Factory**: Khi cần tạo nhiều objects phải consistent với nhau (VD: MySQL database phải dùng với MySQL migration tool)

---

### 3. **Khi nào dùng Factory?**

**Câu trả lời:**

✅ **Nên dùng Factory khi:**

1. **Không biết trước concrete type**
   - Type được quyết định lúc runtime (từ config, user input, environment)

   ```csharp
   var db = DatabaseFactory.Create(Configuration["DatabaseType"]);
   ```

2. **Có nhiều implementations của cùng interface**
   - Payment Gateways (Stripe, PayPal, Square)
   - Loggers (FileLogger, ConsoleLogger, CloudLogger)
   - Database Connections (MySQL, PostgreSQL, SQL Server)

3. **Logic khởi tạo phức tạp**
   - Cần nhiều bước để setup object
   - Cần đọc config, validate, setup dependencies

   ```csharp
   public static IDatabase Create(string type)
   {
       var db = type switch {
           "mysql" => new MySQL(),
           "postgres" => new PostgreSQL()
       };
       db.LoadConfiguration();
       db.ValidateConnection();
       return db;
   }
   ```

4. **Muốn centralize object creation**
   - Quản lý object creation ở một chỗ
   - Dễ maintain và extend

5. **Testing và Mocking**
   - Dễ inject mock objects trong unit tests

❌ **KHÔNG nên dùng Factory khi:**

- Object creation đơn giản (chỉ `new MyClass()`)
- Chỉ có 1 implementation duy nhất
- Không cần switch giữa các implementations
- Over-engineering cho bài toán đơn giản

**Real-world scenarios:**

- Multi-tenant apps (mỗi tenant khác database)
- Plugin systems (load plugins dynamically)
- Strategy pattern implementation
- Dependency Injection containers

---

### 4. **Làm sao tránh switch-case dài trong Factory?**

**Câu trả lời:**

Có 3 cách chính để tránh `switch-case` dài:

#### **Cách 1: Dictionary-based Factory** ⭐ (Recommended)

```csharp
public class PaymentGatewayFactory
{
    private static readonly Dictionary<string, Func<IPaymentGateway>> _gateways =
        new()
        {
            { "stripe", () => new StripeGateway() },
            { "paypal", () => new PayPalGateway() },
            { "square", () => new SquareGateway() }
        };

    public static IPaymentGateway Create(string provider)
    {
        if (_gateways.TryGetValue(provider.ToLower(), out var factory))
            return factory();

        throw new ArgumentException($"Unknown provider: {provider}");
    }

    // Dễ dàng register thêm providers
    public static void RegisterProvider(string name, Func<IPaymentGateway> factory)
    {
        _gateways[name.ToLower()] = factory;
    }
}
```

**Lợi ích:**

- Không cần sửa code khi thêm provider mới
- Có thể register providers lúc runtime
- Clean và dễ đọc

#### **Cách 2: Reflection-based Factory** (Advanced)

```csharp
public class DatabaseFactory
{
    public static IDatabase Create(string typeName)
    {
        // Tìm tất cả classes implement IDatabase
        var type = Assembly.GetExecutingAssembly()
            .GetTypes()
            .FirstOrDefault(t =>
                typeof(IDatabase).IsAssignableFrom(t) &&
                t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));

        if (type == null)
            throw new ArgumentException($"Unknown database: {typeName}");

        return (IDatabase)Activator.CreateInstance(type);
    }
}

// Usage
var db = DatabaseFactory.Create("MySQL"); // Tự động tìm class MySQL
```

**Lợi ích:**

- Không cần modify factory khi thêm implementations mới
- Tự động discover classes

**Nhược điểm:**

- Performance overhead (reflection chậm)
- Compile-time safety kém hơn

#### **Cách 3: Attribute-based Registration**

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class DatabaseTypeAttribute : Attribute
{
    public string TypeName { get; }
    public DatabaseTypeAttribute(string typeName) => TypeName = typeName;
}

[DatabaseType("mysql")]
public class MySQL : IDatabase { }

[DatabaseType("postgres")]
public class PostgreSQL : IDatabase { }

public class DatabaseFactory
{
    private static readonly Dictionary<string, Type> _types = new();

    static DatabaseFactory()
    {
        // Register tất cả types có attribute
        var types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IDatabase).IsAssignableFrom(t) &&
                        t.GetCustomAttribute<DatabaseTypeAttribute>() != null);

        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<DatabaseTypeAttribute>();
            _types[attr.TypeName.ToLower()] = type;
        }
    }

    public static IDatabase Create(string typeName)
    {
        if (_types.TryGetValue(typeName.ToLower(), out var type))
            return (IDatabase)Activator.CreateInstance(type);

        throw new ArgumentException($"Unknown database: {typeName}");
    }
}
```

**Lợi ích:**

- Tự động registration
- Metadata rõ ràng
- Không cần sửa factory

#### **Cách 4: Strategy + Dependency Injection** (Modern Approach)

```csharp
public class PaymentService
{
    private readonly IEnumerable<IPaymentGateway> _gateways;

    public PaymentService(IEnumerable<IPaymentGateway> gateways)
    {
        _gateways = gateways;
    }

    public IPaymentGateway GetGateway(string provider)
    {
        return _gateways.FirstOrDefault(g =>
            g.GetType().Name.Contains(provider, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"Unknown provider: {provider}");
    }
}

// Startup.cs (ASP.NET Core)
services.AddTransient<IPaymentGateway, StripeGateway>();
services.AddTransient<IPaymentGateway, PayPalGateway>();
services.AddTransient<IPaymentGateway, SquareGateway>();
```

**Tóm tắt - Cách nào tốt nhất?**

| Cách         | Khi nào dùng                    | Performance | Maintainability |
| ------------ | ------------------------------- | ----------- | --------------- |
| Dictionary   | ✅ Most cases, production apps  | Fast        | Excellent       |
| Reflection   | Plugin systems, dynamic loading | Slow        | Good            |
| Attribute    | Large systems, auto-discovery   | Medium      | Excellent       |
| DI Container | Modern apps with DI             | Fast        | Excellent       |

**Recommendation:** Dùng **Dictionary-based** cho hầu hết trường hợp. Nó balance tốt giữa simplicity, performance và maintainability.

---

### 5. **Factory vs Dependency Injection - Khác gì?**

**Câu trả lời:**

Đây là câu hỏi hay vì nhiều người confuse 2 concepts này!

#### **Factory Pattern**

**Mục đích:** Tạo objects, quyết định **loại object nào** được tạo lúc runtime

```csharp
// Client tự tạo object thông qua Factory
public class OrderService
{
    public void ProcessOrder(string provider)
    {
        // Runtime decision - tạo object dựa vào provider
        var gateway = PaymentGatewayFactory.Create(provider);
        gateway.ProcessPayment(100, "token");
    }
}
```

**Đặc điểm:**

- Client **chủ động** gọi factory để tạo object
- Quyết định **loại** object lúc runtime (based on input)
- Object được tạo **mỗi lần call**
- Client vẫn biết về Factory

#### **Dependency Injection**

**Mục đích:** Inject dependencies từ bên ngoài, **giảm coupling** giữa classes

```csharp
// Dependencies được inject từ bên ngoài
public class OrderService
{
    private readonly IPaymentGateway _gateway;

    // Constructor injection - không biết concrete type
    public OrderService(IPaymentGateway gateway)
    {
        _gateway = gateway;
    }

    public void ProcessOrder()
    {
        // Dùng injected dependency
        _gateway.ProcessPayment(100, "token");
    }
}

// DI Container quyết định inject gì
services.AddScoped<IPaymentGateway, StripeGateway>();
```

**Đặc điểm:**

- Dependencies được **inject** từ bên ngoài (constructor, property, method)
- Object được **externally configured**
- Client **không biết** concrete type
- DI Container quản lý lifecycle

#### **So sánh trực tiếp**

| Aspect          | Factory Pattern                     | Dependency Injection             |
| --------------- | ----------------------------------- | -------------------------------- |
| **Control**     | Client tự tạo objects               | Container inject objects         |
| **When decide** | Runtime (based on parameters)       | Configuration time               |
| **Coupling**    | Client biết Factory                 | Client không biết concrete class |
| **Flexibility** | Tạo different types lúc runtime     | Inject same type mỗi lần         |
| **Lifecycle**   | Client control                      | Container control                |
| **Testing**     | Mock factory                        | Inject mock dependencies         |
| **Usage**       | Nhiều implementations, switch types | Consistent dependencies          |

#### **Khi nào dùng gì?**

**✅ Dùng Factory khi:**

- Cần **switch** giữa implementations lúc runtime
- Logic quyết định type phức tạp
- Tạo objects với different configurations

```csharp
// Ví dụ: User chọn payment provider
public void Checkout(string userSelectedProvider)
{
    var gateway = PaymentFactory.Create(userSelectedProvider); // Runtime decision
    gateway.ProcessPayment(amount, token);
}
```

**✅ Dùng Dependency Injection khi:**

- Dependencies **cố định** cho class
- Muốn loose coupling và testability
- Dùng same implementation cho class

```csharp
// Ví dụ: OrderService luôn cần IEmailService
public class OrderService
{
    private readonly IEmailService _emailService; // Fixed dependency

    public OrderService(IEmailService emailService)
    {
        _emailService = emailService;
    }
}
```

#### **Kết hợp cả hai** (Best Practice)

Trong thực tế, ta thường **combine** cả 2!

```csharp
// Factory được inject như một dependency
public class PaymentService
{
    private readonly IPaymentGatewayFactory _factory;

    // DI: Inject factory
    public PaymentService(IPaymentGatewayFactory factory)
    {
        _factory = factory;
    }

    public void ProcessPayment(string provider, decimal amount)
    {
        // Factory: Runtime decision
        var gateway = _factory.Create(provider);
        gateway.ProcessPayment(amount, "token");
    }
}

// Register trong DI Container
services.AddSingleton<IPaymentGatewayFactory, PaymentGatewayFactory>();
```

**Hoặc DI Container itself là một Factory!**

```csharp
public class PaymentService
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ProcessPayment(string provider)
    {
        // Use DI Container as Factory
        var gateway = provider.ToLower() switch
        {
            "stripe" => _serviceProvider.GetRequiredService<StripeGateway>(),
            "paypal" => _serviceProvider.GetRequiredService<PayPalGateway>(),
            _ => throw new ArgumentException()
        };

        gateway.ProcessPayment(100, "token");
    }
}
```

#### **Tóm tắt ngắn gọn**

**Factory:** "Tôi cần tạo object, nhưng loại nào thì tùy runtime"
**Dependency Injection:** "Tôi cần dependency này, ai đó hãy provide cho tôi"

**Factory = Object Creation Pattern**
**DI = Dependency Management Pattern**

Chúng **complement** nhau, không phải thay thế!
