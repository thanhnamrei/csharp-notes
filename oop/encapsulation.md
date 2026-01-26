# Encapsulation / Đóng Gói

## 📖 Định Nghĩa / Definition

**Encapsulation** là nguyên tắc che giấu các chi tiết bên trong của một object và chỉ cộng khai những gì cần thiết. Nó bảo vệ dữ liệu khỏi truy cập không được phép.

**Encapsulation** is the principle of hiding internal details of an object and only exposing what's necessary. It protects data from unauthorized access.

---

## 💡 Access Modifiers / Các Mức Truy Cập

| Modifier    | Same Class | Same Namespace | Derived Class | Outside |
| ----------- | ---------- | -------------- | ------------- | ------- |
| `public`    | ✅         | ✅             | ✅            | ✅      |
| `protected` | ✅         | ❌             | ✅            | ❌      |
| `internal`  | ✅         | ✅             | ❌            | ❌      |
| `private`   | ✅         | ❌             | ❌            | ❌      |

---

## 📝 Ví Dụ / Examples

### Without Encapsulation / Không Có Đóng Gói

```csharp
// ❌ BAD - Dữ liệu công khai
public class BankAccount
{
    public decimal Balance;  // Bất cứ ai cũng có thể thay đổi
}

// Người dùng có thể làm điều xấu
BankAccount account = new BankAccount();
account.Balance = -1000;  // Số dư âm! Không hợp lệ
```

### With Encapsulation / Có Đóng Gói

```csharp
// ✅ GOOD - Dữ liệu được bảo vệ
public class BankAccount
{
    private decimal _balance;  // Private - không thể truy cập trực tiếp

    public decimal Balance
    {
        get { return _balance; }
        set
        {
            if (value >= 0)
                _balance = value;
            else
                throw new ArgumentException("Balance cannot be negative");
        }
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= Balance)
            Balance -= amount;
        else
            throw new InvalidOperationException("Insufficient funds");
    }
}
```

---

## 🎯 Properties / Thuộc Tính

### Auto-Implemented Properties / Thuộc Tính Tự Động

```csharp
public class Person
{
    // Cách cũ (verbose)
    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    // Cách mới (clean)
    public string Email { get; set; }

    // Read-only property
    public string Id { get; }

    // Init-only property (C# 9+)
    public string Ssn { get; init; }
}
```

### Property with Validation

```csharp
public class Student
{
    private int _age;

    public int Age
    {
        get { return _age; }
        set
        {
            if (value >= 0 && value <= 120)
                _age = value;
            else
                throw new ArgumentException("Invalid age");
        }
    }
}
```

---

## 📋 Levels of Encapsulation / Các Cấp Độ Đóng Gói

### Level 1: Weak Encapsulation

```csharp
public class Car
{
    public string Color { get; set; }  // Setter công khai
}
```

### Level 2: Medium Encapsulation

```csharp
public class Car
{
    public string Color { get; set; }
    private int _speed;

    public int Speed
    {
        get { return _speed; }
        private set { _speed = value; }  // Chỉ class có thể set
    }
}
```

### Level 3: Strong Encapsulation

```csharp
public class Car
{
    private string _color;
    private int _speed;

    public string Color
    {
        get { return _color; }
        set { _color = value; }
    }

    public int Speed
    {
        get { return _speed; }
    }

    public void Accelerate()
    {
        if (_speed < 200)
            _speed += 10;
    }
}
```

---

## ✅ Best Practices

1. **Keep fields private** - Luôn giữ fields là private
2. **Expose through properties** - Dùng properties để expose dữ liệu
3. **Validate in setters** - Validate dữ liệu khi set
4. **Use meaningful names** - Đặt tên rõ ràng
5. **Minimize exposed surface** - Chỉ expose những gì cần thiết

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Public fields
public class Account
{
    public decimal Balance;  // Ai cũng có thể thay đổi
}

// ✅ CORRECT - Private field with property
public class Account
{
    private decimal _balance;

