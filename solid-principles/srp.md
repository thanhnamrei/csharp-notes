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

### 1. **SRP là gì? Tại sao lại quan trọng?**

- **What is SRP? Why is it important?**

**Trả lời:**

SRP (Single Responsibility Principle) là một trong 5 nguyên tắc SOLID, phát biểu rằng **một class chỉ nên có một lý do duy nhất để thay đổi**, tức là mỗi class chỉ nên đảm nhận **một trách nhiệm duy nhất**.

**Tại sao quan trọng:**

- **Dễ bảo trì:** Khi thay đổi một chức năng, chỉ cần sửa một class liên quan, không ảnh hưởng đến các class khác
- **Dễ kiểm thử:** Mỗi class chỉ test một trách nhiệm cụ thể, giảm độ phức tạp của unit test
- **Tái sử dụng cao:** Class nhỏ, tập trung vào một việc dễ tái sử dụng ở nhiều nơi
- **Giảm coupling:** Các class không phụ thuộc lẫn nhau quá nhiều
- **Code dễ hiểu:** Class nhỏ gọn, rõ ràng, dễ đọc và hiểu hơn

**Ví dụ thực tế:** Thay vì có một class `UserService` làm mọi việc (validate, lưu DB, gửi email, log), ta tách thành `UserService` (quản lý user), `EmailService` (gửi email), `UserValidator` (validate), `Logger` (log).

---

### 2. **Làm sao biết một class có quá nhiều trách nhiệm?**

- **How do you identify if a class has too many responsibilities?**

**Trả lời:**

Có nhiều dấu hiệu cho thấy một class vi phạm SRP:

**1. "And" Test:**

- Nếu mô tả class phải dùng từ "và" (and): `UserService quản lý user VÀ gửi email VÀ log` → Vi phạm SRP

**2. Quá nhiều phương thức public:**

- Class có hàng chục methods không liên quan đến nhau

**3. Quá nhiều dependencies:**

- Constructor inject quá nhiều dependencies (>3-5)

**4. Thay đổi nhiều lý do:**

- Khi logic email thay đổi phải sửa class này
- Khi logic database thay đổi cũng phải sửa class này
- Khi format log thay đổi cũng phải sửa class này

**5. Tên class không rõ ràng:**

- Tên class mơ hồ như `Manager`, `Helper`, `Utility`, `Handler`

**6. Class quá dài:**

- Class có hàng trăm, hàng nghìn dòng code

**7. Khó test:**

- Phải mock quá nhiều dependencies để test một method

**Ví dụ:**

```csharp
// ❌ Vi phạm - UserService làm quá nhiều việc
public class UserService
{
    private IDatabase _db;
    private IEmailSender _email;
    private ILogger _logger;
    private IValidator _validator;
    private IFileStorage _storage;
    // ... nhiều dependencies khác

    public void CreateUser() { }
    public void SendEmail() { }
    public void ValidateUser() { }
    public void UploadAvatar() { }
    public void LogActivity() { }
    // ... nhiều methods không liên quan
}
```

---

### 3. **Class nên có bao nhiêu public methods?**

- **How many public methods should a class have?**

**Trả lời:**

**Không có con số cụ thể**, nhưng có những nguyên tắc chung:

**Nguyên tắc:**

- Số lượng methods phụ thuộc vào **độ phức tạp của trách nhiệm duy nhất** mà class đảm nhận
- Nếu tất cả methods đều liên quan đến **cùng một trách nhiệm** → Số lượng không quan trọng
- Nếu các methods liên quan đến **nhiều trách nhiệm khác nhau** → Vi phạm SRP

**Guideline chung:**

- **Repository class:** 4-7 methods (CRUD + Find)
- **Service class:** 3-10 methods (business operations)
- **Helper/Utility class:** Có thể nhiều nếu cùng một domain

**Ví dụ hợp lệ:**

```csharp
// ✅ GOOD - 5 methods nhưng cùng một trách nhiệm
public interface IUserRepository
{
    User GetById(int id);
    IEnumerable<User> GetAll();
    void Add(User user);
    void Update(User user);
    void Delete(int id);
}
```

**Ví dụ vi phạm:**

```csharp
// ❌ BAD - 5 methods nhưng 5 trách nhiệm khác nhau
public class UserService
{
    void CreateUser() { }      // Trách nhiệm 1: User management
    void SendEmail() { }        // Trách nhiệm 2: Email
    void LogActivity() { }      // Trách nhiệm 3: Logging
    void GenerateReport() { }   // Trách nhiệm 4: Reporting
    void BackupData() { }       // Trách nhiệm 5: Backup
}
```

**Điểm mấu chốt:** Không phải số lượng methods, mà là **cohesion** (độ liên kết) giữa các methods. Nếu các methods đều phục vụ cùng một mục đích → OK.

---

### 4. **Composition vs Inheritance trong context SRP?**

- **Composition vs Inheritance in context of SRP?**

**Trả lời:**

**Composition (Has-A)** thường phù hợp hơn **Inheritance (Is-A)** trong việc tuân thủ SRP vì:

**1. Composition - Ưu điểm với SRP:**

- **Tách biệt trách nhiệm rõ ràng:** Mỗi component là một class riêng với trách nhiệm riêng
- **Linh hoạt:** Có thể thay đổi behavior tại runtime
- **Tránh God Class:** Không tích lũy quá nhiều chức năng trong một class cha
- **Dễ test:** Mock từng dependency riêng lẻ

