# Delegates / Ủy Quyền

## 📖 Định Nghĩa / Definition

**Delegate** là một **type-safe function pointer** hoặc **reference type** đại diện cho một method có signature cụ thể.

**Delegate** is a **type-safe function pointer** or **reference type** that represents a method with a specific signature.

---

## 💡 Khái Niệm / Concept

Delegate cho phép bạn:

- ✅ Truyền methods như parameters
- ✅ Lưu trữ references đến methods
- ✅ Call methods indirectly
- ✅ Implement callbacks

---

## 📝 Syntax & Examples

### 1. **Define a Delegate**

```csharp
// Định nghĩa delegate
public delegate void MyDelegate(string message);
public delegate int MathDelegate(int a, int b);
public delegate string TextDelegate(string input);
```

### 2. **Using Delegate**

```csharp
public class DelegateExample
{
    // Định nghĩa delegate
    public delegate void Greet(string name);

    // Method matching delegate signature
    public void SayHello(string name)
    {
        Console.WriteLine($"Hello, {name}!");
    }

    public void SayGoodbye(string name)
    {
        Console.WriteLine($"Goodbye, {name}!");
    }

    public void Demo()
    {
        // Gán method vào delegate
        Greet greeting = SayHello;
        greeting("John");  // Output: Hello, John!

        // Gán method khác
        greeting = SayGoodbye;
        greeting("John");  // Output: Goodbye, John!
    }
}
```

### 3. **Multicast Delegate** (Chain Multiple Methods)

```csharp
public class MulticastExample
{
    public delegate void NotifyDelegate(string message);

    public void Display(string message) => Console.WriteLine($"Display: {message}");
    public void Log(string message) => Console.WriteLine($"Log: {message}");
    public void Email(string message) => Console.WriteLine($"Email: {message}");

    public void Demo()
    {
        NotifyDelegate notify = Display;
        notify += Log;
        notify += Email;

        notify("System update");
        // Output:
        // Display: System update
        // Log: System update
        // Email: System update
    }
}
```

---

## 🎯 Built-in Delegates

### **Action** - Void Return

```csharp
// Action: không return
Action<string> printMessage = Console.WriteLine;
printMessage("Hello");

Action<int, int> add = (a, b) => Console.WriteLine(a + b);
add(5, 3);
```

### **Func** - Return Value

```csharp
// Func: có return value (parameter cuối là return type)
Func<int, int, int> multiply = (a, b) => a * b;
int result = multiply(5, 3);

Func<string, int> getLength = str => str.Length;
Console.WriteLine(getLength("Hello"));  // 5
```

### **Predicate** - Return Boolean

```csharp
Predicate<int> isEven = n => n % 2 == 0;
Console.WriteLine(isEven(4));   // true
Console.WriteLine(isEven(5));   // false

List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
List<int> evens = numbers.FindAll(isEven);
```

---

## 📝 Ví Dụ Thực Tế / Real-World Example

### Calculator with Delegate

```csharp
public class Calculator
{
    // Định nghĩa delegate
    public delegate int Operation(int a, int b);

    // Methods matching delegate
    public int Add(int a, int b) => a + b;
    public int Subtract(int a, int b) => a - b;
    public int Multiply(int a, int b) => a * b;
    public int Divide(int a, int b) => b != 0 ? a / b : 0;

    // Method sử dụng delegate
    public void ExecuteOperation(int x, int y, Operation operation)
    {
        int result = operation(x, y);
        Console.WriteLine($"Result: {result}");
    }
}

// Usage
var calc = new Calculator();
Calculator.Operation op = calc.Add;
calc.ExecuteOperation(10, 5, op);  // Result: 15

// With lambda
calc.ExecuteOperation(10, 5, (a, b) => a * b);  // Result: 50
```

### Callback Pattern

```csharp
public class FileProcessor
{
    public delegate void ProcessComplete(string result);

    public void ReadFile(string filePath, ProcessComplete callback)
    {
        try
        {
            string content = File.ReadAllText(filePath);
            callback?.Invoke($"File read successfully: {content}");
        }
        catch (Exception ex)
        {
            callback?.Invoke($"Error: {ex.Message}");
        }
    }
}

// Usage
var processor = new FileProcessor();
processor.ReadFile("data.txt", result => Console.WriteLine(result));
```

---

## ✅ Delegate vs Action vs Func / So Sánh

| Loại          | Return Type    | Use Case                            |
| ------------- | -------------- | ----------------------------------- |
| **Delegate**  | Any            | Custom delegates, type safety       |
| **Action**    | void           | Void operations (Action, Action<T>) |
| **Func**      | Always returns | Return value (Func<in,in,out>)      |
| **Predicate** | bool           | Conditions (Predicate<T>)           |

---

## ✅ Best Practices

1. **Use Action/Func** thay vì custom delegates khi có thể
2. **Null check** khi invoke: `callback?.Invoke()`
3. **Unsubscribe** từ events để tránh memory leaks
4. **Use lambda** cho simple operations
5. **Document expected behavior** rõ ràng

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - NullReferenceException
public class Notifier
{
    public delegate void Notify(string message);

    public void SendNotification(Notify callback, string message)
    {
        callback(message);  // Error if callback is null!
    }
}

// ✅ CORRECT
public class Notifier
{
    public delegate void Notify(string message);

    public void SendNotification(Notify callback, string message)
    {
        callback?.Invoke(message);  // Safe: check null
    }
}
```

---

## 🎓 Interview Questions

1. **Delegate là gì? Tại sao dùng?**
2. **Delegate vs Interface khác gì?**
3. **Action vs Func khác gì?**
4. **Multicast delegate là gì?**
5. **Làm sao unsubscribe từ delegate?**

---

## 📚 Related Topics

- [Events](events.md)
- [Action & Func](action-func.md)
- [Delegates & Callbacks Pattern](delegates.md)
