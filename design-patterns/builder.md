# Builder Pattern / Mẫu Xây Dựng

## 📖 Định Nghĩa / Definition

**Builder** là design pattern cho phép tạo object **phức tạp từng bước** và **tách logic constructor** khỏi object.

**Builder** is a design pattern that allows building **complex objects step-by-step** and **separates construction from representation**.

---

## 💡 Khi Nào Dùng / When to Use

✅ HTTP Requests - Xây dựng request với nhiều header, payload...
✅ SQL Queries - Xây dựng query với WHERE, JOIN, ORDER BY...
✅ Configuration Objects - Xây dựng settings với nhiều options
✅ Document Creation - Xây dựng document với format, style...
✅ Immutable Objects - Tạo object không thay đổi được

---

## 📝 Implementations / Các Cách Implement

### 1. **Classic Builder Pattern**

```csharp
public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    private User() { } // Private constructor

    public class Builder
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private int _age;
        private string _phone;
        private string _address;

        public Builder WithFirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        public Builder WithLastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }

        public Builder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public Builder WithAge(int age)
        {
            _age = age;
            return this;
        }

        public Builder WithPhone(string phone)
        {
            _phone = phone;
            return this;
        }

        public Builder WithAddress(string address)
        {
            _address = address;
            return this;
        }

        public User Build()
        {
            return new User
            {
                FirstName = _firstName,
                LastName = _lastName,
                Email = _email,
                Age = _age,
                Phone = _phone,
                Address = _address
            };
        }
    }
}

// Usage
var user = new User.Builder()
    .WithFirstName("John")
    .WithLastName("Doe")
    .WithEmail("john@example.com")
    .WithAge(30)
    .Build();
```

### 2. **Fluent Interface Builder**

```csharp
public class HttpRequestBuilder
{
    private string _url;
    private string _method = "GET";
    private Dictionary<string, string> _headers = new();
    private string _body;

    public HttpRequestBuilder WithUrl(string url)
    {
        _url = url;
        return this;
    }

    public HttpRequestBuilder WithMethod(string method)
    {
        _method = method;
        return this;
    }

    public HttpRequestBuilder AddHeader(string key, string value)
    {
        _headers[key] = value;
        return this;
    }

    public HttpRequestBuilder WithBody(string body)
    {
        _body = body;
        return this;
    }

    public HttpRequest Build()
    {
        if (string.IsNullOrEmpty(_url))
            throw new InvalidOperationException("URL is required");

        return new HttpRequest
        {
            Url = _url,
            Method = _method,
            Headers = _headers,
            Body = _body
        };
    }
}

public class HttpRequest
{
    public string Url { get; set; }
    public string Method { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    public string Body { get; set; }

    public override string ToString()
    {
        return $"{Method} {Url}";
    }
}

// Usage
var request = new HttpRequestBuilder()
    .WithUrl("https://api.example.com/users")
    .WithMethod("POST")
    .AddHeader("Content-Type", "application/json")
    .AddHeader("Authorization", "Bearer token123")
    .WithBody("{\"name\": \"John\"}")
    .Build();
```

### 3. **StringBuilder (Built-in C# Example)**

```csharp
// C# đã implement Builder pattern trong StringBuilder
var sb = new System.Text.StringBuilder();
var result = sb
    .Append("Hello")
    .Append(" ")
    .Append("World")
    .ToString();

Console.WriteLine(result); // Hello World
```

---

## 🎯 Real-World Example / Ví Dụ Thực Tế

### **SQL Query Builder**

```csharp
public class SqlQueryBuilder
{
    private string _select = "*";
    private string _from;
    private string _join;
    private string _where;
    private string _orderBy;
    private int? _limit;

    public SqlQueryBuilder Select(params string[] columns)
    {
        _select = string.Join(", ", columns);
        return this;
    }

    public SqlQueryBuilder From(string table)
    {
        _from = table;
        return this;
    }

    public SqlQueryBuilder Join(string joinClause)
    {
        _join = joinClause;
        return this;
    }

    public SqlQueryBuilder Where(string condition)
    {
        _where = condition;
        return this;
    }

    public SqlQueryBuilder OrderBy(string column, string direction = "ASC")
    {
        _orderBy = $"{column} {direction}";
        return this;
    }

    public SqlQueryBuilder Limit(int count)
    {
        _limit = count;
        return this;
    }

    public string Build()
    {
        if (string.IsNullOrEmpty(_from))
            throw new InvalidOperationException("FROM clause is required");

        var query = $"SELECT {_select} FROM {_from}";

        if (!string.IsNullOrEmpty(_join))
            query += $" {_join}";

        if (!string.IsNullOrEmpty(_where))
            query += $" WHERE {_where}";

        if (!string.IsNullOrEmpty(_orderBy))
            query += $" ORDER BY {_orderBy}";

        if (_limit.HasValue)
            query += $" LIMIT {_limit}";

        return query;
    }
}

// Usage
var query = new SqlQueryBuilder()
    .Select("id", "name", "email")
    .From("users")
    .Join("INNER JOIN orders ON users.id = orders.user_id")
    .Where("users.age > 18")
    .OrderBy("name", "ASC")
    .Limit(10)
    .Build();

Console.WriteLine(query);
// Output: SELECT id, name, email FROM users INNER JOIN orders ON users.id = orders.user_id WHERE users.age > 18 ORDER BY name ASC LIMIT 10
```

### **Email Builder (Thực Tế)**

