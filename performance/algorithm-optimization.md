# ⚡ Algorithm Optimization / Tối Ưu Giải Thuật

## 📋 Tổng Quan / Overview

Algorithm optimization tập trung vào cải thiện time complexity và space complexity của code.

Algorithm optimization focuses on improving time and space complexity of your code.

---

## 1. Time Complexity Basics

### Big O Notation

```
O(1)        - Constant time      - Best
O(log n)    - Logarithmic        - Excellent
O(n)        - Linear             - Good
O(n log n)  - Linearithmic       - Acceptable
O(n²)       - Quadratic          - Poor
O(2ⁿ)       - Exponential        - Very Poor
```

---

## 2. LINQ Optimization

### ❌ BAD: Multiple Iterations

```csharp
var numbers = Enumerable.Range(1, 1000000).ToList();

// Three separate iterations!
var evens = numbers.Where(x => x % 2 == 0).ToList();
var doubled = evens.Select(x => x * 2).ToList();
var sum = doubled.Sum();
```

### ✅ GOOD: Single Iteration

```csharp
var numbers = Enumerable.Range(1, 1000000);

// Single iteration with deferred execution
var sum = numbers
    .Where(x => x % 2 == 0)
    .Select(x => x * 2)
    .Sum(); // Only materializes here
```

### Avoid Repeated Enumeration

```csharp
// ❌ BAD: Multiple enumerations
var query = numbers.Where(x => x > 100);
int count = query.Count();  // First enumeration
int sum = query.Sum();      // Second enumeration!

// ✅ GOOD: Materialize once
var list = numbers.Where(x => x > 100).ToList();
int count = list.Count;     // O(1)
int sum = list.Sum();       // Single enumeration
```

---

## 3. Loop Optimization

### Extract Invariant Calculations

```csharp
// ❌ BAD: Calculate in every iteration
for (int i = 0; i < items.Count; i++)
{
    var limit = GetComplexLimit(); // Calculated 1000 times!
    if (items[i] < limit)
    {
        // process
    }
}

// ✅ GOOD: Calculate once
var limit = GetComplexLimit();
for (int i = 0; i < items.Count; i++)
{
    if (items[i] < limit)
    {
        // process
    }
}
```

### Cache Length/Count

```csharp
// ❌ BAD: Property access in condition
for (int i = 0; i < list.Count; i++) // Count accessed every iteration
{
    Process(list[i]);
}

// ✅ GOOD: Cache count
int count = list.Count;
for (int i = 0; i < count; i++)
{
    Process(list[i]);
}
```

---

## 4. Dictionary vs List Lookup

### Linear Search O(n)

```csharp
// ❌ BAD: O(n) for each lookup
var users = new List<User>();

User FindUser(int id)
{
    return users.FirstOrDefault(u => u.Id == id); // O(n)
}
```

### Hash Table Lookup O(1)

```csharp
// ✅ GOOD: O(1) for each lookup
var users = new Dictionary<int, User>();

User FindUser(int id)
{
    return users.TryGetValue(id, out var user) ? user : null; // O(1)
}
```

---

## 5. Lazy Evaluation

### Eager Loading (All at Once)

```csharp
// ❌ Loads everything into memory
var allUsers = database.Users.ToList();
var activeUsers = allUsers.Where(u => u.IsActive).ToList();
```

### Lazy Loading (On Demand)

```csharp
// ✅ GOOD: Only loads what's needed
var activeUsers = database.Users
    .Where(u => u.IsActive)
    .Take(10); // SQL query executed with LIMIT

foreach (var user in activeUsers) // Enumerated lazily
{
    Process(user);
}
```

---

## 6. Parallel Processing

### Sequential Processing

```csharp
// Process one by one - slow for independent operations
var results = new List<int>();
foreach (var item in items)
{
    results.Add(ProcessItem(item)); // Takes 100ms each
}
// Total: 100ms * n items
```

### Parallel Processing

```csharp
using System.Threading.Tasks;

// ✅ Process in parallel - faster for CPU-bound work
var results = items
    .AsParallel()
    .Select(item => ProcessItem(item))
    .ToList();
// Total: ~100ms (if enough cores)

// Or use Parallel.ForEach
var resultList = new ConcurrentBag<int>();
Parallel.ForEach(items, item =>
{
    resultList.Add(ProcessItem(item));
});
```

