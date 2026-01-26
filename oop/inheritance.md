# Inheritance / Kế Thừa

## 📖 Định Nghĩa / Definition

**Inheritance** là một trong những cột trụ của OOP, cho phép một class (derived class/child class) kế thừa properties và methods từ một class khác (base class/parent class).

**Inheritance** is one of the pillars of OOP, allowing a derived class (child class) to inherit properties and methods from another class (base class/parent class).

---

## 💡 Khái Niệm Cơ Bản / Basic Concepts

### Types of Inheritance / Các Loại Kế Thừa

| Loại                     | Mô Tả                             | C# Support            |
| ------------------------ | --------------------------------- | --------------------- |
| Single Inheritance       | Một child kế thừa từ một parent   | ✅ Yes                |
| Multiple Inheritance     | Một child kế thừa từ nhiều parent | ❌ No (use Interface) |
| Multilevel Inheritance   | A → B → C (Chain)                 | ✅ Yes                |
| Hierarchical Inheritance | Nhiều child kế thừa từ một parent | ✅ Yes                |

---

## 📝 Syntax

```csharp
// Base Class / Lớp Cha
public class Animal
{
    public string Name { get; set; }

    public void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }
}

// Derived Class / Lớp Con
public class Dog : Animal
{
    public void Bark()
    {
        Console.WriteLine($"{Name} is barking");
    }
}

// Usage
Dog dog = new Dog { Name = "Buddy" };
dog.Eat();   // Inherited method từ Animal
dog.Bark();  // Dog's own method
```

---

## 🎯 Key Points / Những Điểm Quan Trọng

### 1. **Base Class Constructor** / Hàm Tạo Lớp Cha

```csharp
public class Animal
{
    public string Name { get; set; }

    public Animal(string name)
    {
        Name = name;
    }
}

public class Dog : Animal
{
    public Dog(string name) : base(name)
    {
        // Gọi constructor của base class
    }
}
```

### 2. **Virtual & Override** / Ghi Đè Methods

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
        Console.WriteLine("Dog barks");
    }
}
```

### 3. **Protected Access Modifier** / Truy Cập Bảo Vệ

```csharp
public class Animal
{
    protected string Name { get; set; }  // Accessible in derived class
    private int Age { get; set; }         // NOT accessible in derived class
}
```

### 4. **Sealed Class** / Lớp Được Bảo Vệ

```csharp
public sealed class FinalClass
{
    // Không thể bị kế thừa
    // Cannot be inherited
}
```

---

## ✅ Best Practices

1. **Favor Composition Over Inheritance** - Ưu tiên composition khi có thể
2. **Keep Hierarchies Shallow** - Không nên tạo chuỗi kế thừa quá sâu (2-3 levels là đủ)
3. **Use Abstract Classes** - Dùng abstract class cho behavior chung
4. **Use Interfaces** - Dùng interface thay vì multiple inheritance
5. **Document Virtual Methods** - Ghi chú rõ ràng những method có thể override

---

## 🔄 Composition Over Inheritance

**Ý chính:** Ghép hành vi (has-a / uses) thay vì kéo dài cây thừa kế (is-a), để giảm coupling, tránh fragile base class, dễ thay thế và test.

### Khi nên chọn composition

- Behavior thay đổi theo ngữ cảnh (strategy, policy) hoặc cần hoán đổi runtime
- Tính năng mang tính “kèm thêm” (logging, caching, retry, validation, metrics)
- Tránh tạo subclass chỉ để tái dùng vài dòng code

### Ví dụ so sánh nhanh

```csharp
// ❌ Kế thừa chỉ để thêm logging
public class LoggingUserService : UserService
{
    public override void Create(User user)
    {
        Console.WriteLine("Creating user");
        base.Create(user);
    }
}

// ✅ Composition: ghép logger
public class UserService
{
    private readonly ILogger _logger;
    private readonly IUserRepository _repo;

    public UserService(ILogger logger, IUserRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public void Create(User user)
    {
        _logger.Log("Creating user");
        _repo.Add(user);
    }
}
```

### Checklist nhanh

- Có phải quan hệ thực sự “is-a”? Nếu không, đừng kế thừa.
- Hành vi có cần hoán đổi/plug-in? → Composition + interface/strategy.
- Base class thay đổi có thể phá vỡ subclass? → Ưu tiên composition.

---

## 🔴 Common Mistakes / Những Lỗi Thường Gặp

```csharp
// ❌ WRONG - Quên gọi base constructor
public class Dog : Animal
{
    public Dog(string name)
    {
        // Lỗi! Không gọi base(name)
    }
}

// ✅ CORRECT
public class Dog : Animal
{
    public Dog(string name) : base(name)
    {
    }
}
```

---

## 🌍 Real-World Scenarios / Tình Huống Thực Tế

### Base Controller Sharing Common Behavior

```csharp
public abstract class BaseController : ControllerBase
{
    protected readonly ILogger _logger;

    protected BaseController(ILogger logger)
    {
        _logger = logger;
    }

    protected IActionResult OkResult(object data)
    {
        return Ok(new { success = true, data });
    }
}

public class UsersController : BaseController
{
    public UsersController(ILogger logger) : base(logger) { }