```csharp
public class EmailBuilder
{
    private string _from;
    private List<string> _to = new();
    private List<string> _cc = new();
    private List<string> _bcc = new();
    private string _subject;
    private string _body;
    private bool _isHtml = false;

    public EmailBuilder From(string email)
    {
        _from = email;
        return this;
    }

    public EmailBuilder To(string email)
    {
        _to.Add(email);
        return this;
    }

    public EmailBuilder Cc(string email)
    {
        _cc.Add(email);
        return this;
    }

    public EmailBuilder Bcc(string email)
    {
        _bcc.Add(email);
        return this;
    }

    public EmailBuilder Subject(string subject)
    {
        _subject = subject;
        return this;
    }

    public EmailBuilder Body(string body, bool isHtml = false)
    {
        _body = body;
        _isHtml = isHtml;
        return this;
    }

    public Email Build()
    {
        if (string.IsNullOrEmpty(_from))
            throw new InvalidOperationException("From is required");
        if (_to.Count == 0)
            throw new InvalidOperationException("To is required");
        if (string.IsNullOrEmpty(_subject))
            throw new InvalidOperationException("Subject is required");

        return new Email
        {
            From = _from,
            To = _to,
            Cc = _cc,
            Bcc = _bcc,
            Subject = _subject,
            Body = _body,
            IsHtml = _isHtml
        };
    }
}

public class Email
{
    public string From { get; set; }
    public List<string> To { get; set; }
    public List<string> Cc { get; set; }
    public List<string> Bcc { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }

    public void Send()
    {
        Console.WriteLine($"Sending email to: {string.Join(", ", To)}");
        Console.WriteLine($"Subject: {Subject}");
    }
}

// Usage
var email = new EmailBuilder()
    .From("noreply@company.com")
    .To("user@gmail.com")
    .Cc("manager@company.com")
    .Subject("Welcome to our service!")
    .Body("<h1>Welcome!</h1><p>Thank you for joining us.</p>", isHtml: true)
    .Build();

email.Send();
```

---

## ✅ Advantages / Lợi Ích

| Lợi Ích              | Mô Tả                             |
| -------------------- | --------------------------------- |
| **Clear Steps**      | Xây dựng object rõ ràng từng bước |
| **Immutability**     | Tạo object immutable dễ dàng      |
| **Fluent Interface** | Code đọc như câu tiếng Anh        |
| **Flexible**         | Tùy chọn giá trị nào cần          |
| **Validation**       | Validate trước khi tạo object     |

---

## ❌ Disadvantages / Nhược Điểm

| Nhược Điểm       | Mô Tả                          |
| ---------------- | ------------------------------ |
| **More Classes** | Cần thêm Builder class         |
| **Overhead**     | Có thể overkill cho object đơn |
| **Memory Usage** | Giữ nhiều state tạm            |

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Constructor quá nhiều tham số
public class User
{
    public User(string firstName, string lastName, string email, int age,
                string phone, string address, string city, string country) { }
}

// ✅ CORRECT - Dùng Builder
var user = new User.Builder()
    .WithFirstName("John")
    .WithLastName("Doe")
    .WithEmail("john@example.com")
    .Build();
```

---

## 📚 Related Topics

- [Immutable Objects](../memory-management/)
- [Object Creation Patterns](./factory.md)
- [SOLID Principles](../solid-principles/)

---

## 🎓 Interview Questions

### 1. **Builder pattern là gì? Tại sao cần nó?**

**Câu trả lời:**

Builder Pattern là một creational design pattern cho phép xây dựng **complex objects step-by-step**. Nó tách rời quá trình construction khỏi representation, giúp tạo objects với cấu hình khác nhau từ cùng một quá trình xây dựng.

**Tại sao cần Builder Pattern?**

#### **Problem: Telescoping Constructor Anti-pattern**

```csharp
// ❌ Telescoping constructor - RẤT KHÁC ĐỌC!
public class Pizza
{
    public Pizza(int size) { }
    public Pizza(int size, bool cheese) { }
    public Pizza(int size, bool cheese, bool pepperoni) { }
    public Pizza(int size, bool cheese, bool pepperoni, bool bacon) { }
    public Pizza(int size, bool cheese, bool pepperoni, bool bacon, bool mushrooms) { }
    // ... nhiều constructors nữa!!!
}

// Sử dụng - không rõ tham số nào là gì
var pizza = new Pizza(12, true, false, true, false);
```

**Problems:**

- Quá nhiều constructors với nhiều tham số
- Không rõ tham số nào là gì (true/false không có nghĩa)
- Thứ tự tham số dễ nhầm
- Không flexible (muốn thêm option phải tạo constructor mới)

#### **Solution: Builder Pattern**

```csharp
// ✅ Builder Pattern - RÕ RÀNG, DỄ ĐỌC!
var pizza = new PizzaBuilder()
    .SetSize(12)
    .AddCheese()
    .AddPepperoni()
    .AddMushrooms()
    .Build();
```

**Benefits:**

- **Readable**: Code đọc như câu tiếng Anh tự nhiên
- **Flexible**: Chỉ set giá trị cần thiết, bỏ qua các optional parameters
- **Maintainable**: Thêm options mới không ảnh hưởng code cũ
- **Immutable objects**: Dễ tạo immutable objects
- **Validation**: Có thể validate trước khi build

#### **Khi nào cần Builder?**

✅ **NÊN dùng Builder khi:**

1. **Object có nhiều hơn 4-5 constructor parameters**
2. **Nhiều optional parameters**
3. **Cần tạo immutable objects**
4. **Quá trình construction phức tạp (nhiều bước)**
5. **Muốn step-by-step initialization với validation**

**Real-world examples:**

- HTTP Request Builder (nhiều headers, parameters)
- SQL Query Builder (SELECT, FROM, WHERE, JOIN...)
- Email/Document Builder (many configurations)
- Configuration objects
- Complex domain models

#### **Example thực tế: HTTP Request**

```csharp
// ❌ Không dùng Builder - khó đọc và maintain
public class HttpRequest
{
    public HttpRequest(string url, string method, Dictionary<string, string> headers,
                      string body, int timeout, bool followRedirects, string proxy) { }
}

