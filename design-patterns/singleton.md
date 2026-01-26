# Singleton Pattern / Mẫu Đơn

## 📖 Định Nghĩa / Definition

**Singleton** là design pattern đảm bảo một class chỉ có **một instance duy nhất** và cung cấp **global point of access** để truy cập nó.

**Singleton** is a design pattern that ensures a class has **only one instance** and provides a **global point of access** to it.

---

## 💡 Khi Nào Dùng / When to Use

✅ Logger - Chỉ cần một logger
✅ Database Connection - Connection pool
✅ Configuration - Cấu hình ứng dụng
✅ Cache - Shared cache
✅ Thread Pools - Shared thread pool

---

## 📝 Implementations / Các Cách Implement

### 1. **Eager Initialization** (Thread-Safe, nhưng lãng phí memory)

```csharp
public class DatabaseConnection
{
    // Instance được tạo ngay khi class load
    private static readonly DatabaseConnection _instance =
        new DatabaseConnection();

    private DatabaseConnection()
    {
        // Private constructor
    }

    public static DatabaseConnection Instance => _instance;

    public void Connect()
    {
        Console.WriteLine("Connected to database");
    }
}

// Usage
DatabaseConnection db = DatabaseConnection.Instance;
db.Connect();
```

### 2. **Lazy Initialization** (Thread-Safe, Chỉ tạo khi cần)

```csharp
public class Logger
{
    private static readonly Lazy<Logger> _instance =
        new Lazy<Logger>(() => new Logger());

    private Logger()
    {
        Console.WriteLine("Logger initialized");
    }

    public static Logger Instance => _instance.Value;

    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

// Usage
Logger logger = Logger.Instance;
logger.Log("Application started");
```

### 3. **Double-Checked Locking** (Cơ bản)

```csharp
public class Singleton
{
    private static Singleton _instance;
    private static readonly object _lockObject = new object();

    private Singleton()
    {
    }

    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new Singleton();
                    }
                }
            }
            return _instance;
        }
    }
}
```

### 4. **Thread-Safe Singleton** (C# Recommended)

```csharp
public sealed class Configuration
{
    private static readonly Configuration _instance = new Configuration();

    // Static constructor to initialize
    static Configuration()
    {
    }

    private Configuration()
    {
        LoadConfiguration();
    }

    public static Configuration Instance => _instance;

    private void LoadConfiguration()
    {
        Console.WriteLine("Loading configuration...");
    }

    public string GetSetting(string key)
    {
        return "value";
    }
}
```

---

## 🎯 Real-World Example / Ví Dụ Thực Tế

```csharp
public sealed class AppSettings
{
    private static readonly Lazy<AppSettings> _instance =
        new Lazy<AppSettings>(() => new AppSettings());

    public static AppSettings Instance => _instance.Value;

    private Dictionary<string, string> _settings;

    private AppSettings()
    {
        _settings = new Dictionary<string, string>
        {
            { "DatabaseConnection", "Server=localhost;Database=MyDb" },
            { "ApiKey", "secret-key-123" },
            { "Timeout", "30000" }
        };
    }

    public string GetSetting(string key)
    {
        return _settings.TryGetValue(key, out var value) ? value : null;
    }
}

// Usage
public class Application
{
    public void Run()
    {
        var settings = AppSettings.Instance;
        Console.WriteLine(settings.GetSetting("DatabaseConnection"));

        // Lần thứ 2 truy cập cùng instance
        var settings2 = AppSettings.Instance;
        Console.WriteLine(ReferenceEquals(settings, settings2)); // true
    }
}
```

---

## ✅ Advantages / Lợi Ích

| Lợi Ích             | Mô Tả                         |
| ------------------- | ----------------------------- |
| **Global Access**   | Dễ truy cập từ bất kỳ đâu     |
| **Single Instance** | Tiết kiệm bộ nhớ              |
| **Thread-Safe**     | Bezpečné trong multithreading |
| **Lazy Loading**    | Tạo khi cần                   |

---

## ❌ Disadvantages / Nhược Điểm

| Nhược Điểm             | Mô Tả                         |
| ---------------------- | ----------------------------- |
| **Hard to Test**       | Khó mock trong unit tests     |
| **Hides Dependencies** | Không rõ ràng dependency      |
| **Global State**       | Không tốt cho maintainability |
| **Thread Safety Cost** | Performance overhead          |

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Not thread-safe
public class BadSingleton
{
    private static BadSingleton _instance;

    private BadSingleton() { }

    public static BadSingleton Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BadSingleton(); // Race condition!
            return _instance;
        }
    }
}

// ✅ CORRECT
public class GoodSingleton
{
    private static readonly Lazy<GoodSingleton> _instance =
        new Lazy<GoodSingleton>(() => new GoodSingleton());

    private GoodSingleton() { }

    public static GoodSingleton Instance => _instance.Value;
}
```

---

## 💡 Alternatives / Các Giải Pháp Thay Thế

### Dependency Injection (Better Practice)

```csharp
// Thay vì dùng Singleton...
public class Logger
{
    private static readonly Logger _instance = new Logger();
    public static Logger Instance => _instance;
}

// ... dùng DI
public class UserService
{
    private readonly ILogger _logger;

    public UserService(ILogger logger)
    {
        _logger = logger;  // Inject, dễ test hơn
    }
}

// Đăng ký DI container
services.AddSingleton<ILogger, Logger>();
```

---

## 🎓 Interview Questions

1. **Singleton là gì? Khi nào dùng?**
2. **Lazy<T> singleton có lợi thế gì?**
3. **Thread-safe singleton như thế nào?**
4. **Tại sao Singleton khó test?**
5. **Singleton vs Static Classes?**

---

## 📚 Related Topics

- [Design Patterns](./index.md)
- [Factory Pattern](factory.md)
- [Dependency Injection](../solid-principles/dip.md)