    [HttpGet("users/{id}")]
    public IActionResult Get(int id)
    {
        _logger.LogInformation("Fetching user {Id}", id);
        return OkResult(new { id, name = "Alice" });
    }
}
```

### Domain Entity Base Class (Audit Fields)

```csharp
public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }

    public void Touch() => UpdatedAt = DateTime.UtcNow;
}

public class Order : Entity
{
    public decimal Total { get; private set; }
    public void AddLine(decimal amount)
    {
        Total += amount;
        Touch();
    }
}
```

### Reusing Validation Logic

```csharp
public abstract class RequestValidator
{
    public void ValidateNotNull(object value, string name)
    {
        if (value is null) throw new ArgumentNullException(name);
    }
}

public class CreateUserValidator : RequestValidator
{
    public void Validate(CreateUserRequest request)
    {
        ValidateNotNull(request, nameof(request));
        ValidateNotNull(request.Email, nameof(request.Email));
    }
}
```

---

## 🎓 Interview Questions & Answers / Câu Hỏi Phỏng Vấn & Trả Lời

### 1. **Giải thích sự khác nhau giữa Inheritance và Composition?**

**Trả lời:**

- **Inheritance (kế thừa):** Quan hệ "is-a" (chó là một động vật). Kế thừa properties/methods từ base class, tạo cây kế thừa.
  - ✅ Dùng khi có quan hệ phân cấp rõ ràng, behavior ổn định.
  - ❌ Tạo coupling mạnh, fragile base class, cây sâu khó bảo trì.
- **Composition (ghép):** Quan hệ "has-a" (chó có một chủ). Ghép các behavior khác nhau như lego bricks.
  - ✅ Linh hoạt, dễ hoán đổi, giảm coupling, dễ test.
  - ❌ Cần nhiều interfaces, hơi verbose.

**Ví dụ:**

```csharp
// Inheritance: Dog IS-A Animal
public class Dog : Animal { }

// Composition: Dog HAS-A Logger
public class Dog
{
    private readonly ILogger _logger;
    public Dog(ILogger logger) => _logger = logger;
}
```

---

### 2. **Khi nào nên dùng `virtual` keyword?**

**Trả lời:**

- Dùng `virtual` khi bạn muốn **cho phép subclass override** method từ base class.
- Nếu không khai báo `virtual`, subclass không thể override (ngoài khi base class là abstract).
- **Nguyên tắc:** Chỉ mark `virtual` khi thực sự cần override; mặc định là sealed để tránh abuse.

**Ví dụ:**

```csharp
public class Vehicle
{
    public virtual void Start() => Console.WriteLine("Vehicle starts");
}

public class Car : Vehicle
{
    public override void Start() => Console.WriteLine("Car engine starts");
}
```

---

### 3. **Có thể kế thừa từ multiple classes không? Nếu không, giải pháp là gì?**

**Trả lời:**

- **C# không hỗ trợ multiple inheritance** (một class chỉ kế thừa từ một class).
- **Giải pháp:** Dùng **Interfaces** (một class có thể implement nhiều interfaces).

**Ví dụ:**

```csharp
// ❌ CÓ LỖI
public class Dog : Animal, Pet { }  // ERROR

// ✅ ĐÚNG - Dùng interface
public interface IPet { }
public class Dog : Animal, IPet { }  // OK
```

**Lý do:** Multiple inheritance phức tạp (diamond problem), interfaces cung cấp flexibility mà không duplicate code.

---

### 4. **`protected` vs `private` keyword là gì?**

**Trả lời:**

| Keyword     | Same Class | Derived Class | Outside |
| ----------- | ---------- | ------------- | ------- |
| `private`   | ✅         | ❌            | ❌      |
| `protected` | ✅         | ✅            | ❌      |
| `public`    | ✅         | ✅            | ✅      |

- **`private`:** Chỉ accessible trong class hiện tại, subclass không thể dùng.
- **`protected`:** Accessible trong class hiện tại + subclass, ngoài code không thể dùng.

**Ví dụ:**

```csharp
public class Base
{
    private int _secret;        // Chỉ Base có thể dùng
    protected int _semi;        // Base và Derived có thể dùng
    public int _public;         // Ai cũng dùng được
}

public class Derived : Base
{
    public void Method()
    {
        // _secret;  // ❌ ERROR
        _semi = 5;  // ✅ OK
        _public = 10; // ✅ OK
    }
}
```

---

### 5. **Sealed class dùng để làm gì?**

**Trả lời:**

- **Sealed class** là class **không thể bị kế thừa**.
- Dùng khi bạn muốn **chắc chắn không ai có thể thay đổi logic** của class này.
- **Lợi thế:** Compiler tối ưu tốt hơn (không cần virtual dispatch), rõ ràng ý định.

**Ví dụ:**

```csharp
public sealed class ConfigManager
{
    // Không thể kế thừa, bảo vệ logic nhạy cảm
}

// ❌ ERROR: Không thể kế thừa sealed class
public class MyConfig : ConfigManager { }
```

**Khi dùng:**

- Classes mang logic critical/security-sensitive (cryptography, payment, authentication).
- Classes design sẵn để final (string, int, decimal trong .NET là sealed).

---

## 📚 Related Topics / Các Chủ Đề Liên Quan

- [Polymorphism](polymorphism.md)
- [Encapsulation](encapsulation.md)
- [Abstraction](abstraction.md)
- [SOLID Principles](../solid-principles/)