var request = new HttpRequest(
    "https://api.com",
    "POST",
    new Dictionary<string, string> { {"Auth", "token"} },
    "{data}",
    5000,
    true,
    null
); // What is what???

// ✅ Dùng Builder - rõ ràng và dễ maintain
var request = new HttpRequestBuilder()
    .WithUrl("https://api.com")
    .WithMethod("POST")
    .AddHeader("Authorization", "Bearer token")
    .WithBody("{data}")
    .WithTimeout(5000)
    .FollowRedirects(true)
    .Build();
```

**Tóm tắt:** Builder Pattern giải quyết vấn đề **quá nhiều constructor parameters** và **tạo objects phức tạp** một cách **readable và maintainable**.

---

### 2. **Builder vs Constructor? Khi nào dùng Builder?**

**Câu trả lời:**

Đây là câu hỏi quan trọng để hiểu khi nào nên áp dụng Builder Pattern!

#### **Constructor (Traditional Approach)**

```csharp
public class User
{
    public string Name { get; }
    public string Email { get; }
    public int Age { get; }

    // Simple constructor
    public User(string name, string email, int age)
    {
        Name = name;
        Email = email;
        Age = age;
    }
}

// Usage
var user = new User("John", "john@email.com", 30);
```

**Ưu điểm Constructor:**

- ✅ Simple, straightforward
- ✅ Performance tốt (no overhead)
- ✅ Compile-time safety (required parameters rõ ràng)
- ✅ Phù hợp với objects đơn giản

**Nhược điểm Constructor:**

- ❌ Không scale với nhiều parameters
- ❌ Không rõ ràng với nhiều parameters cùng type
- ❌ Khó thêm optional parameters
- ❌ Không flexible

#### **Builder Pattern**

```csharp
public class User
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public int Age { get; private set; }
    public string Phone { get; private set; } // Optional
    public string Address { get; private set; } // Optional

    private User() { } // Private constructor

    public class Builder
    {
        private string _name;
        private string _email;
        private int _age;
        private string _phone;
        private string _address;

        public Builder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Builder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public Builder WithAge(int age)
        {
            _age = age;
            return this;
        }

        public Builder WithPhone(string phone)
        {
            _phone = phone;
            return this;
        }

        public Builder WithAddress(string address)
        {
            _address = address;
            return this;
        }

        public User Build()
        {
            // Validation
            if (string.IsNullOrEmpty(_name))
                throw new ArgumentException("Name is required");
            if (string.IsNullOrEmpty(_email))
                throw new ArgumentException("Email is required");

            return new User
            {
                Name = _name,
                Email = _email,
                Age = _age,
                Phone = _phone,
                Address = _address
            };
        }
    }
}

// Usage
var user = new User.Builder()
    .WithName("John")
    .WithEmail("john@email.com")
    .WithAge(30)
    .WithPhone("123-456-7890") // Optional
    .Build();
```

**Ưu điểm Builder:**

- ✅ Readable và self-documenting
- ✅ Flexible với optional parameters
- ✅ Dễ thêm properties mới
- ✅ Có thể validate trước build
- ✅ Tạo immutable objects dễ dàng
- ✅ Method chaining fluent interface

**Nhược điểm Builder:**

- ❌ More code (thêm Builder class)
- ❌ Slight performance overhead
- ❌ Overkill cho simple objects

#### **So sánh trực tiếp**

| Aspect              | Constructor                       | Builder Pattern                           |
| ------------------- | --------------------------------- | ----------------------------------------- |
| **Simplicity**      | ✅ Very simple                    | ❌ More complex                           |
| **Readability**     | ❌ Poor with many params          | ✅ Excellent                              |
| **Optional Params** | ❌ Clumsy (overloads/nulls)       | ✅ Easy and clean                         |
| **Flexibility**     | ❌ Limited                        | ✅ Very flexible                          |
| **Validation**      | ⚠️ In constructor (less flexible) | ✅ In Build() method                      |
| **Immutability**    | ⚠️ Possible but harder            | ✅ Natural fit                            |
| **Performance**     | ✅ Best                           | ⚠️ Slight overhead                        |
| **Code Amount**     | ✅ Minimal                        | ❌ More code                              |
| **Type Safety**     | ✅ Compile-time checks            | ⚠️ Runtime checks in Build()              |
| **Best For**        | Simple objects (2-4 params)       | Complex objects (5+ params, many options) |

#### **Khi nào dùng Constructor?**

✅ **Dùng Constructor khi:**

```csharp
// 1. Object đơn giản với ít parameters (2-4)
public class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

// 2. Tất cả parameters đều required
public class EmailAddress
{
    public EmailAddress(string email)
    {
        if (!IsValid(email))
            throw new ArgumentException("Invalid email");
        Email = email;
    }
}

// 3. Value objects, DTOs
public record UserDto(string Name, string Email, int Age);
```

#### **Khi nào dùng Builder?**

✅ **Dùng Builder khi:**

```csharp
// 1. Nhiều parameters (5+)
var request = new HttpRequestBuilder()
    .WithUrl("https://api.com")
    .WithMethod("POST")
    .AddHeader("Auth", "token")
    .WithBody(payload)
    .WithTimeout(5000)
    .WithRetryPolicy(3)
    .Build();

// 2. Nhiều optional parameters
var email = new EmailBuilder()
    .From("sender@email.com")
    .To("receiver@email.com")
    .Subject("Hello") // Required
    .Cc("cc@email.com") // Optional
    .Bcc("bcc@email.com") // Optional
    .Priority(EmailPriority.High) // Optional
    .Build();