**⚠️ Cảnh Báo:** Only use for CPU-bound operations, not for I/O bound!

---

## 7. Caching Strategies

### Without Cache

```csharp
// ❌ Expensive calculation every time
public decimal CalculatePrice(int productId)
{
    var product = database.GetProduct(productId); // DB call
    var discount = CalculateDiscount(product);     // Complex logic
    return product.Price * (1 - discount);
}
```

### With Memory Cache

```csharp
using Microsoft.Extensions.Caching.Memory;

// ✅ GOOD: Cache expensive results
public class PriceCalculator
{
    private readonly IMemoryCache _cache;

    public decimal CalculatePrice(int productId)
    {
        string cacheKey = $"price_{productId}";

        if (!_cache.TryGetValue(cacheKey, out decimal price))
        {
            var product = database.GetProduct(productId);
            var discount = CalculateDiscount(product);
            price = product.Price * (1 - discount);

            _cache.Set(cacheKey, price, TimeSpan.FromMinutes(5));
        }

        return price;
    }
}
```

---

## 8. String Comparison Optimization

### Case-Insensitive Comparison

```csharp
// ❌ BAD: Creates new strings
if (str1.ToLower() == str2.ToLower())
{
    // ...
}

// ✅ GOOD: No allocation
if (string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase))
{
    // ...
}
```

---

## 9. Early Exit Patterns

### Short-Circuit Evaluation

```csharp
// ✅ Check cheap conditions first
public bool IsValid(Product product)
{
    // Quick checks first
    if (product == null) return false;
    if (string.IsNullOrEmpty(product.Name)) return false;

    // Expensive checks last
    if (!IsValidInDatabase(product)) return false;

    return true;
}
```

### Break Early from Loops

```csharp
// ❌ BAD: Continues checking after found
bool found = false;
foreach (var item in items)
{
    if (item.Id == targetId)
    {
        found = true;
    }
}

// ✅ GOOD: Exit immediately when found
bool found = false;
foreach (var item in items)
{
    if (item.Id == targetId)
    {
        found = true;
        break; // Stop searching!
    }
}

// ✅ BETTER: Use LINQ
bool found = items.Any(item => item.Id == targetId);
```

---

## 10. Avoid Premature Optimization

### Algorithm Example: Binary Search

```csharp
// Linear search - O(n)
public int LinearSearch(int[] array, int target)
{
    for (int i = 0; i < array.Length; i++)
    {
        if (array[i] == target)
            return i;
    }
    return -1;
}

// Binary search - O(log n) - but requires sorted array
public int BinarySearch(int[] sortedArray, int target)
{
    int left = 0;
    int right = sortedArray.Length - 1;

    while (left <= right)
    {
        int mid = left + (right - left) / 2;

        if (sortedArray[mid] == target)
            return mid;

        if (sortedArray[mid] < target)
            left = mid + 1;
        else
            right = mid - 1;
    }

    return -1;
}
```

---

## 11. Data Structure Selection

### Choose Right Data Structure

```csharp
// Fast lookup by key - O(1)
Dictionary<string, User> userDict;

// Fast lookup + ordering
SortedDictionary<string, User> sortedUsers;

// Unique items, fast contains - O(1)
HashSet<int> uniqueIds;

// Sorted unique items
SortedSet<int> sortedIds;

// Queue - FIFO
Queue<Task> taskQueue;

// Stack - LIFO
Stack<State> stateStack;

// Priority Queue (min heap)
PriorityQueue<Task, int> prioritizedTasks;
```

---

## 12. Async/Await for I/O Operations

### Synchronous (Blocks Thread)

```csharp
// ❌ BAD: Blocks thread during I/O
public List<User> GetUsers()
{
    var response = httpClient.GetStringAsync(url).Result; // Blocks!
    return JsonSerializer.Deserialize<List<User>>(response);
}
```

### Asynchronous (Non-blocking)

```csharp
// ✅ GOOD: Doesn't block thread during I/O
public async Task<List<User>> GetUsersAsync()
{
    var response = await httpClient.GetStringAsync(url);
    return JsonSerializer.Deserialize<List<User>>(response);
}
```

