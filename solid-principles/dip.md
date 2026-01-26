# Dependency Inversion Principle (DIP)

## 📖 Định Nghĩa / Definition

**DIP:**

1. High-level modules **không** phụ thuộc trực tiếp low-level modules; **cả hai** phụ thuộc vào **abstractions**.
2. Abstractions **không** phụ thuộc chi tiết; chi tiết phụ thuộc abstractions.

**Nói gọn:** Code vào interface/abstraction, không phụ thuộc implementation cụ thể.

---

## 💡 Vấn Đề Khi Vi Phạm

- Class cấp cao (business) new trực tiếp class cấp thấp (infrastructure).
- Khó test (không mock được), khó thay thế implementation (DB khác, logger khác).
- Thay đổi chi tiết kéo theo đổi logic cao hơn.

---

## 📝 Ví Dụ Vi Phạm

```csharp
// ❌ High-level phụ thuộc trực tiếp SqlUserRepository
public class UserService
{
    private readonly SqlUserRepository _repo = new SqlUserRepository();

    public User Get(int id) => _repo.GetById(id);
}
```

### Cách Sửa (Dùng Abstraction + DI)

```csharp
public interface IUserRepository
{
    User GetById(int id);
}

public class SqlUserRepository : IUserRepository
{
    public User GetById(int id) => new User { Id = id }; // Stub
}

public class InMemoryUserRepository : IUserRepository
{
    public User GetById(int id) => new User { Id = id, Name = "Test" };
}

public class UserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) => _repo = repo;

    public User Get(int id) => _repo.GetById(id);
}

// Composition Root / DI Container
services.AddScoped<IUserRepository, SqlUserRepository>();
services.AddScoped<UserService>();
```

---

## ✅ Best Practices

- **Code to interfaces/abstractions**, không code vào concrete.
- Đặt **composition root** (DI container) để wire dependencies.
- Interface nên thuộc layer cấp cao (domain), implementation ở layer thấp (infra).
- Hạn chế `new` trong business logic; inject thay vì tự tạo.

---

## 🔧 Checklist Nhanh

- Có `new` trong service/domain? → Xem lại, chuyển sang inject.
- Thay DB/logging cần sửa business code? → Vi phạm DIP.
- Không test được vì phụ thuộc concrete? → Thêm interface, inject mock.

---

## 🎓 Interview Q&A

- **Hỏi:** DIP là gì?  
  **Đáp:** High-level và low-level đều phụ thuộc abstraction; abstraction không phụ thuộc chi tiết.
- **Hỏi:** Lợi ích?  
  **Đáp:** Dễ test, dễ thay thế implementation, giảm coupling, tuân OCP.
- **Hỏi:** Áp dụng thực tế?  
  **Đáp:** Dùng interfaces + DI container; tách interface ở domain, implement ở infrastructure.

---

## 📚 Related

- [OCP](ocp.md)
- [ISP](isp.md)
- [SRP](srp.md)