// 3. Complex construction logic
var report = new ReportBuilder()
    .WithTitle("Sales Report")
    .AddSection("Summary", summaryData)
    .AddSection("Details", detailsData)
    .AddChart(ChartType.Bar, chartData)
    .WithFormat(ReportFormat.PDF)
    .Build();

// 4. Immutable objects with many properties
var config = new AppConfigBuilder()
    .WithDatabaseConnection(connString)
    .WithCaching(true)
    .WithLogging(LogLevel.Info)
    .WithRetryPolicy(3, TimeSpan.FromSeconds(2))
    .Build(); // Returns immutable config
```

#### **Hybrid Approach (Best Practice)**

Kết hợp cả hai!

```csharp
public class User
{
    // Constructor cho required fields
    public User(string name, string email)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException(nameof(name));
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException(nameof(email));

        Name = name;
        Email = email;
    }

    public string Name { get; }
    public string Email { get; }
    public int? Age { get; init; } // Optional với init
    public string Phone { get; init; } // Optional với init

    // Builder cho complex scenarios
    public class Builder
    {
        // Builder implementation...
    }
}

// Simple case - dùng constructor
var user1 = new User("John", "john@email.com");

// Complex case - dùng builder
var user2 = new User.Builder()
    .WithName("John")
    .WithEmail("john@email.com")
    .WithAge(30)
    .WithPhone("123-456")
    .Build();

// C# 9+ init-only setters
var user3 = new User("John", "john@email.com")
{
    Age = 30,
    Phone = "123-456"
};
```

#### **Decision Tree: Constructor hay Builder?**

```
Có > 4-5 parameters?
├─ NO → Dùng Constructor
└─ YES → Có nhiều optional parameters?
           ├─ NO → Dùng Constructor với overloads
           └─ YES → Dùng Builder Pattern ✅

Object có quá trình construction phức tạp (nhiều bước)?
├─ YES → Dùng Builder Pattern ✅
└─ NO → Dùng Constructor

Cần immutable object với nhiều properties?
├─ YES → Dùng Builder Pattern ✅
└─ NO → Dùng Constructor hoặc init properties
```

**Tóm tắt:**

- **Constructor**: Simple objects, ít parameters, tất cả required
- **Builder**: Complex objects, nhiều parameters, nhiều optional fields, construction logic phức tạp

---

### 3. **Cách validate trong Builder?**

**Câu trả lời:**

Validation trong Builder Pattern rất quan trọng để ensure object được tạo ra là valid. Có nhiều strategies để implement validation:

#### **Strategy 1: Validate trong Build() method** ⭐ (Most Common)

```csharp
public class UserBuilder
{
    private string _email;
    private string _password;
    private int _age;

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public UserBuilder WithAge(int age)
    {
        _age = age;
        return this;
    }

    public User Build()
    {
        // Validate tất cả required fields
        if (string.IsNullOrWhiteSpace(_email))
            throw new InvalidOperationException("Email is required");

        if (string.IsNullOrWhiteSpace(_password))
            throw new InvalidOperationException("Password is required");

        // Validate business rules
        if (!IsValidEmail(_email))
            throw new ArgumentException("Invalid email format");

        if (_password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters");

        if (_age < 18 || _age > 120)
            throw new ArgumentException("Age must be between 18 and 120");

        return new User
        {
            Email = _email,
            Password = _password,
            Age = _age
        };
    }

    private bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
}

// Usage
try
{
    var user = new UserBuilder()
        .WithEmail("invalid-email") // Invalid!
        .WithPassword("short") // Invalid!
        .Build(); // Throws exception here
}
catch (Exception ex)
{
    Console.WriteLine($"Validation failed: {ex.Message}");
}
```

**Ưu điểm:**

- ✅ Validate tất cả fields cùng lúc
- ✅ Object chỉ tạo khi valid
- ✅ Clear separation: build vs validation

**Nhược điểm:**

- ❌ Lỗi chỉ được phát hiện ở cuối (Build time)

#### **Strategy 2: Validate trong Each Setter Method**

```csharp
public class UserBuilder
{
    private string _email;
    private string _password;
    private int _age;