```csharp
// ✅ GOOD - Composition tuân thủ SRP
public class OrderProcessor
{
    private readonly IOrderValidator _validator;      // Trách nhiệm: Validate
    private readonly IPaymentProcessor _payment;      // Trách nhiệm: Payment
    private readonly IEmailService _emailService;     // Trách nhiệm: Email
    private readonly ILogger _logger;                 // Trách nhiệm: Logging

    public OrderProcessor(
        IOrderValidator validator,
        IPaymentProcessor payment,
        IEmailService emailService,
        ILogger logger)
    {
        _validator = validator;
        _payment = payment;
        _emailService = emailService;
        _logger = logger;
    }

    public void ProcessOrder(Order order)
    {
        _validator.Validate(order);
        _payment.Process(order);
        _emailService.SendConfirmation(order);
        _logger.Log($"Order {order.Id} processed");
    }
}
```

**2. Inheritance - Vấn đề với SRP:**

- **Kế thừa nhiều trách nhiệm:** Class con kế thừa tất cả trách nhiệm của class cha
- **Rigid structure:** Khó thay đổi behavior
- **Tight coupling:** Class con phụ thuộc chặt chẽ vào class cha
- **Bloated base class:** Class cha có xu hướng phình to theo thời gian

```csharp
// ❌ BAD - Inheritance vi phạm SRP
public class BaseService
{
    protected void SendEmail() { }      // Trách nhiệm 1
    protected void LogActivity() { }    // Trách nhiệm 2
    protected void ValidateInput() { }  // Trách nhiệm 3
}

public class UserService : BaseService
{
    // Kế thừa tất cả 3 trách nhiệm từ base class
    // + Thêm trách nhiệm quản lý user
    public void CreateUser() { }
}
```

**Khi nào dùng Inheritance:**

- Khi có quan hệ **"Is-A"** thực sự (Dog is an Animal)
- Khi muốn **polymorphism** và **abstraction**
- Khi base class **không có implementation** (abstract class, interface)

**Nguyên tắc chung:** **"Favor Composition over Inheritance"** - Ưu tiên Composition hơn Inheritance để tuân thủ SRP tốt hơn.

---

### 5. **Ví dụ về việc vi phạm SRP?**

- **Give an example of SRP violation?**

**Trả lời:**

**Ví dụ 1: God Class - UserManager**

```csharp
// ❌ VI PHẠM SRP - UserManager làm quá nhiều việc
public class UserManager
{
    // Trách nhiệm 1: Database operations
    public void SaveToDatabase(User user)
    {
        using (var connection = new SqlConnection("..."))
        {
            // SQL code
        }
    }

    // Trách nhiệm 2: Email
    public void SendWelcomeEmail(User user)
    {
        var smtp = new SmtpClient();
        // Email logic
    }

    // Trách nhiệm 3: Validation
    public bool ValidateEmail(string email)
    {
        return email.Contains("@");
    }

    // Trách nhiệm 4: Password hashing
    public string HashPassword(string password)
    {
        // Hashing logic
    }

    // Trách nhiệm 5: Logging
    public void LogUserAction(string action)
    {
        File.AppendAllText("log.txt", action);
    }

    // Trách nhiệm 6: Generate report
    public void GenerateUserReport()
    {
        // Report generation
    }
}
```

**Vấn đề:**

- Khi thay đổi logic email → phải sửa UserManager
- Khi thay đổi cách hash password → phải sửa UserManager
- Khi thay đổi database → phải sửa UserManager
- Class quá lớn, khó test, khó maintain

**✅ GIẢI PHÁP - Tách thành nhiều class tuân thủ SRP:**

```csharp
// Class 1: Chỉ quản lý User business logic
public class UserService
{
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;
    private readonly ILogger _logger;

    public void CreateUser(User user)
    {
        _repository.Add(user);
        _emailService.SendWelcomeEmail(user.Email);
        _logger.Log($"User {user.Id} created");
    }
}

// Class 2: Chỉ database operations
public class UserRepository : IUserRepository
{
    public void Add(User user)
    {
        // Database logic only
    }
}

// Class 3: Chỉ email
public class EmailService : IEmailService
{
    public void SendWelcomeEmail(string email)
    {
        // Email logic only
    }
}

// Class 4: Chỉ validation
public class UserValidator : IUserValidator
{
    public bool ValidateEmail(string email)
    {
        return email.Contains("@");
    }
}

// Class 5: Chỉ password
public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        // Hashing logic only
    }
}

// Class 6: Chỉ logging
public class Logger : ILogger
{
    public void Log(string message)
    {
        File.AppendAllText("log.txt", message);
    }
}

// Class 7: Chỉ reporting
public class UserReportGenerator
{
    public void GenerateReport()
    {
        // Report logic only
    }
}
```

**Lợi ích:**

- Mỗi class chỉ có **một lý do để thay đổi**
- Dễ test từng class riêng lẻ
- Dễ tái sử dụng (ví dụ: EmailService có thể dùng cho Order, Product...)
- Code dễ đọc, dễ hiểu hơn

---

## 📚 Related Topics / Chủ Đề Liên Quan

- [Open/Closed Principle](ocp.md)
- [Dependency Inversion Principle](dip.md)
- [Design Patterns - Single Responsibility](../design-patterns/)
