# Code Structure / Cấu Trúc Mã

## 📖 Tổng Quan / Overview

Một cấu trúc mã tốt làm cho code dễ đọc, dễ bảo trì, và dễ test. Nó tuân theo các nguyên tắc SOLID và clean code.

Good code structure makes code readable, maintainable, and testable. It follows SOLID principles and clean code practices.

---

## 🏗️ Class Structure / Cấu Trúc Lớp

### Recommended Order / Thứ Tự Được Khuyến Nghị

```csharp
public class UserService
{
    // 1. Constants / Hằng số
    private const int MAX_RETRIES = 3;

    // 2. Static Fields / Fields Tĩnh
    private static ILogger _staticLogger;

    // 3. Fields / Các Field
    private readonly IRepository _repository;
    private readonly IValidator _validator;
    private ILogger _logger;

    // 4. Constructor / Hàm Tạo
    public UserService(IRepository repository, IValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    // 5. Properties / Thuộc Tính
    public string ServiceName { get; } = "UserService";
    public int MaxRetries { get; private set; } = MAX_RETRIES;

    // 6. Public Methods / Phương Thức Công Khai
    public async Task<User> GetUserAsync(int userId)
    {
        return await _repository.GetByIdAsync(userId);
    }

    public void UpdateUser(User user)
    {
        _validator.Validate(user);
        _repository.Update(user);
    }

    // 7. Private Methods / Phương Thức Riêng Tư
    private void LogOperation(string operation)
    {
        _logger?.Log($"Operation: {operation}");
    }

    // 8. Static Methods / Phương Thức Tĩnh
    public static void SetLogger(ILogger logger)
    {
        _staticLogger = logger;
    }
}
```

---

## 📋 Class Organization / Tổ Chức Lớp Chi Tiết

### Example / Ví Dụ Đầy Đủ

```csharp
/// <summary>
/// Manages user-related operations
/// </summary>
public class UserService : IUserService
{
    // ============ CONSTANTS & STATICS ============
    private const string USER_NOT_FOUND = "User not found";
    private static readonly ILogger Logger = LoggerFactory.Create();

    // ============ FIELDS ============
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    // ============ CONSTRUCTORS ============
    /// <summary>
    /// Initializes a new instance of UserService
    /// </summary>
    public UserService(
        IUserRepository userRepository,
        IEmailService emailService,
        IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    // ============ PROPERTIES ============
    public string ServiceVersion => "1.0.0";

    // ============ PUBLIC METHODS ============
    /// <summary>
    /// Gets a user by ID
    /// </summary>
    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        Logger.Log($"Getting user {userId}");

        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException(USER_NOT_FOUND);

        return _mapper.Map<UserDto>(user);
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
    {
        ValidateUserRequest(request);

        var user = _mapper.Map<User>(request);
        var createdUser = await _userRepository.AddAsync(user);

        await SendWelcomeEmailAsync(createdUser);

        return _mapper.Map<UserDto>(createdUser);
    }

    public async Task UpdateUserAsync(int userId, UpdateUserRequest request)
    {
        ValidateUserRequest(request);

        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException(USER_NOT_FOUND);

        _mapper.Map(request, user);
        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException(USER_NOT_FOUND);

        await _userRepository.DeleteAsync(user);
    }

    // ============ PRIVATE METHODS ============
    private void ValidateUserRequest(object request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
    }

    private async Task SendWelcomeEmailAsync(User user)
    {
        try
        {
            await _emailService.SendWelcomeEmailAsync(user.Email);
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to send welcome email: {ex.Message}");
        }
    }
}
```

---

## 📁 Project Structure / Cấu Trúc Dự Án

### Typical Layered Architecture

