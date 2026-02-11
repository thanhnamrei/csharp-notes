# 🚀 Performance Optimization / Tối Ưu Hiệu Năng

## 📋 Tổng Quan / Overview

Performance optimization trong C# là quá trình cải thiện tốc độ thực thi và giảm resource usage của ứng dụng.

Performance optimization in C# is the process of improving execution speed and reducing resource usage of applications.

---

## 📚 Nội Dung / Contents

### 1. [Memory Optimization](memory-optimization.md)

Tối ưu bộ nhớ, giảm GC pressure, và cải thiện memory footprint.

Topics:

- Value Types vs Reference Types
- String Optimization (StringBuilder)
- Collection Optimization
- Object Pooling
- Span<T> and Memory<T>
- Avoiding Boxing
- Lazy Initialization
- Dispose Pattern
- Memory Profiling
- Struct Best Practices

### 2. [Algorithm Optimization](algorithm-optimization.md)

Tối ưu thuật toán, cải thiện time complexity và space complexity.

Topics:

- Time Complexity (Big O)
- LINQ Optimization
- Loop Optimization
- Dictionary vs List Lookup
- Lazy Evaluation
- Parallel Processing
- Caching Strategies
- String Comparison
- Early Exit Patterns
- Data Structure Selection
- Async/Await Optimization
- Batch Operations
- ValueTask Performance
- ConfigureAwait Optimization
- Ref Returns & In Parameters
- Switch Expression
- Frozen Collections (.NET 8+)
- BenchmarkDotNet

### 3. [Collection Performance](collection-performance.md)

So sánh hiệu năng các collection types và cách chọn đúng collection.

Topics:

- List vs Array vs LinkedList
- Dictionary vs HashSet vs SortedSet
- Queue and Stack
- Concurrent Collections
- Immutable Collections
- Frozen Collections
- Collection Selection Guide
- Real-World Benchmarks

### 4. [Garbage Collection](garbage-collection.md)

Hiểu và tối ưu Garbage Collection để giảm GC pressure.

Topics:

- GC Generations (Gen 0, 1, 2)
- Large Object Heap (LOH)
- GC Modes (Workstation vs Server)
- Weak References
- Finalization vs IDisposable
- GC Tuning
- Memory Leak Detection
- GC Profiling
- GC Latency Modes

### 5. [Benchmarking & Profiling](benchmarking.md)

Đo lường performance chính xác và tìm bottlenecks.

Topics:

- BenchmarkDotNet
- Manual Timing
- Memory Profiling
- CPU Profiling Tools
- Async Benchmarking
- Common Mistakes
- Performance Counters
- Profiling Checklist

### 6. [Real-World Scenarios](real-world-scenarios.md)

Tối ưu performance trong các tình huống thực tế.

Topics:

- ASP.NET Core Web API Performance
- Entity Framework Core Optimization
- Microservices Performance
- Database Performance
- JSON Serialization
- Async/Await Best Practices
- Logging Performance
- Complete E-Commerce Example
- Distributed Caching with Redis
- HTTP Client Best Practices
- Bulk Operations
- Health Checks & Monitoring

---

## 🎯 Key Concepts / Khái Niệm Chính

### Performance Pillars

1. **Speed / Tốc độ** - Execution time
2. **Memory / Bộ nhớ** - Memory usage
3. **Scalability / Khả năng mở rộng** - Handle load
4. **Responsiveness / Phản hồi** - User experience

---

## ⚡ Quick Reference

### Most Impactful Optimizations

| Optimization                          | Impact           | Difficulty  | When to Use            |
| ------------------------------------- | ---------------- | ----------- | ---------------------- |
| Use Dictionary instead of List search | 🔥🔥🔥 Very High | ⭐ Easy     | Frequent lookups       |
| Use StringBuilder                     | 🔥🔥🔥 Very High | ⭐ Easy     | String concatenation   |
| Object Pooling                        | 🔥🔥 High        | ⭐⭐ Medium | Frequent allocations   |
| Span<T> / Memory<T>                   | 🔥🔥 High        | ⭐⭐⭐ Hard | Array slicing          |
| Async/Await for I/O                   | 🔥🔥🔥 Very High | ⭐⭐ Medium | I/O operations         |
| Parallel Processing                   | 🔥🔥 High        | ⭐⭐ Medium | CPU-bound work         |
| Caching                               | 🔥🔥🔥 Very High | ⭐⭐ Medium | Expensive calculations |
| LINQ optimization                     | 🔥🔥 High        | ⭐ Easy     | Data processing        |

---

## 📊 Performance Metrics

### What to Measure