    public UserBuilder WithEmail(string email)
    {
        // Immediate validation
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty");

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format");

        _email = email;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        // Immediate validation
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty");

        if (password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters");

        _password = password;
        return this;
    }

    public UserBuilder WithAge(int age)
    {
        // Immediate validation
        if (age < 18 || age > 120)
            throw new ArgumentException("Age must be between 18 and 120");

        _age = age;
        return this;
    }

    public User Build()
    {
        // Only check required fields are set
        if (string.IsNullOrEmpty(_email))
            throw new InvalidOperationException("Email is required");

        if (string.IsNullOrEmpty(_password))
            throw new InvalidOperationException("Password is required");

        return new User { Email = _email, Password = _password, Age = _age };
    }

    private bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
}

// Usage
try
{
    var user = new UserBuilder()
        .WithEmail("invalid-email") // Throws immediately!
        .WithPassword("short")
        .Build();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

**Ưu điểm:**

- ✅ Fail fast - lỗi được phát hiện ngay
- ✅ Better developer experience

**Nhược điểm:**

- ❌ Không validate được cross-field validation
- ❌ Có thể throw exception ở nhiều chỗ

#### **Strategy 3: Try-Build Pattern with Result Object** ⭐ (Modern, Best Practice)

```csharp
// Result object để trả về validation errors
public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<string> Errors { get; } = new();

    public void AddError(string error)
    {
        Errors.Add(error);
    }
}

public class UserBuilder
{
    private string _email;
    private string _password;
    private int _age;

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public UserBuilder WithAge(int age)
    {
        _age = age;
        return this;
    }

    // Try-Build pattern - không throw exception
    public ValidationResult TryBuild(out User user)
    {
        user = null;
        var result = new ValidationResult();

        // Validate all fields
        if (string.IsNullOrWhiteSpace(_email))
            result.AddError("Email is required");
        else if (!IsValidEmail(_email))
            result.AddError("Invalid email format");

        if (string.IsNullOrWhiteSpace(_password))
            result.AddError("Password is required");
        else if (_password.Length < 8)
            result.AddError("Password must be at least 8 characters");

        if (_age < 18 || _age > 120)
            result.AddError("Age must be between 18 and 120");

        // Only create user if valid
        if (result.IsValid)
        {
            user = new User
            {
                Email = _email,
                Password = _password,
                Age = _age
            };
        }

        return result;
    }

    // Traditional Build method for backward compatibility
    public User Build()
    {
        var result = TryBuild(out var user);
        if (!result.IsValid)
        {
            throw new InvalidOperationException(
                $"Validation failed: {string.Join(", ", result.Errors)}");
        }
        return user;
    }

    private bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
}

// Usage - NO exceptions!
var builder = new UserBuilder()
    .WithEmail("invalid-email")
    .WithPassword("short")
    .WithAge(150);

var result = builder.TryBuild(out var user);

if (result.IsValid)
{
    Console.WriteLine("User created successfully!");
}
else
{
    Console.WriteLine("Validation errors:");
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"- {error}");
    }
}

// Output:
// Validation errors:
// - Invalid email format
// - Password must be at least 8 characters
// - Age must be between 18 and 120
```

**Ưu điểm:**

- ✅ Không throw exceptions (better performance)
- ✅ Trả về TẤT CẢ validation errors cùng lúc
- ✅ Better UX (user thấy all errors, không phải fix từng lỗi)
- ✅ Testable

**Nhược điểm:**

- ❌ More code

#### **Strategy 4: FluentValidation Library Integration** ⭐ (Production-grade)

```csharp
using FluentValidation;

// Validator class
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");

        RuleFor(x => x.Age)
            .InclusiveBetween(18, 120).WithMessage("Age must be between 18 and 120");
    }
}

public class UserBuilder
{
    private string _email;
    private string _password;
    private int _age;
    private readonly UserValidator _validator = new();

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public UserBuilder WithAge(int age)
    {
        _age = age;
        return this;
    }

    public User Build()
    {
        var user = new User
        {
            Email = _email,
            Password = _password,
            Age = _age
        };

        // Validate using FluentValidation
        var validationResult = _validator.Validate(user);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new InvalidOperationException($"Validation failed: {errors}");
        }

        return user;
    }
}
```

#### **Strategy 5: Composite Validation (Cross-field Validation)**

```csharp
public class OrderBuilder
{
    private decimal _totalAmount;
    private decimal _discount;
    private string _couponCode;
    private bool _isPremiumMember;

    public OrderBuilder WithTotalAmount(decimal amount)
    {
        _totalAmount = amount;
        return this;
    }

    public OrderBuilder WithDiscount(decimal discount)
    {
        _discount = discount;
        return this;
    }

    public OrderBuilder WithCoupon(string code)
    {
        _couponCode = code;
        return this;
    }

    public OrderBuilder AsPremiumMember()
    {
        _isPremiumMember = true;
        return this;
    }

    public Order Build()
    {
        var errors = new List<string>();

        // Basic validation
        if (_totalAmount <= 0)
            errors.Add("Total amount must be greater than 0");

        if (_discount < 0 || _discount > 100)
            errors.Add("Discount must be between 0 and 100");

        // Cross-field validation
        if (_discount > 50 && !_isPremiumMember)
            errors.Add("Discount over 50% is only available for premium members");

        if (!string.IsNullOrEmpty(_couponCode) && _discount > 0)
            errors.Add("Cannot use both coupon and discount");

        if (_totalAmount < 100 && !string.IsNullOrEmpty(_couponCode))
            errors.Add("Coupon only valid for orders over $100");

        if (errors.Any())
        {
            throw new InvalidOperationException(
                $"Validation failed: {string.Join("; ", errors)}");
        }

        return new Order
        {
            TotalAmount = _totalAmount,
            Discount = _discount,
            CouponCode = _couponCode,
            IsPremiumMember = _isPremiumMember
        };
    }
}
```

#### **Best Practices cho Validation trong Builder**

| Practice                          | Description                                       |
| --------------------------------- | ------------------------------------------------- |
| **Validate in Build()**           | ⭐ Recommended - Validate tất cả at build time    |
| **Use Try-Build Pattern**         | ⭐ Best for UX - Return all errors at once        |
| **Required vs Optional**          | Clearly separate required vs optional fields      |
| **Business Rules**                | Validate business logic, not just null checks     |
| **Cross-field Validation**        | Validate relationships between fields             |
| **Clear Error Messages**          | Provide actionable error messages                 |
| **Fail Fast (Optional)**          | Validate each setter for immediate feedback       |
| **Use Validation Libraries**      | FluentValidation for complex validation scenarios |
| **Don't Over-validate in Setter** | Giữ setters simple, validate trong Build()        |

**Tóm tắt:** Validation trong `Build()` method là approach phổ biến nhất. Với complex scenarios, dùng Try-Build pattern hoặc FluentValidation library.

---

### 4. **Builder pattern có ảnh hưởng hiệu năng không?**

**Câu trả lời:**

Có, Builder Pattern có **slight performance overhead** so với direct constructor, nhưng trong hầu hết trường hợp **không đáng kể**. Let's analyze chi tiết:

#### **Performance Comparison**

```csharp
// Setup for benchmarking
public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}

// Method 1: Direct Constructor
public User CreateWithConstructor()
{
    return new User
    {
        Name = "John",
        Email = "john@email.com",
        Age = 30,
        Phone = "123-456",
        Address = "123 Street"
    };
}

// Method 2: Builder Pattern
public User CreateWithBuilder()
{
    return new UserBuilder()
        .WithName("John")
        .WithEmail("john@email.com")
        .WithAge(30)
        .WithPhone("123-456")
        .WithAddress("123 Street")
        .Build();
}
```

#### **Benchmark Results** (Using BenchmarkDotNet)

```
| Method              | Mean      | Allocated |
|---------------------|-----------|-----------|
| Constructor         | 15.2 ns   | 64 B      |
| Builder             | 45.8 ns   | 128 B     |
| Difference          | +30.6 ns  | +64 B     |
```

**Analysis:**

- Builder is ~3x slower (45.8ns vs 15.2ns)
- Builder allocates 2x more memory (128B vs 64B)
- **BUT:** 30 nanoseconds difference is NEGLIGIBLE for most applications!

#### **Where Performance Cost Comes From**

```csharp
// 1. Extra object allocation (Builder object)
var builder = new UserBuilder(); // ← Extra allocation

// 2. Method call overhead (multiple method calls)
builder.WithName("John")        // ← Method call
       .WithEmail("email")      // ← Method call
       .WithAge(30)             // ← Method call
       .Build();                // ← Method call

// 3. Intermediate state storage
private string _name;    // ← Builder stores temp values
private string _email;   // ← Before creating final object
private int _age;

// 4. Object creation in Build()
return new User { ... }; // ← Final object creation
```

#### **When Performance Matters**

❌ **Builder có thể chậm khi:**

```csharp
// 1. Creating millions of objects in tight loop
for (int i = 0; i < 10_000_000; i++)
{
    var user = new UserBuilder()  // ← 10M allocations!
        .WithName("John")
        .WithEmail("email")
        .Build();
}

// 2. High-frequency operations (game loops, real-time systems)
void UpdateFrame() // Called 60 times per second
{
    var config = new ConfigBuilder()
        .WithOption1(true)
        .Build(); // ← Wasteful!
}

// 3. Memory-constrained environments
public class EmbeddedSystem
{
    // Very limited RAM - extra allocations matter!
    void ProcessSensorData()
    {
        var data = new SensorDataBuilder().Build(); // ← Avoid!
    }
}
```

✅ **Builder performance OK khi:**

```csharp
// 1. One-time initialization
var config = new AppConfigBuilder()
    .WithDatabase(connectionString)
    .WithLogging(LogLevel.Info)
    .Build(); // ← Fine! Only runs once at startup

// 2. User-driven operations (UI, API requests)
[HttpPost]
public IActionResult CreateUser(UserDto dto)
{
    var user = new UserBuilder()  // ← Fine! Not called frequently
        .WithName(dto.Name)
        .Build();
}

// 3. Complex object construction
var report = new ReportBuilder()
    .WithTitle("Sales Report")
    .AddSection("Summary", data)
    .AddChart(ChartType.Bar, chartData)
    .Build(); // ← Fine! Readability > tiny perf loss
```

#### **Optimization Techniques**

**1. Object Pooling (cho high-frequency scenarios)**

```csharp
public class UserBuilderPool
{
    private static readonly ObjectPool<UserBuilder> _pool =
        new ObjectPool<UserBuilder>(() => new UserBuilder());

    public static UserBuilder Get()
    {
        var builder = _pool.Get();
        builder.Reset(); // Clear previous state
        return builder;
    }

    public static void Return(UserBuilder builder)
    {
        _pool.Return(builder);
    }
}

// Usage
var builder = UserBuilderPool.Get();
try
{
    var user = builder
        .WithName("John")
        .Build();
}
finally
{
    UserBuilderPool.Return(builder); // Reuse builder
}
```

**2. Struct Builder (cho value types)**

```csharp
// Struct builder = no heap allocation
public struct PointBuilder
{
    private int _x;
    private int _y;

    public PointBuilder WithX(int x)
    {
        _x = x;
        return this;
    }

    public PointBuilder WithY(int y)
    {
        _y = y;
        return this;
    }

    public Point Build() => new Point(_x, _y);
}

// Usage - no heap allocation for builder!
var point = new PointBuilder()
    .WithX(10)
    .WithY(20)
    .Build();
```

**3. Lazy Builder (reuse for multiple objects)**

```csharp
public class ConfigBuilder
{
    private Dictionary<string, string> _settings = new();

    public ConfigBuilder AddSetting(string key, string value)
    {
        _settings[key] = value;
        return this;
    }

    public Config Build()
    {
        // Don't destroy builder state - can reuse!
        return new Config(new Dictionary<string, string>(_settings));
    }

    public void Reset()
    {
        _settings.Clear(); // Reuse builder
    }
}

// Reuse single builder for multiple configs
var builder = new ConfigBuilder();

var config1 = builder
    .AddSetting("timeout", "5000")
    .Build();

builder.Reset();

var config2 = builder
    .AddSetting("timeout", "3000")
    .Build();
```

**4. Static Builder Methods (reduce allocations)**

```csharp
public class HttpRequest
{
    // Static builder methods - no builder object needed
    public static HttpRequest Get(string url)
    {
        return new HttpRequest { Method = "GET", Url = url };
    }

    public static HttpRequest Post(string url, string body)
    {
        return new HttpRequest { Method = "POST", Url = url, Body = body };
    }
}

// Usage - zero overhead!
var request = HttpRequest.Get("https://api.com");
```

#### **Real-World Performance Impact**

| Scenario                     | Impact   | Use Builder?     |
| ---------------------------- | -------- | ---------------- |
| Web API endpoint             | Nil      | ✅ Yes           |
| Desktop app startup          | Nil      | ✅ Yes           |
| Background job processing    | Minimal  | ✅ Yes           |
| Database operations          | Nil      | ✅ Yes           |
| **Game engine render loop**  | Moderate | ❌ No            |
| **High-freq trading system** | High     | ❌ No            |
| **Embedded system**          | High     | ❌ No            |
| **Parsing 1M+ records/sec**  | Moderate | ⚠️ Profile first |

#### **Measurement Example**

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class BuilderBenchmark
{
    [Benchmark(Baseline = true)]
    public User Constructor()
    {
        return new User
        {
            Name = "John",
            Email = "john@email.com",
            Age = 30
        };
    }

    [Benchmark]
    public User Builder()
    {
        return new UserBuilder()
            .WithName("John")
            .WithEmail("john@email.com")
            .WithAge(30)
            .Build();
    }
}

// Run: dotnet run -c Release
// Results will show exact performance difference
```

#### **When to Care About Performance**

```
Builder performance matters:
├─ Creating millions of objects? → YES, care about perf
├─ Real-time/embedded system? → YES, care about perf
├─ Game engine hot path? → YES, care about perf
├─ Web API endpoint? → NO, readability > tiny perf
├─ Desktop app? → NO, readability > tiny perf
└─ Startup/initialization? → NO, runs once anyway
```

**Decision Framework:**

```csharp
// ✅ USE Builder: Readability > tiny performance cost
public IActionResult CreateOrder(OrderRequest request)
{
    var order = new OrderBuilder()
        .WithCustomer(request.CustomerId)
        .WithItems(request.Items)
        .WithShipping(request.ShippingAddress)
        .Build();

    return Ok(order);
}

// ❌ DON'T use Builder: Performance-critical path
public void RenderFrame() // Called 60 FPS
{
    for (int i = 0; i < 10000; i++)
    {
        // Use direct constructor - no allocations
        var particle = new Particle(x, y, velocity);
    }
}
```

**Tóm tắt:**

- Builder Pattern có **slight overhead** (~3x slower, 2x memory)
- Trong **99% use cases**, overhead này **negligible**
- **Readability và maintainability** quan trọng hơn
- Chỉ care về performance ở **high-frequency, tight loops, real-time systems**
- **Profile first** trước khi optimize!

**"Premature optimization is the root of all evil" - Donald Knuth**

---

### 5. **Sự khác biệt giữa Builder và Fluent Interface?**

**Câu trả lời:**

Đây là câu hỏi hay vì nhiều người nghĩ Builder và Fluent Interface là một! Thực tế chúng **khác nhau** nhưng thường **kết hợp** với nhau.

#### **Fluent Interface là gì?**

**Fluent Interface** là một **coding style/pattern** cho phép method chaining để code đọc như natural language.

**Key characteristic:** Method returns `this` (hoặc another object) để chain tiếp

```csharp
// Fluent Interface - NOT a Builder!
public class StringBuilder
{
    private string _value = "";

    public StringBuilder Append(string text)
    {
        _value += text;
        return this; // ← Returns 'this' for chaining
    }

    public StringBuilder AppendLine(string text)
    {
        _value += text + "\n";
        return this; // ← Fluent chaining
    }

    public override string ToString() => _value;
}

// Fluent usage
var result = new StringBuilder()
    .Append("Hello")
    .Append(" ")
    .Append("World")
    .ToString();
```

#### **Builder Pattern là gì?**

**Builder Pattern** là một **creational design pattern** giúp construct complex objects step-by-step.

**Key characteristic:** Tách object construction khỏi representation

```csharp
// Builder Pattern - constructs complex object
public class UserBuilder
{
    private string _name;
    private string _email;

    public UserBuilder WithName(string name)
    {
        _name = name;
        return this; // ← Happens to be fluent!
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public User Build() // ← Creates final object
    {
        return new User(_name, _email);
    }
}

// Builder usage
var user = new UserBuilder()
    .WithName("John")
    .WithEmail("john@email.com")
    .Build(); // ← Returns final product
```

#### **So sánh trực tiếp**

| Aspect               | Fluent Interface                 | Builder Pattern                        |
| -------------------- | -------------------------------- | -------------------------------------- |
| **What is it?**      | Coding style/API design          | Creational design pattern              |
| **Purpose**          | Readable, chainable API          | Construct complex objects step-by-step |
| **Returns**          | `this` or another fluent object  | Builder (then final object in Build()) |
| **Final Method**     | Any method (ToString, Execute)   | Build() method                         |
| **Object Creation**  | May or may not create new object | Specifically creates new object        |
| **State**            | May modify existing object       | Accumulates state, creates new object  |
| **Pattern Category** | API Design Pattern               | Creational Design Pattern              |

#### **Key Differences với Examples**

**1. Fluent Interface WITHOUT Builder Pattern**

```csharp
// Fluent API modifying existing object - NOT a builder
public class Calculator
{
    private int _value = 0;

    public Calculator Add(int x)
    {
        _value += x;
        return this; // ← Fluent
    }

    public Calculator Multiply(int x)
    {
        _value *= x;
        return this; // ← Fluent
    }

    public int Result() => _value;
}

// Usage - Fluent but NOT Builder!
var result = new Calculator()
    .Add(5)
    .Multiply(2)
    .Add(3)
    .Result(); // = 13

// This is Fluent Interface, NOT Builder Pattern
// Why? Because we're modifying ONE object, not building a new one
```

**2. Builder Pattern WITHOUT Fluent Interface**

```csharp
// Builder without method chaining - NOT fluent
public class UserBuilder
{
    private string _name;
    private string _email;

    public void SetName(string name) // ← void return, not fluent!
    {
        _name = name;
    }

    public void SetEmail(string email) // ← void return
    {
        _email = email;
    }

    public User Build()
    {
        return new User(_name, _email);
    }
}

// Usage - Builder but NOT Fluent!
var builder = new UserBuilder();
builder.SetName("John");      // ← No chaining
builder.SetEmail("john@email.com");
var user = builder.Build();

// This IS Builder Pattern, but NOT Fluent Interface
// Why? Because it builds object step-by-step but doesn't chain
```

**3. Builder Pattern WITH Fluent Interface** ⭐ (Most Common)

```csharp
// Combining both - this is what most people use!
public class UserBuilder
{
    private string _name;
    private string _email;

    public UserBuilder WithName(string name) // ← Fluent!
    {
        _name = name;
        return this; // ← Returns this for chaining
    }

    public UserBuilder WithEmail(string email) // ← Fluent!
    {
        _email = email;
        return this;
    }

    public User Build() // ← Builder pattern!
    {
        return new User(_name, _email); // ← Creates new object
    }
}

// Usage - Both Builder AND Fluent!
var user = new UserBuilder()
    .WithName("John")        // ← Fluent chaining
    .WithEmail("john@email.com")
    .Build();                 // ← Builder pattern

// This combines BOTH patterns for best experience!
```

#### **Real-World Examples**

**Example 1: StringBuilder (Fluent but NOT Builder)**

```csharp
// C#'s StringBuilder is Fluent Interface, NOT Builder Pattern
var sb = new StringBuilder(); // ← Object already exists

sb.Append("Hello")      // ← Modifying existing object
  .Append(" ")
  .Append("World")
  .ToString();          // ← Returns string, not new StringBuilder

// Why NOT Builder?
// - Doesn't create a new object
// - Modifies existing StringBuilder
// - ToString() doesn't return StringBuilder
```

**Example 2: LINQ (Fluent but NOT Builder)**

```csharp
// LINQ is Fluent Interface, NOT Builder Pattern
var result = numbers
    .Where(x => x > 5)      // ← Fluent chaining
    .Select(x => x * 2)
    .OrderBy(x => x)
    .ToList();              // ← Final method

// Why NOT Builder?
// - Transforms data, doesn't build complex object
// - Each method returns IEnumerable, not accumulating state for ONE object
```

**Example 3: HttpRequestBuilder (Both Builder AND Fluent)**

```csharp
// This is BOTH Builder Pattern AND Fluent Interface
public class HttpRequestBuilder
{
    private string _url;
    private string _method = "GET";
    private Dictionary<string, string> _headers = new();

    public HttpRequestBuilder WithUrl(string url)
    {
        _url = url;
        return this; // ← Fluent
    }

    public HttpRequestBuilder WithMethod(string method)
    {
        _method = method;
        return this; // ← Fluent
    }

    public HttpRequestBuilder AddHeader(string key, string value)
    {
        _headers[key] = value;
        return this; // ← Fluent
    }

    public HttpRequest Build() // ← Builder
    {
        return new HttpRequest  // ← Creates NEW object
        {
            Url = _url,
            Method = _method,
            Headers = _headers
        };
    }
}

// Why BOTH?
// - Builder: Constructs complex HttpRequest step-by-step
// - Fluent: Methods return 'this' for readable chaining
```

#### **How to Identify**

**Is it Fluent Interface?**

```
Does method return 'this' or another object for chaining?
├─ YES → Fluent Interface ✅
└─ NO → Not Fluent ❌
```

**Is it Builder Pattern?**

```
Does it construct a complex object step-by-step?
├─ Has it got a Build() method that creates new object?
│   ├─ YES → Builder Pattern ✅
│   └─ NO → Check: Does it accumulate state to create object?
│       ├─ YES → Builder Pattern ✅
│       └─ NO → Not Builder ❌
└─ Does it just modify existing object?
    └─ YES → NOT Builder (just Fluent) ❌
```

#### **Practical Comparison**

```csharp
// ❌ Neither Fluent nor Builder
public class User
{
    public void SetName(string name) { }
    public void SetEmail(string email) { }
}

var user = new User();
user.SetName("John");
user.SetEmail("john@email.com");

// ✅ Fluent only (not Builder)
public class User
{
    public User SetName(string name) { return this; }
    public User SetEmail(string email) { return this; }
}

var user = new User()
    .SetName("John")
    .SetEmail("john@email.com"); // Modifying existing object

// ✅ Builder only (not Fluent)
public class UserBuilder
{
    public void SetName(string name) { }
    public void SetEmail(string email) { }
    public User Build() { return new User(); }
}

var builder = new UserBuilder();
builder.SetName("John");
builder.SetEmail("john@email.com");
var user = builder.Build();

// ✅✅ Both Fluent AND Builder (BEST!)
public class UserBuilder
{
    public UserBuilder WithName(string name) { return this; }
    public UserBuilder WithEmail(string email) { return this; }
    public User Build() { return new User(); }
}

var user = new UserBuilder()
    .WithName("John")
    .WithEmail("john@email.com")
    .Build();
```

#### **Summary Table**

| Pattern      | Returns This? | Creates New Object? | Example                         |
| ------------ | ------------- | ------------------- | ------------------------------- |
| Neither      | ❌            | ❌                  | Traditional setters             |
| Fluent only  | ✅            | ❌                  | StringBuilder, LINQ             |
| Builder only | ❌            | ✅                  | Builder with void setters       |
| **Both** ⭐  | ✅            | ✅                  | HttpRequestBuilder, UserBuilder |

**Tóm tắt:**

- **Fluent Interface** = Method chaining style (returns `this`)
- **Builder Pattern** = Creational pattern (constructs complex object)
- **Thường kết hợp cả hai** để có readable API + powerful object creation
- **Builder can be Fluent**, but **Fluent không nhất thiết là Builder**!

**Analogy:**

- **Fluent Interface** = "How you speak" (coding style)
- **Builder Pattern** = "What you're building" (design pattern)
- You can speak fluently while building something (Builder + Fluent) ✅
- You can speak fluently without building anything (just Fluent) ✅
- You can build something without speaking fluently (just Builder) ✅
