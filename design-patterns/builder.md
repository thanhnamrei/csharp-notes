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

1. **Builder pattern là gì? Tại sao cần nó?**
2. **Builder vs Constructor? Khi nào dùng Builder?**
3. **Cách validate trong Builder?**
4. **Builder pattern có ảnh hưởng hiệu năng không?**
5. **Sự khác biệt giữa Builder và Fluent Interface?**