    public decimal Balance
    {
        get { return _balance; }
        set
        {
            if (value >= 0)
                _balance = value;
        }
    }
}
```

---

## 🌍 Real-World Scenarios / Tình Huống Thực Tế

### 1) JWT Token Service (Ẩn Secret, Chỉ Expose API)

```csharp
public class JwtTokenService
{
    private readonly string _secretKey;
    private readonly TimeSpan _lifetime = TimeSpan.FromMinutes(30);

    public JwtTokenService(string secretKey)
    {
        _secretKey = secretKey;
    }

    public string IssueToken(string userId)
    {
        // Secret key không lộ ra ngoài, chỉ expose method phát token
        return $"token-for-{userId}"; // Placeholder demo
    }
}
```

### 2) Payment Aggregate (Kiểm Soát Invariants)

```csharp
public class Payment
{
    public Guid Id { get; } = Guid.NewGuid();
    public decimal Amount { get; private set; }
    public bool IsCaptured { get; private set; }

    public void Authorize(decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive");
        Amount = amount;
    }

    public void Capture()
    {
        if (Amount <= 0) throw new InvalidOperationException("Nothing to capture");
        IsCaptured = true;
    }
}
```

### 3) Profile Update (Chỉ Cho Update Qua Methods An Toàn)

```csharp
public class UserProfile
{
    public string Email { get; private set; }
    public string DisplayName { get; private set; }

    public void UpdateEmail(string email)
    {
        if (!email.Contains("@")) throw new ArgumentException("Invalid email");
        Email = email.Trim();
    }

    public void UpdateDisplayName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
        DisplayName = name.Trim();
    }
}
```

---

## 🎓 Interview Questions & Answers / Câu Hỏi Phỏng Vấn & Trả Lời

### 1. **Tại sao encapsulation lại quan trọng?**

**Trả lời:**

Encapsulation bảo vệ **data integrity** (toàn vẹn dữ liệu) bằng cách:

1. **Kiểm soát truy cập:**
   - Public fields cho phép ai cũng sửa → dữ liệu bất kỳ lúc nào.
   - Properties với validation → chỉ dữ liệu hợp lệ mới lưu.

   ```csharp
   // ❌ BAD
   account.Balance = -1000;  // Sai dữ liệu, không ai phát hiện

   // ✅ GOOD
   account.Balance = -1000;  // Exception, bị chặn
   ```

2. **Giảm coupling:**
   - Code phụ thuộc interface (property), không implement chi tiết.
   - Thay đổi implement bên trong mà API không đổi.

   ```csharp
   // Bên ngoài chỉ biết .Balance, không cần biết _balance được lưu ở đâu
   public decimal Balance { get; private set; }
   ```

3. **Bảo mật (Security):**
   - Che giấu logic nhạy cảm (passwords, API keys, secrets).
   ```csharp
   // Không thể access _secretKey từ ngoài
   private readonly string _secretKey;
   ```

---

### 2. **Access modifiers là gì? Sự khác nhau giữa public và private?**

**Trả lời:**

**Access Modifiers** là từ khóa control scope (phạm vi truy cập) của members.

| Modifier    | Same Class | Derived Class | Outside | Namespace | Use Case                |
| ----------- | ---------- | ------------- | ------- | --------- | ----------------------- |
| `public`    | ✅         | ✅            | ✅      | ✅        | Public API, interface   |
| `protected` | ✅         | ✅            | ❌      | ❌        | Subclass access         |
| `internal`  | ✅         | ❌            | ❌      | ✅        | Assembly-internal       |
| `private`   | ✅         | ❌            | ❌      | ❌        | Internal implementation |

**public vs private:**

```csharp
public class User
{
    public string Username { get; set; }     // Ai cũng thấy, thay đổi được
    private string _passwordHash;            // Chỉ User class access