```csharp
using System.Diagnostics;

// 1. Execution Time
var sw = Stopwatch.StartNew();
// Your code here
sw.Stop();
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");

// 2. Memory Usage
long memBefore = GC.GetTotalMemory(false);
// Your code here
long memAfter = GC.GetTotalMemory(false);
Console.WriteLine($"Memory: {(memAfter - memBefore) / 1024}KB");

// 3. GC Collections
Console.WriteLine($"Gen 0: {GC.CollectionCount(0)}");
Console.WriteLine($"Gen 1: {GC.CollectionCount(1)}");
Console.WriteLine($"Gen 2: {GC.CollectionCount(2)}");
```

---

## 🛠️ Profiling Tools

### Built-in Tools

- **Stopwatch** - Measure execution time
- **GC.GetTotalMemory()** - Memory usage
- **Performance Counters** - System metrics

### External Tools

- **Visual Studio Profiler** - Comprehensive analysis
- **JetBrains dotMemory** - Memory profiling
- **PerfView** - Advanced profiling
- **BenchmarkDotNet** - Micro-benchmarking

---

## 💡 Golden Rules

1. **Measure First** 📏
   - "Premature optimization is the root of all evil"
   - Profile để tìm bottlenecks
   - Optimize chỗ quan trọng nhất

2. **Choose Right Data Structure** 📦
   - Dictionary cho lookups
   - List cho sequential access
   - HashSet cho unique items
   - Queue/Stack cho FIFO/LIFO

3. **Avoid Allocations** 🚫
   - Use Span<T> thay vì array slicing
   - StringBuilder cho string concatenation
   - Object pooling cho frequent allocations
   - Struct cho small objects

4. **Async for I/O** ⚡
   - Async/await cho database, API, file I/O
   - Parallel cho CPU-bound work
   - Don't block threads

5. **Cache Expensive Operations** 💾
   - Memory cache cho calculations
   - Lazy<T> cho expensive initialization
   - Static caching cho unchanging data

---

## ✅ Performance Checklist

### Before Optimization

- [ ] Profile để identify bottlenecks
- [ ] Set performance targets
- [ ] Establish baseline metrics

### Common Optimizations

- [ ] Use StringBuilder cho multiple string concatenations
- [ ] Pre-allocate collection capacity
- [ ] Use Dictionary thay vì List.Find()
- [ ] Implement caching cho expensive operations
- [ ] Use async/await cho I/O operations
- [ ] Batch database operations
- [ ] Avoid boxing/unboxing
- [ ] Use Span<T> cho array operations
- [ ] Implement object pooling nếu cần
- [ ] Optimize LINQ queries

### After Optimization

- [ ] Measure results
- [ ] Compare with baseline
- [ ] Verify correctness
- [ ] Document changes

---

## 🎯 Interview Questions

**Q1: Khi nào nên optimize performance?**

- Sau khi profile và identify bottlenecks
- Khi có specific performance requirements
- KHÔNG optimize mọi thứ ngay từ đầu

**Q2: Phân biệt CPU-bound và I/O-bound?**

- **CPU-bound**: Heavy computation → use Parallel
- **I/O-bound**: Database, file, network → use async/await

**Q3: Làm thế nào giảm memory usage?**

- Use value types cho small data
- Object pooling
- Span<T> để avoid allocations
- Dispose resources properly
- Avoid boxing

**Q4: LINQ có chậm không?**

- Depends! Có thể nhanh hoặc chậm
- Deferred execution là advantage
- Avoid multiple enumeration
- ToList() khi cần reuse results

---

## 🚀 Performance Best Practices Summary

```csharp
// 1. Use right collection
var lookup = new Dictionary<int, User>(); // O(1) lookup

// 2. Use StringBuilder
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
    sb.Append(i);

// 3. Use Span<T>
Span<int> slice = array.AsSpan(0, 10);

// 4. Object pooling
var buffer = ArrayPool<byte>.Shared.Rent(4096);

// 5. Async for I/O
await httpClient.GetAsync(url);

// 6. Caching
var cached = _cache.GetOrCreate(key, entry => ExpensiveOperation());

// 7. Parallel for CPU work
Parallel.ForEach(items, item => ProcessItem(item));

// 8. Early exit
if (quickCheck) return;
ExpensiveOperation();
```

---

## 📚 Next Steps

1. Read [Memory Optimization](memory-optimization.md)
2. Read [Algorithm Optimization](algorithm-optimization.md)
3. Practice with real code examples
4. Profile your own applications
5. Benchmark different approaches

---

**Remember:** Make it work, make it right, make it fast - IN THAT ORDER! 🎯

Happy Optimizing! 🚀
