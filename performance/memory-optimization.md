# 🚀 Memory Optimization / Tối Ưu Bộ Nhớ

## 📋 Tổng Quan / Overview

Memory optimization trong C# giúp giảm memory footprint và cải thiện hiệu năng ứng dụng.

Memory optimization in C# helps reduce memory footprint and improve application performance.

---

## 1. Value Types vs Reference Types

### Value Types (Stack)

```csharp
// Struct - stored on stack
public struct Point
{
    public int X;
    public int Y;
}

Point p1 = new Point { X = 10, Y = 20 }; // Stack allocation
```

### Reference Types (Heap)

```csharp
// Class - stored on heap
public class Rectangle
{
    public int Width { get; set; }
    public int Height { get; set; }
}

Rectangle rect = new Rectangle(); // Heap allocation + GC overhead
```

**💡 Lời Khuyên / Tips:**

- Dùng `struct` cho objects nhỏ (< 16 bytes)
- Dùng `class` cho objects lớn hoặc cần inheritance

---

## 2. String Optimization

### ❌ BAD: String Concatenation

```csharp
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += i.ToString(); // Creates 1000 new string objects!
}
```

### ✅ GOOD: StringBuilder

```csharp
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append(i);
}
string result = sb.ToString();
```

### String Interning

```csharp
// String interning to save memory
string s1 = "hello";
string s2 = "hello";
bool same = Object.ReferenceEquals(s1, s2); // true - same reference

// Manual interning
string s3 = new string("hello".ToCharArray());
string s4 = string.Intern(s3);
```

---

## 3. Collection Optimization

### Pre-allocate Capacity

```csharp
// ❌ BAD: Multiple resizing operations
var list = new List<int>();
for (int i = 0; i < 10000; i++)
{
    list.Add(i); // May trigger multiple array resizing
}

// ✅ GOOD: Pre-allocated capacity
var list = new List<int>(10000);
for (int i = 0; i < 10000; i++)
{
    list.Add(i); // No resizing needed
}
```

### Choose Right Collection

```csharp
// HashSet for unique items and fast lookup - O(1)
var uniqueIds = new HashSet<int>();

// Dictionary for key-value pairs - O(1) lookup
var cache = new Dictionary<string, object>();

// List for sequential access
var items = new List<string>();
```

---

## 4. Object Pooling

### Before: Creating Many Objects

```csharp
// ❌ Creates many objects, lots of GC pressure
public void ProcessRequests()
{
    for (int i = 0; i < 1000; i++)
    {
        var buffer = new byte[4096];
        // Process...
        // buffer becomes garbage
    }
}
```

### After: Object Pooling

```csharp
using System.Buffers;

// ✅ Reuse buffers from pool
public void ProcessRequests()
{
    var pool = ArrayPool<byte>.Shared;

    for (int i = 0; i < 1000; i++)
    {
        byte[] buffer = pool.Rent(4096);
        try
        {
            // Process...
        }
        finally
        {
            pool.Return(buffer);
        }
    }
}
```

---

## 5. Span<T> and Memory<T>

### Traditional Array Slicing (Creates Copy)

```csharp
// ❌ Creates new array
int[] numbers = { 1, 2, 3, 4, 5 };
int[] subset = numbers.Skip(1).Take(3).ToArray();
```

### Using Span<T> (Zero Allocation)

```csharp
// ✅ No allocation, just a view
int[] numbers = { 1, 2, 3, 4, 5 };
Span<int> subset = numbers.AsSpan(1, 3);

// Read-only span
ReadOnlySpan<char> text = "Hello World".AsSpan();
ReadOnlySpan<char> hello = text.Slice(0, 5);
```

---

## 6. Avoiding Boxing

### ❌ BAD: Boxing Value Types

```csharp
int number = 42;
object obj = number;  // Boxing - allocates on heap
int value = (int)obj; // Unboxing
```

### ✅ GOOD: Use Generics

```csharp
// Generic method - no boxing
public T Max<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) > 0 ? a : b;
}

int result = Max(10, 20); // No boxing!
```

---

## 7. Lazy Initialization

### Defer Expensive Object Creation

```csharp
public class ExpensiveResource
{
    // Only created when first accessed
    private Lazy<HeavyObject> _resource = new Lazy<HeavyObject>(() =>
    {
        return new HeavyObject();
    });

    public HeavyObject Resource => _resource.Value;
}
```

---

## 8. Dispose Pattern

### Proper Resource Cleanup