```
MyProject/
├── Models/
│   ├── User.cs
│   ├── Order.cs
│   └── Product.cs
│
├── DTOs/
│   ├── UserDto.cs
│   ├── CreateUserRequest.cs
│   └── UpdateUserRequest.cs
│
├── Services/
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── IOrderService.cs
│   └── OrderService.cs
│
├── Repositories/
│   ├── IRepository.cs
│   ├── IUserRepository.cs
│   ├── UserRepository.cs
│   └── OrderRepository.cs
│
├── Controllers/
│   ├── UserController.cs
│   └── OrderController.cs
│
├── Interfaces/
│   ├── IValidator.cs
│   ├── ILogger.cs
│   └── IMapper.cs
│
├── Implementations/
│   ├── Validators/
│   │   └── UserValidator.cs
│   ├── Loggers/
│   │   └── FileLogger.cs
│   └── Mappers/
│       └── AutoMapperProfile.cs
│
├── Exceptions/
│   ├── UserNotFoundException.cs
│   └── ValidationException.cs
│
├── Extensions/
│   ├── ServiceCollectionExtensions.cs
│   └── StringExtensions.cs
│
└── Configuration/
    ├── AppSettings.json
    └── DatabaseConfiguration.cs
```

---

## ✅ Best Practices / Thực Hành Tốt

### 1. **Single Responsibility** / Trách Nhiệm Đơn

```csharp
// ❌ BAD - Class làm quá nhiều việc
public class UserService
{
    public void CreateUser() { }
    public void SendEmail() { }
    public void LogData() { }
    public void SaveToDatabase() { }
}

// ✅ GOOD - Mỗi class có trách nhiệm cụ thể
public class UserService { public void CreateUser() { } }
public class EmailService { public void SendEmail() { } }
public class Logger { public void LogData() { } }
public class Repository { public void SaveToDatabase() { } }
```

### 2. **Dependency Injection** / Tiêm Phụ Thuộc

```csharp
// ❌ BAD - Tightly coupled
public class UserService
{
    private Logger _logger = new Logger();
}

// ✅ GOOD - Loosely coupled
public class UserService
{
    private readonly ILogger _logger;

    public UserService(ILogger logger)
    {
        _logger = logger;
    }
}
```

### 3. **Use Interfaces** / Dùng Giao Diện

```csharp
// ✅ Good practice
public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task AddAsync(User user);
}

public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }
}
```

### 4. **Separate Concerns** / Tách Mối Quan Tâm

```csharp
// ✅ GOOD - Separate layers
// Models/User.cs
public class User { }

// DTOs/UserDto.cs
public class UserDto { }

// Services/UserService.cs
public class UserService { }

// Controllers/UserController.cs
public class UserController { }
```

### 5. **Keep Methods Small** / Giữ Phương Thức Nhỏ

```csharp
// ❌ BAD - Method quá dài
public void ProcessOrder(Order order)
{
    // 50 dòng code
}

// ✅ GOOD - Split vào methods nhỏ
public async Task ProcessOrderAsync(Order order)
{
    ValidateOrder(order);
    await UpdateInventoryAsync(order);
    await SendConfirmationEmailAsync(order);
}
```

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Không có tổ chức
public class UserService
{
    public void DeleteUser() { }
    private void LogData() { }
    private const string NAME = "UserService";
    private IRepository _repo;
    public void CreateUser() { }
    public static void Test() { }
}

// ✅ CORRECT - Có tổ chức rõ ràng
public class UserService
{
    private const string NAME = "UserService";
    private IRepository _repo;

    public UserService(IRepository repo) => _repo = repo;

    public void CreateUser() { }
    public void DeleteUser() { }

    private void LogData() { }
}
```

---

## 🎓 Interview Questions

1. **Tại sao code structure lại quan trọng?**
2. **Một class nên có bao nhiêu trách nhiệm?**
3. **Tại sao nên dùng dependency injection?**
4. **Enum vs Constants, khi nào dùng?**
5. **Method nên dài bao nhiêu?**

---

## 📚 Related Topics

- [Naming Conventions](naming-conventions.md)
- [SOLID Principles](../solid-principles/)
- [Design Patterns](../design-patterns/)
