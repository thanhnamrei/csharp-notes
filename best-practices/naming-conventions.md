# Naming Conventions / Quy Ước Đặt Tên

## 📖 Tổng Quan / Overview

Quy ước đặt tên giúp code dễ đọc, dễ bảo trì, và tạo consistency trong codebase. C# có các quy ước đặt tên chuẩn từ Microsoft.

Naming conventions make code readable, maintainable, and consistent. C# follows Microsoft's standard naming guidelines.

---

## 📋 Các Quy Ước Chính / Main Conventions

### 1. **PascalCase** - Class, Method, Property, Namespace

```csharp
// Class / Lớp
public class CustomerService { }

// Method / Phương thức
public void ProcessPayment() { }

// Property / Thuộc tính
public string FirstName { get; set; }

// Namespace / Không gian tên
namespace MyCompany.Services.Payment { }

// Enum / Liệt kê
public enum OrderStatus { }
```

### 2. **camelCase** - Local Variables, Parameters

```csharp
public void CalculateTotal(decimal unitPrice, int quantity)
{
    decimal totalAmount = unitPrice * quantity;
    int discountPercentage = 10;
    string customerName = "John";
}
```

### 3. **\_camelCase** - Private Fields

```csharp
public class BankAccount
{
    private decimal _balance;
    private string _accountNumber;
    private DateTime _createdDate;

    public decimal Balance
    {
        get { return _balance; }
    }
}
```

### 4. **UPPER_CASE** - Constants

```csharp
public class AppConfig
{
    public const int MAX_RETRY_ATTEMPTS = 3;
    public const string DATABASE_CONNECTION_STRING = "Server=...";
    public const decimal TAX_RATE = 0.1m;
}
```

---

## 🎯 Naming by Type / Đặt Tên Theo Loại

| Type           | Convention               | Example                               |
| -------------- | ------------------------ | ------------------------------------- |
| Class          | PascalCase               | `public class UserService`            |
| Interface      | PascalCase with I prefix | `public interface ILogger`            |
| Method         | PascalCase               | `public void GetUserData()`           |
| Property       | PascalCase               | `public string Email { get; set; }`   |
| Local Variable | camelCase                | `int userId = 123;`                   |
| Private Field  | \_camelCase              | `private string _password;`           |
| Constant       | UPPER_CASE               | `const int MAX_SIZE = 100;`           |
| Parameter      | camelCase                | `public void Update(string userName)` |
| Enum           | PascalCase               | `public enum OrderStatus`             |
| Enum Member    | PascalCase               | `Pending, Approved, Rejected`         |

---

## 📝 Detailed Examples / Ví Dụ Chi Tiết

### Class Names / Tên Lớp

```csharp
// ✅ GOOD
public class CustomerRepository { }
public class OrderService { }
public class PaymentProcessor { }

// ❌ AVOID
public class customer { }
public class OrderSvc { }
public class ProcessPayment { }  // Looks like a method
```

### Method Names / Tên Phương Thức

```csharp
// ✅ GOOD - Use verbs
public void SaveUser() { }
public bool ValidateEmail(string email) { }
public List<Order> GetUserOrders(int userId) { }
public void DeleteAccount(int accountId) { }

// ❌ AVOID
public void Save1() { }
public void Validate_Email() { }  // Underscore not typical
public void user_orders() { }      // Should be PascalCase
```

### Property Names / Tên Thuộc Tính

```csharp
public class User
{
    // ✅ GOOD
    public string FirstName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }

    // ❌ AVOID
    public string first_name { get; set; }
    public string email_address { get; set; }
    public DateTime created { get; set; }
    public bool Active { get; set; }  // Should indicate boolean with Is/Has
}
```

### Boolean Naming / Đặt Tên Boolean

```csharp
public class User
{
    // ✅ GOOD - Use Is, Has, Can, Should prefix
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public bool HasPermission { get; set; }
    public bool CanDelete { get; set; }
    public bool ShouldRetry { get; set; }

    // ❌ AVOID
    public bool Active { get; set; }        // Ambiguous
    public bool Status { get; set; }        // Sounds like string
    public bool Delete { get; set; }        // Sounds like method
}
```

### Interface Names / Tên Interface

```csharp
// ✅ GOOD - Start with 'I'
public interface IRepository { }
public interface ILogger { }
public interface IPaymentProcessor { }
public interface IValidator { }

// ❌ AVOID
public interface Repository { }
public interface Logger { }
public interface Validatable { }  // Sounds like adjective
```

### Private Field Names / Tên Field Private

```csharp
public class OrderService
{
    // ✅ GOOD
    private IRepository _repository;
    private ILogger _logger;
    private decimal _totalAmount;

    // ❌ AVOID
    private IRepository repository;         // Missing underscore
    private ILogger Logger;                 // PascalCase for private
    private decimal m_totalAmount;          // Hungarian notation
}
```

---

## 📋 Special Naming Cases / Các Trường Hợp Đặc Biệt

### Async Methods / Phương Thức Bất Đồng Bộ

```csharp
// ✅ GOOD - Suffix with Async
public async Task<User> GetUserAsync(int userId) { }
public async Task SaveUserAsync(User user) { }
public async Task<bool> ValidateEmailAsync(string email) { }

// ❌ AVOID
public async Task<User> GetUser() { }
public async Task Save() { }
```

### Abbreviations / Viết Tắt

```csharp
// ✅ GOOD - Avoid abbreviations
public class HtmlParser { }
public string XmlContent { get; set; }
public int UserId { get; set; }

// ❌ AVOID
public class HtmlPrsr { }
public string XmlCont { get; set; }
public int UID { get; set; }
```

---

## ✅ Best Practices

1. **Be Descriptive** - Tên phải rõ ràng, mô tả đúng mục đích
2. **Avoid Abbreviations** - Tránh viết tắt (exception: standard abbreviations)
3. **Use Meaningful Names** - Bạn sẽ đọc code nhiều hơn viết
4. **Be Consistent** - Quy ước giống nhau throughout codebase
5. **Use English** - Dùng tiếng Anh cho code
6. **Avoid Special Characters** - Chỉ dùng letters, numbers, underscore

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG
public class usr { }           // Should be PascalCase
public int userId = 5;         // Field should be camelCase for local var, _userId for private
public const int maxSize = 10; // Should be UPPER_CASE
public void get_data() { }     // Should be PascalCase
public bool status;            // For boolean, use Is/Has prefix

// ✅ CORRECT
public class User { }
public int userId = 5;
public const int MAX_SIZE = 10;
public void GetData() { }
public bool IsActive { get; set; }
```

---

## 🎓 Interview Questions

1. **C# naming conventions là gì?**
2. **PascalCase vs camelCase là gì?**
3. **Tại sao private fields nên bắt đầu với underscore?**
4. **Boolean properties nên bắt đầu với gì?**
5. **Async methods nên kết thúc bằng gì?**

---

## 📚 References

- Microsoft C# Coding Conventions
- StyleCop Analyzers
- Roslyn Analyzers

---

## 📚 Related Topics

- [Code Structure](code-structure.md)
- [SOLID Principles](../solid-principles/)
