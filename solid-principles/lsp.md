# Liskov Substitution Principle (LSP)

## 📖 Định Nghĩa / Definition

**LSP:** Objects of a superclass should be replaceable with objects of a subclass **without breaking correctness**.

**Nói gọn:** Subclass phải **hành xử như** base class hứa hẹn (behavioral contract), không phá vỡ expectation của client.

---

## 💡 Dấu Hiệu Vi Phạm / Smells

- Subclass **throw NotImplementedException** cho method mà base yêu cầu.
- Subclass **thay đổi meaning**: nhận input hợp lệ nhưng reject, return khác expectation.
- Subclass **thêm precondition mạnh hơn** (đòi hỏi điều kiện nghiêm ngặt hơn base).
- Subclass **giảm postcondition** (đảm bảo ít hơn base hứa).

---

## 📝 Ví Dụ Vi Phạm

```csharp
public abstract class Bird
{
    public abstract void Fly();
}

public class Sparrow : Bird
{
    public override void Fly() => Console.WriteLine("Sparrow flying");
}

// ❌ Vi phạm LSP: Penguin không bay được nhưng bị buộc phải Fly
public class Penguin : Bird
{
    public override void Fly()
    {
        throw new NotSupportedException("Penguins can't fly");
    }
}

// Client code
public void MakeItFly(Bird bird) => bird.Fly(); // Ném exception với Penguin
```

### Cách Sửa (Tách Interface)

```csharp
public interface IBird { }
public interface IFlyingBird : IBird
{
    void Fly();
}

public class Sparrow : IFlyingBird
{
    public void Fly() => Console.WriteLine("Sparrow flying");
}

public class Penguin : IBird { }

public void MakeItFly(IFlyingBird bird) => bird.Fly(); // Không còn Penguin ở đây
```

---

## ✅ Ví Dụ Đúng

```csharp
public abstract class PaymentMethod
{
    public abstract void Pay(decimal amount);
}

public class CreditCard : PaymentMethod
{
    public override void Pay(decimal amount)
    {
        Console.WriteLine($"Paying ${amount} with credit card");
    }
}

public class PayPal : PaymentMethod
{
    public override void Pay(decimal amount)
    {
        Console.WriteLine($"Paying ${amount} with PayPal");
    }
}

// Client dùng base type, mọi subclass hoạt động đúng
public void Checkout(PaymentMethod method, decimal total) => method.Pay(total);
```

---

## 🎯 Nguyên Tắc Kiểm Tra LSP

- **Không strengthen preconditions:** Subclass không đòi hỏi điều kiện khó hơn base.
- **Không weaken postconditions:** Subclass phải đảm bảo ít nhất những gì base hứa.
- **Preserve invariants:** Bất biến của base phải còn đúng ở subclass.
- **Same semantics:** Kết quả/behavior không gây bất ngờ cho client dùng base type.

---

## 🔧 Checklist Nhanh

- Có method nào `throw NotImplementedException`? → Tách interface/abstract khác.
- Có thay đổi domain logic (ví dụ base nhận mọi số, subclass chỉ nhận số dương)? → Xem lại contract.
- Có subclass thêm side-effect phá vỡ expectation? → Refactor.

---

## 🎓 Interview Q&A

- **Hỏi:** LSP là gì?  
  **Đáp:** Subclass phải thay thế được cho base mà không đổi behavior mong đợi.
- **Hỏi:** Dấu hiệu vi phạm?  
  **Đáp:** `NotImplementedException`, thay đổi pre/postcondition, behavior bất ngờ.
- **Hỏi:** Giải pháp?  
  **Đáp:** Tách interface, dùng composition, điều chỉnh contract.

---

## 📚 Related

- [ISP](isp.md)
- [OCP](ocp.md)
