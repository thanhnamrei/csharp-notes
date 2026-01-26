# Interface Segregation Principle (ISP)

## 📖 Định Nghĩa / Definition

**ISP:** Clients **không nên bị ép** phụ thuộc vào những interface mà họ **không dùng**.

**Nói gọn:** Thà có nhiều interface nhỏ, cụ thể, còn hơn một interface God khổng lồ.

---

## 💡 Dấu Hiệu Vi Phạm / Smells

- Interface có quá nhiều method (God interface).
- Class implement interface nhưng nhiều method `throw NotImplementedException` hoặc để trống.
- Mỗi client chỉ dùng vài method trong interface to.

---

## 📝 Ví Dụ Vi Phạm

```csharp
public interface IPrinter
{
    void Print(string content);
    void Scan(string content);
    void Fax(string content);
}

// ❌ SimplePrinter không cần Scan, Fax nhưng bị ép implement
public class SimplePrinter : IPrinter
{
    public void Print(string content) => Console.WriteLine(content);
    public void Scan(string content) => throw new NotImplementedException();
    public void Fax(string content) => throw new NotImplementedException();
}
```

### Cách Sửa (Tách Nhỏ Interface)

```csharp
public interface IPrint
{
    void Print(string content);
}

public interface IScan
{
    void Scan(string content);
}

public interface IFax
{
    void Fax(string content);
}

public class SimplePrinter : IPrint
{
    public void Print(string content) => Console.WriteLine(content);
}

public class MultiFunctionPrinter : IPrint, IScan, IFax
{
    public void Print(string content) => Console.WriteLine(content);
    public void Scan(string content) => Console.WriteLine($"Scanning {content}");
    public void Fax(string content) => Console.WriteLine($"Faxing {content}");
}
```

---

## ✅ Best Practices

- Thiết kế interface **theo vai trò** (role interface), không theo thiết bị tổng hợp.
- Ưu tiên **nhỏ, cohesive**; tránh "one-size-fits-all".
- Dùng **composition**: interface lớn = hợp của nhiều interface nhỏ.
- Với public API, giữ interface tối giản, ổn định.

---

## 🔧 Checklist Nhanh

- Interface > ~5-7 methods? Xem xét tách.
- Class implement nhưng ném `NotImplementedException`? Vi phạm ISP.
- Client chỉ gọi 1-2 method? Tạo interface riêng cho client.

---

## 🎓 Interview Q&A

- **Hỏi:** ISP là gì?  
  **Đáp:** Không ép client phụ thuộc vào method họ không dùng; tách interface nhỏ, chuyên biệt.
- **Hỏi:** Dấu hiệu vi phạm?  
  **Đáp:** God interface, `NotImplementedException` trong implementer.
- **Hỏi:** Cách khắc phục?  
  **Đáp:** Tách interface, dùng composition, thiết kế theo vai trò.

---

## 📚 Related

- [LSP](lsp.md)
- [SRP](srp.md)
