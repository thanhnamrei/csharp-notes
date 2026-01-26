# Single Responsibility Principle (SRP) / Nguyên Tắc Trách Nhiệm Đơn

## 📖 Định Nghĩa / Definition

**SRP** - Một class nên có chỉ **một lý do để thay đổi**, tức là chỉ nên có **một trách nhiệm**.

**SRP** - A class should have only **one reason to change**, meaning **one responsibility**.

---

## 💡 Khái Niệm / Concept

Mỗi class nên tập trung vào **một khía cạnh duy nhất** của chương trình:

Each class should focus on **one single aspect** of the program:

```
❌ BAD: UserService quản lý người dùng + gửi email + log
✅ GOOD: UserService quản lý người dùng
         EmailService gửi email
         Logger ghi log
```

---

## 📝 Ví Dụ / Examples

### ❌ Violation (Vi Phạm SRP)

```csharp
// BAD - UserService làm quá nhiều việc
public class UserService
{
    // Trách nhiệm 1: Quản lý user
    public void CreateUser(string name, string email) { }

    // Trách nhiệm 2: Gửi email
    public void SendWelcomeEmail(string email) { }

    // Trách nhiệm 3: Ghi log
    public void LogUserCreated(string userId) { }

    // Trách nhiệm 4: Tính toán
    public decimal CalculateUserScore(User user) { }
}

// Vấn đề:
// - Nếu logic gửi email thay đổi, phải sửa UserService
// - Nếu logic log thay đổi, phải sửa UserService
// - Lớp quá phức tạp, khó test, khó bảo trì
```

### ✅ Applying SRP (Áp Dụng SRP)

```csharp
// Trách nhiệm 1: Quản lý user
public class UserService
{
    private readonly IUserRepository _repository;

    public User CreateUser(string name, string email)
    {
        var user = new User { Name = name, Email = email };
        _repository.Add(user);
        return user;
    }
}

// Trách nhiệm 2: Gửi email
public class EmailService
{
    public void SendWelcomeEmail(string email)
    {
        // Logic gửi email
    }
}

// Trách nhiệm 3: Ghi log
public class Logger : ILogger
{
    public void Log(string message)
    {
        // Logic ghi log
    }
}

// Trách nhiệm 4: Tính toán
public class UserScoringService
{
    public decimal CalculateScore(User user)
    {
        // Logic tính điểm
        return 0;
    }
}
```

---

## 🎯 Real-World Example / Ví Dụ Thực Tế

### ❌ Before SRP

```csharp
public class OrderService
{
    // 1. Tạo order
    public void CreateOrder(Order order) { }

    // 2. Xử lý thanh toán
    public void ProcessPayment(Order order) { }

    // 3. Gửi email
    public void SendConfirmationEmail(Order order) { }

    // 4. Cập nhật inventory
    public void UpdateInventory(Order order) { }

    // 5. In hoá đơn
    public void PrintInvoice(Order order) { }
}
```

### ✅ After SRP

```csharp
// 1. Quản lý order
public class OrderService
{
    private readonly IOrderRepository _repository;

    public Order CreateOrder(Order order)
    {
        _repository.Add(order);
        return order;
    }
}

// 2. Xử lý thanh toán
public class PaymentProcessor
{
    public bool ProcessPayment(Order order) { }
}

// 3. Gửi email
public class EmailNotificationService
{
    public void SendConfirmationEmail(Order order) { }
}

// 4. Cập nhật inventory
public class InventoryService
{
    public void UpdateInventory(Order order) { }
}

// 5. In hoá đơn
public class InvoicePrinter
{
    public void PrintInvoice(Order order) { }
}

// Orchestration / Điều phối
public class OrderProcessor
{
    private readonly OrderService _orderService;
    private readonly PaymentProcessor _paymentProcessor;
    private readonly EmailNotificationService _emailService;
    private readonly InventoryService _inventoryService;
    private readonly InvoicePrinter _invoicePrinter;

    public async Task ProcessOrderAsync(Order order)
    {
        _orderService.CreateOrder(order);
        _paymentProcessor.ProcessPayment(order);
        _emailService.SendConfirmationEmail(order);
        _inventoryService.UpdateInventory(order);
        _invoicePrinter.PrintInvoice(order);
    }
}
```

---

## ✅ Benefits / Lợi Ích

| Lợi Ích         | Mô Tả                                       |
| --------------- | ------------------------------------------- |
| **Dễ Test**     | Mỗi class chỉ test một điều, không phức tạp |
| **Dễ Bảo Trì**  | Thay đổi một chức năng không ảnh hưởng khác |
| **Tái Sử Dụng** | Class nhỏ, focused dễ tái sử dụng           |
| **Dễ Hiểu**     | Class nhỏ dễ hiểu hơn                       |
| **Linh Hoạt**   | Dễ thêm/sửa chức năng                       |

---

## ✅ How to Apply / Cách Áp Dụng

### Step 1: Xác Định Trách Nhiệm / Identify Responsibilities

```csharp
// Hỏi: Class này làm những gì?
public class UserService
{
    // Trách nhiệm 1: Quản lý user (get, create, update, delete)
    // Trách nhiệm 2: Validate user
    // Trách nhiệm 3: Gửi email
    // Trách nhiệm 4: Lưu log
}
```

### Step 2: Tách Rời Trách Nhiệm / Separate Responsibilities

```csharp
public class UserService
{
    // Chỉ quản lý user
    public User CreateUser(string name, string email) { }
}

public class UserValidator
{
    // Chỉ validate
    public bool IsValidEmail(string email) { }
}

public class EmailService
{
    // Chỉ gửi email
    public void SendEmail(string to, string message) { }
}
```

---

## 🔴 Common Mistakes / Lỗi Thường Gặp

```csharp
// ❌ WRONG - God Class (Lớp Chúa)
public class UserController
{
    // Validate
    // Service logic
    // Database access
    // Email sending
    // Logging
    // File handling
}

// ✅ CORRECT - Separated Concerns
public class UserController
{
    // Chỉ handle HTTP requests
}
```

---

## 🎓 Interview Questions / Câu Hỏi Phỏng Vấn

1. **SRP là gì? Tại sao lại quan trọng?**
   - What is SRP? Why is it important?

2. **Làm sao biết một class có quá nhiều trách nhiệm?**
   - How do you identify if a class has too many responsibilities?

3. **Class nên có bao nhiêu public methods?**
   - How many public methods should a class have?

4. **Composition vs Inheritance trong context SRP?**
   - Composition vs Inheritance in context of SRP?

5. **Ví dụ về việc vi phạm SRP?**
   - Give an example of SRP violation?

---

## 📚 Related Topics / Chủ Đề Liên Quan

- [Open/Closed Principle](ocp.md)
- [Dependency Inversion Principle](dip.md)
- [Design Patterns - Single Responsibility](../design-patterns/)