    public void SetPassword(string pwd)
    {
        // Validate trước khi lưu
        if (pwd.Length < 8) throw new Exception("Too weak");
        _passwordHash = HashPassword(pwd);
    }
}
```

**Tại sao `private` tốt hơn `public`:**

- ✅ Bảo vệ invariants (điều kiện phải luôn đúng).
- ✅ Giảm coupling, dễ refactor.
- ✅ Kiểm soát hoàn toàn cách dữ liệu thay đổi.

---

### 3. **Properties là gì? Có lợi thế gì so với public fields?**

**Trả lời:**

**Properties** là accessors (getter/setter) cho private fields, cung cấp **controlled access**.

**So sánh:**

```csharp
// ❌ OLD - Public Field (nguy hiểm)
public class Student
{
    public int Age;  // Ai cũng thay đổi được, không check
}
student.Age = -5;  // Không hợp lệ nhưng ai cũng đặt được

// ✅ NEW - Property (an toàn)
public class Student
{
    private int _age;
    public int Age
    {
        get { return _age; }
        set
        {
            if (value >= 0 && value <= 120)
                _age = value;
            else
                throw new ArgumentException("Invalid age");
        }
    }
}
student.Age = -5;  // Exception, dữ liệu bảo vệ
```

**Lợi thế của Properties:**

| Tính Năng      | Public Field | Property                 |
| -------------- | ------------ | ------------------------ |
| Validation     | ❌           | ✅                       |
| Read-only      | ❌           | ✅ (get-only)            |
| Lazy loading   | ❌           | ✅ (logic in get)        |
| Notification   | ❌           | ✅ (trigger on change)   |
| API compatible | ❌           | ✅ (can add logic later) |

---

### 4. **Làm sao để tạo read-only property?**

**Trả lời:**

**Read-only** = có getter, không có setter hoặc setter private.

**Cách 1: Init-only (C# 9+) - Khuyến khích**

```csharp
public class User
{
    public string Id { get; init; }  // Set lúc init, sau không thay đổi được
    public string Email { get; init; }
}

var user = new User { Id = "123", Email = "test@example.com" };
user.Id = "456";  // ❌ ERROR - init-only không thể set sau
```

**Cách 2: Get-only**

```csharp
public class Order
{
    public Guid OrderId { get; }  // Không có setter

    public Order()
    {
        OrderId = Guid.NewGuid();  // Set trong constructor
    }
}

var order = new Order();
order.OrderId = Guid.Empty;  // ❌ ERROR
```

**Cách 3: Private Setter**

```csharp
public class Payment
{
    public decimal Amount { get; private set; }

    public void Authorize(decimal amount)
    {
        if (amount > 0)
            Amount = amount;  // Chỉ class có thể set
    }
}
```

**Khi nào dùng:**

- `get; init;` → Immutable sau khởi tạo.
- `get;` → Hoàn toàn read-only.
- `get; private set;` → Controlled internal updates.

---

### 5. **Khi nào nên dùng private setter?**

**Trả lời:**

Dùng **private setter** khi:

1. **Cần validation trước lưu:**

   ```csharp
   public class Account
   {
       public decimal Balance { get; private set; }

       public void Deposit(decimal amount)
       {
           if (amount > 0)
               Balance += amount;  // Validate trước
       }
   }
   ```

2. **Cần maintain invariants (điều kiện luôn đúng):**

   ```csharp
   public class DateRange
   {
       public DateTime Start { get; private set; }
       public DateTime End { get; private set; }

       public void SetRange(DateTime start, DateTime end)
       {
           if (start <= end)
           {
               Start = start;
               End = end;  // Ensure Start <= End luôn
           }
       }
   }
   ```

3. **Cần log hoặc notify khi thay đổi:**

   ```csharp
   public class User
   {
       private string _status;
       public string Status
       {
           get { return _status; }
           private set
           {
               _status = value;
               Logger.Log($"Status changed to {value}");
           }
       }
   }
   ```

4. **Không muốn public API nhưng subclass có thể set:**
   ```csharp
   public class Base
   {
       public string Name { get; protected set; }  // Subclass thay đổi được
   }
   ```

**So sánh nhanh:**

- `public set;` → Tự do, nguy hiểm.
- `private set;` → Chỉ class, controlled.
- `protected set;` → Subclass, controlled.
- `get;` → Read-only.

---

## 📚 Related Topics

- [Inheritance](inheritance.md)
- [Abstraction](abstraction.md)
- [SOLID Principles - Encapsulation Principle](../solid-principles/)