```csharp
public class ResourceManager : IDisposable
{
    private FileStream _fileStream;
    private bool _disposed = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
                _fileStream?.Dispose();
            }

            // Free unmanaged resources
            _disposed = true;
        }
    }

    ~ResourceManager()
    {
        Dispose(false);
    }
}
```

---

## 9. Memory Profiling Tools

### Using Diagnostic Tools

```csharp
using System.Diagnostics;

// Monitor memory usage
long memoryBefore = GC.GetTotalMemory(false);

// Your code here...

long memoryAfter = GC.GetTotalMemory(false);
Console.WriteLine($"Memory used: {memoryAfter - memoryBefore} bytes");

// Generation statistics
Console.WriteLine($"Gen 0 collections: {GC.CollectionCount(0)}");
Console.WriteLine($"Gen 1 collections: {GC.CollectionCount(1)}");
Console.WriteLine($"Gen 2 collections: {GC.CollectionCount(2)}");
```

---

## 10. Struct Best Practices

### When to Use Struct

```csharp
// ✅ GOOD: Small, immutable data
public readonly struct Color
{
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;

    public Color(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
}

// ❌ BAD: Large mutable struct
public struct LargeStruct // Avoid!
{
    public int Field1;
    public int Field2;
    // ... many more fields
}
```

---

## 📊 Performance Comparison

| Technique                 | Memory Impact   | Performance Gain |
| ------------------------- | --------------- | ---------------- |
| StringBuilder vs String + | 90% reduction   | 10-100x faster   |
| Object Pooling            | 70% reduction   | 5-10x faster     |
| Span<T>                   | Zero allocation | 2-5x faster      |
| Struct vs Class           | 50% reduction   | 1.5-3x faster    |
| Pre-allocated Collections | 30% reduction   | 2x faster        |

---

## ✅ Best Practices Checklist

- [ ] Sử dụng `StringBuilder` cho string concatenation nhiều lần
- [ ] Pre-allocate collection capacity khi biết kích thước
- [ ] Dùng `Span<T>` và `Memory<T>` để tránh allocation
- [ ] Implement object pooling cho objects được tạo thường xuyên
- [ ] Tránh boxing/unboxing bằng cách dùng generics
- [ ] Dùng `struct` cho small, immutable data
- [ ] Implement `IDisposable` đúng cách cho resource cleanup
- [ ] Profile memory usage để tìm bottlenecks
- [ ] Sử dụng `Lazy<T>` cho expensive initialization
- [ ] Chọn collection type phù hợp với use case

---

## 🎯 Interview Questions

**Q1: Khi nào nên dùng struct thay vì class?**

- Khi object nhỏ (< 16 bytes)
- Immutable data
- Được dùng tạm thời (short-lived)
- Không cần inheritance

**Q2: String concatenation tốn bộ nhớ như thế nào?**

- String là immutable
- Mỗi lần concatenate tạo string object mới
- Dùng StringBuilder để reuse buffer

**Q3: Object pooling hoạt động thế nào?**

- Reuse objects thay vì tạo mới
- Giảm GC pressure
- Tăng performance cho frequent allocations

---

## 11. Stackalloc for Small Arrays

### Heap Allocation

```csharp
// ❌ Allocates on heap - GC pressure
public void ProcessData()
{
    byte[] buffer = new byte[256];
    // Process...
}
```

### Stack Allocation

```csharp
// ✅ Allocates on stack - no GC, super fast
public void ProcessData()
{
    Span<byte> buffer = stackalloc byte[256];
    // Process...
    // Automatically freed when method exits
}
```

**⚠️ Lưu ý:** Chỉ dùng cho arrays nhỏ (< 1KB) để tránh stack overflow!

---

## 12. String Pooling

### Avoid Duplicate Strings

```csharp
// Reduce memory for repeated strings
public class StringCache
{
    private static readonly HashSet<string> _pool = new();

    public static string Intern(string str)
    {
        if (_pool.TryGetValue(str, out var cached))
            return cached;

        _pool.Add(str);
        return str;
    }
}

// Usage
var name1 = StringCache.Intern("John");
var name2 = StringCache.Intern("John"); // Returns same reference
```

---

## 13. Avoid Closures in Hot Paths

### ❌ BAD: Closure Creates Allocation

```csharp
public void ProcessItems(List<int> items)
{
    int threshold = 100;

    // Closure captures 'threshold' - allocates object!
    var filtered = items.Where(x => x > threshold).ToList();
}
```

### ✅ GOOD: No Closure