---

## 13. Batch Operations

### Individual Operations

```csharp
// ❌ BAD: N database calls
foreach (var user in users)
{
    database.UpdateUser(user); // Separate DB call each time
}
```

### Batch Operations

```csharp
// ✅ GOOD: Single batch operation
database.UpdateUsers(users); // One DB call for all

// Or use bulk operations
await database.BulkUpdateAsync(users);
```

---

## 📊 Performance Comparison Table

| Operation                | Slow Approach       | Fast Approach        | Speedup |
| ------------------------ | ------------------- | -------------------- | ------- |
| String concat (1000x)    | + operator          | StringBuilder        | 100x    |
| Lookup in 10k items      | List.FirstOrDefault | Dictionary lookup    | 1000x   |
| Sequential processing    | foreach             | Parallel.ForEach     | 4-8x    |
| Multiple LINQ operations | ToList() each       | Chain without ToList | 3-5x    |
| Case-insensitive compare | ToLower()           | StringComparison     | 2x      |

---

## ✅ Optimization Checklist

- [ ] Profile code trước khi optimize (measure first!)
- [ ] Optimize bottlenecks, không phải mọi thứ
- [ ] Dùng appropriate data structures (Dictionary cho lookup)
- [ ] Tránh multiple enumeration trong LINQ
- [ ] Cache expensive calculations
- [ ] Use async/await cho I/O operations
- [ ] Parallelize CPU-bound independent operations
- [ ] Early exit patterns (break, short-circuit)
- [ ] Batch operations thay vì individual calls
- [ ] Tránh boxing/unboxing trong hot paths

---

## 🎯 Interview Questions

**Q1: Phân biệt O(n) và O(n²)?**

```csharp
// O(n) - Linear
for (int i = 0; i < n; i++)
{
    Console.WriteLine(i);
}

// O(n²) - Quadratic
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        Console.WriteLine(i + j);
    }
}
```

**Q2: Khi nào nên dùng AsParallel()?**

- CPU-bound operations
- Independent operations (no shared state)
- Large dataset
- NOT for I/O bound operations

**Q3: Dictionary vs List cho lookup?**

- Dictionary: O(1) lookup - dùng khi cần frequent lookups
- List: O(n) lookup - dùng khi iterate nhiều hơn lookup

**Q4: Lazy evaluation là gì?**

- LINQ queries không execute cho đến khi enumerate
- Saves memory và processing time
- Cho phép query composition

---

## 💡 Real-World Example

### Before Optimization

```csharp
// Slow: O(n²) + multiple DB calls
public List<OrderDTO> GetOrdersWithDetails(List<int> orderIds)
{
    var results = new List<OrderDTO>();

    foreach (var orderId in orderIds)
    {
        var order = database.GetOrder(orderId); // N DB calls

        foreach (var itemId in order.ItemIds)
        {
            var item = database.GetItem(itemId); // N*M DB calls!
            // process...
        }

        results.Add(order);
    }

    return results;
}
```

### After Optimization

```csharp
// Fast: O(n) + batch operations
public async Task<List<OrderDTO>> GetOrdersWithDetailsAsync(List<int> orderIds)
{
    // Batch DB calls
    var orders = await database.GetOrdersBatchAsync(orderIds); // 1 DB call
    var allItemIds = orders.SelectMany(o => o.ItemIds).Distinct().ToList();
    var items = await database.GetItemsBatchAsync(allItemIds); // 1 DB call

    // Create lookup dictionary for O(1) access
    var itemLookup = items.ToDictionary(i => i.Id);

    // Parallel processing
    var results = orders.AsParallel()
        .Select(order => MapOrderToDTO(order, itemLookup))
        .ToList();

    return results;
}
```

**Improvement:** 100+ DB calls → 2 DB calls, O(n²) → O(n)

---

## 14. ValueTask for Performance

### Task Allocation Overhead

```csharp
// ❌ Task<T> allocates on heap every time
public async Task<int> GetCachedValueAsync(string key)
{
    if (_cache.TryGetValue(key, out int value))
        return value; // Still allocates Task!

    return await LoadFromDatabaseAsync(key);
}
```

### ValueTask - Zero Allocation for Sync Path

```csharp
// ✅ ValueTask<T> - no allocation if sync path
public async ValueTask<int> GetCachedValueAsync(string key)
{
    if (_cache.TryGetValue(key, out int value))
        return value; // No allocation!

    return await LoadFromDatabaseAsync(key);
}
```

**Khi nào dùng ValueTask:**

- Method có thể complete synchronously
- Hot path được called thường xuyên
- Cần reduce allocation overhead

---

## 15. ConfigureAwait(false) for Performance

### Avoid Context Switching

```csharp
// ❌ Captures synchronization context - overhead
public async Task<string> LoadDataAsync()
{
    var data = await httpClient.GetStringAsync(url);
    return Process(data);
}

// ✅ No context capture - faster in libraries
public async Task<string> LoadDataAsync()
{
    var data = await httpClient.GetStringAsync(url).ConfigureAwait(false);
    return Process(data);
}
```

**💡 Rule:** Always use `ConfigureAwait(false)` in library code!

---

## 16. Ref Returns for Zero-Copy

### Copy Large Struct (Slow)

```csharp
public struct LargeData
{
    public int[] Values; // Large struct
}

// ❌ Copies entire struct
public LargeData GetData(int index)
{
    return _array[index]; // Copy!
}
```

### Ref Return (Zero Copy)

```csharp
// ✅ Returns reference - no copy
public ref LargeData GetData(int index)
{
    return ref _array[index]; // No copy!
}

// Usage
ref var data = ref GetData(5);
data.Values[0] = 100; // Modifies original
```

---

## 17. In Parameters for Large Structs

### Pass by Value (Copies)

```csharp
public struct Vector3
{
    public double X, Y, Z;
}

// ❌ Copies struct on every call
public double Distance(Vector3 a, Vector3 b)
{
    return Math.Sqrt(Math.Pow(a.X - b.X, 2) +
                     Math.Pow(a.Y - b.Y, 2) +
                     Math.Pow(a.Z - b.Z, 2));
}
```

### Pass by Readonly Reference

```csharp
// ✅ Passes reference - no copy
public double Distance(in Vector3 a, in Vector3 b)
{
    return Math.Sqrt(Math.Pow(a.X - b.X, 2) +
                     Math.Pow(a.Y - b.Y, 2) +
                     Math.Pow(a.Z - b.Z, 2));
}
```

---

## 18. Switch Expression Optimization

### Traditional Switch

```csharp
// Older syntax
public string GetCategory(int score)
{
    switch (score)
    {
        case >= 90: return "A";
        case >= 80: return "B";
        case >= 70: return "C";
        default: return "F";
    }
}
```

### Modern Switch Expression

```csharp
// ✅ More efficient, compiler optimized
public string GetCategory(int score) => score switch
{
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    _ => "F"
};
```

---

## 19. Avoid Enumeration Multiple Times

### Problem: Multiple Enumerations

```csharp
// ❌ BAD: Enumerates 3 times!
public void ProcessData(IEnumerable<int> data)
{
    if (data.Any()) // Enumeration 1
    {
        int count = data.Count(); // Enumeration 2
        int sum = data.Sum();     // Enumeration 3
    }
}
```

### Solution: Materialize Once

```csharp
// ✅ GOOD: Enumerate once
public void ProcessData(IEnumerable<int> data)
{
    var list = data.ToList(); // Enumerate once

    if (list.Count > 0)
    {
        int count = list.Count; // O(1)
        int sum = list.Sum();   // Single enumeration
    }
}
```

---

## 20. String.GetHashCode() for Fast Comparison

### Slow String Comparison

```csharp
// ❌ Full string comparison every time
if (longString1 == longString2)
{
    // ...
}
```

### Fast Hash Comparison

```csharp
// ✅ Quick rejection via hash
int hash1 = longString1.GetHashCode();
int hash2 = longString2.GetHashCode();

if (hash1 != hash2)
{
    return false; // Quick rejection
}

// Only full compare if hashes match
if (longString1 == longString2)
{
    // ...
}
```

---

## 21. Avoid LINQ in Performance-Critical Loops

### LINQ in Hot Path