```csharp
public void ProcessItems(List<int> items)
{
    const int threshold = 100; // Constant - no closure needed

    var filtered = items.Where(x => x > threshold).ToList();
}

// Or extract to method
private static bool IsAboveThreshold(int value) => value > 100;

public void ProcessItems(List<int> items)
{
    var filtered = items.Where(IsAboveThreshold).ToList();
}
```

---

## 14. Memory<T> for Async Operations

### Problem with Span<T>

```csharp
// ❌ ERROR: Can't use Span<T> in async methods
public async Task ProcessAsync()
{
    Span<byte> buffer = stackalloc byte[256]; // Compiler error!
    await DoWorkAsync(buffer);
}
```

### Solution: Memory<T>

```csharp
// ✅ GOOD: Memory<T> works with async
public async Task ProcessAsync()
{
    Memory<byte> buffer = new byte[256];
    await DoWorkAsync(buffer);
}
```

---

## 15. Reduce Large Object Heap (LOH) Allocations

### Problem: LOH Fragmentation

```csharp
// ❌ Objects > 85KB go to LOH
// LOH is not compacted → fragmentation!
public void ProcessLargeData()
{
    byte[] largeBuffer = new byte[100_000]; // Goes to LOH
    // Process...
} // LOH fragmentation
```

### Solution: Reuse Large Buffers

```csharp
// ✅ Reuse large buffers from pool
public class LargeBufferPool
{
    private static readonly ArrayPool<byte> _pool =
        ArrayPool<byte>.Create(100_000, 10);

    public void ProcessLargeData()
    {
        byte[] buffer = _pool.Rent(100_000);
        try
        {
            // Process...
        }
        finally
        {
            _pool.Return(buffer);
        }
    }
}
```

---

## 16. ValueStringBuilder (Advanced)

### Custom Stack-Allocated StringBuilder

```csharp
using System;
using System.Buffers;

public ref struct ValueStringBuilder
{
    private Span<char> _buffer;
    private int _pos;

    public ValueStringBuilder(Span<char> initialBuffer)
    {
        _buffer = initialBuffer;
        _pos = 0;
    }

    public void Append(char c)
    {
        if (_pos < _buffer.Length)
        {
            _buffer[_pos++] = c;
        }
    }

    public override string ToString()
    {
        return new string(_buffer.Slice(0, _pos));
    }
}

// Usage - zero heap allocations!
Span<char> buffer = stackalloc char[256];
var builder = new ValueStringBuilder(buffer);
builder.Append('H');
builder.Append('i');
string result = builder.ToString();
```

---

## 17. IMemoryOwner<T> Pattern

### Managed Memory Ownership

```csharp
using System.Buffers;

public class DataProcessor : IDisposable
{
    private IMemoryOwner<byte> _memoryOwner;

    public DataProcessor(int size)
    {
        _memoryOwner = MemoryPool<byte>.Shared.Rent(size);
    }

    public void Process()
    {
        Memory<byte> memory = _memoryOwner.Memory;
        // Use memory...
    }

    public void Dispose()
    {
        _memoryOwner?.Dispose();
    }
}
```

---

## 🎯 Advanced Interview Questions

**Q5: Phân biệt Span<T> và Memory<T>?**

- **Span<T>**: Stack-only, không dùng được trong async, super fast
- **Memory<T>**: Heap, dùng được trong async, cho async I/O

**Q6: Khi nào object vào Large Object Heap?**

- Objects ≥ 85,000 bytes (85KB)
- Arrays lớn
- LOH không được compacted → fragmentation
- Solution: Object pooling cho large objects

**Q7: Closure allocation là gì?**

- Lambda captures variables → tạo class
- Allocates object mỗi lần gọi
- Avoid trong hot paths bằng cách dùng static methods

**Q8: stackalloc có an toàn không?**

- An toàn khi dùng với Span<T> (bounds checking)
- Chỉ dùng cho small buffers (< 1KB)
- Stack overflow nếu allocate quá lớn

---

## 📊 Advanced Performance Tips

```csharp
// 1. Use stackalloc for small buffers
Span<int> numbers = stackalloc int[10];

// 2. Use ArrayPool for large temporary arrays
var pool = ArrayPool<byte>.Shared;
byte[] buffer = pool.Rent(4096);
try { /* use */ } finally { pool.Return(buffer); }

// 3. Use Memory<T> for async scenarios
Memory<byte> memory = new byte[1024];
await ProcessAsync(memory);

// 4. Avoid closures in hot paths
static bool Filter(int x) => x > 0;
items.Where(Filter); // No closure allocation

// 5. Use string.Create for zero-allocation string building
string result = string.Create(10, state, (span, s) =>
{
    // Fill span...
});
```

---

Happy Optimizing! 🚀