```csharp
// ❌ LINQ overhead in tight loop
for (int i = 0; i < 1000000; i++)
{
    var result = items.Where(x => x.Value > i).FirstOrDefault();
}
```

### Manual Loop

```csharp
// ✅ Much faster for hot paths
for (int i = 0; i < 1000000; i++)
{
    Item result = null;
    foreach (var item in items)
    {
        if (item.Value > i)
        {
            result = item;
            break;
        }
    }
}
```

---

## 22. Use Frozen Collections (.NET 8+)

### Regular Collections

```csharp
// Standard collections - mutable, slower lookups
var dictionary = new Dictionary<string, int>
{
    ["one"] = 1,
    ["two"] = 2,
    ["three"] = 3
};
```

### Frozen Collections

```csharp
using System.Collections.Frozen;

// ✅ Immutable, optimized for lookups - up to 4x faster!
var frozenDict = new Dictionary<string, int>
{
    ["one"] = 1,
    ["two"] = 2,
    ["three"] = 3
}.ToFrozenDictionary();

var frozenSet = new[] { 1, 2, 3, 4, 5 }.ToFrozenSet();
```

**Khi nào dùng:**

- Read-only collections
- Frequent lookups
- Data không thay đổi sau initialization

---

## 23. Benchmarking with BenchmarkDotNet

### Proper Benchmarking

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class StringConcatBenchmark
{
    [Benchmark]
    public string StringConcat()
    {
        string result = "";
        for (int i = 0; i < 100; i++)
            result += i.ToString();
        return result;
    }

    [Benchmark]
    public string StringBuilder()
    {
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < 100; i++)
            sb.Append(i);
        return sb.ToString();
    }
}

// Run benchmark
public class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<StringConcatBenchmark>();
    }
}
```

---

## 🎯 Advanced Interview Questions

**Q5: ValueTask vs Task - khi nào dùng?**

- **ValueTask**: Hot path có thể complete sync, reduce allocations
- **Task**: Async operations, cached/reused tasks
- Don't await ValueTask multiple times!

**Q6: ConfigureAwait(false) có tác dụng gì?**

- Không capture SynchronizationContext
- Faster continuation
- Dùng trong library code
- Không dùng trong UI code

**Q7: Ref return có rủi ro gì?**

- Caller có thể modify original data
- Lifetime management phức tạp
- Use with readonly struct để safe hơn

**Q8: Frozen collections vs regular?**

- Frozen: Immutable, optimized lookups, initialization overhead
- Regular: Mutable, slower lookups, no init overhead
- Frozen best cho read-heavy workloads

---

## 📊 Performance Comparison - Advanced

| Technique             | Speedup | When to Use                    |
| --------------------- | ------- | ------------------------------ |
| ValueTask vs Task     | 2-3x    | Hot path with sync completion  |
| ConfigureAwait(false) | 1.2-2x  | Library code, no UI context    |
| Ref returns           | 2-5x    | Large structs, zero-copy       |
| In parameters         | 1.5-3x  | Large readonly structs         |
| Frozen collections    | 2-4x    | Read-only data, frequent reads |
| Manual loop vs LINQ   | 3-10x   | Hot paths, tight loops         |

---

## 💡 Real-World Advanced Example

```csharp
public class HighPerformanceCache<TKey, TValue>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _cache = new();

    // ValueTask for zero allocation on cache hit
    public ValueTask<TValue> GetOrCreateAsync(
        TKey key,
        Func<TKey, Task<TValue>> factory)
    {
        // Hot path - no allocation if cached
        if (_cache.TryGetValue(key, out var value))
            return new ValueTask<TValue>(value);

        // Cold path - async load
        return GetOrCreateSlowAsync(key, factory);
    }

    private async ValueTask<TValue> GetOrCreateSlowAsync(
        TKey key,
        Func<TKey, Task<TValue>> factory)
    {
        var value = await factory(key).ConfigureAwait(false);
        _cache[key] = value;
        return value;
    }

    // Frozen dictionary for read-only scenarios
    public FrozenDictionary<TKey, TValue> ToFrozen()
    {
        return _cache.ToFrozenDictionary();
    }
}
```

---

Happy Optimizing! ⚡🚀
