# 📊 Benchmarking & Profiling / Đo Lường Hiệu Năng

## 📋 Tổng Quan / Overview

Benchmarking là quá trình đo lường performance một cách chính xác để identify bottlenecks và verify optimizations.

Benchmarking is the process of accurately measuring performance to identify bottlenecks and verify optimizations.

---

## 1. BenchmarkDotNet - The Gold Standard

### Installation

```bash
dotnet add package BenchmarkDotNet
```

### Basic Benchmark

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 5)]
public class StringBenchmark
{
    private const int N = 1000;

    [Benchmark(Baseline = true)]
    public string StringConcat()
    {
        string result = "";
        for (int i = 0; i < N; i++)
            result += i.ToString();
        return result;
    }

    [Benchmark]
    public string StringBuilderAppend()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < N; i++)
            sb.Append(i);
        return sb.ToString();
    }

    [Benchmark]
    public string StringCreate()
    {
        return string.Join("", Enumerable.Range(0, N));
    }
}

// Run
public class Program
{
    public static void Main()
    {
        var summary = BenchmarkRunner.Run<StringBenchmark>();
    }
}
```

### Output Example

```
|              Method |       Mean |     Error |    StdDev | Ratio | Gen0 | Allocated |
|-------------------- |-----------:|----------:|----------:|------:|-----:|----------:|
|        StringConcat | 21,450.0 μs | 150.20 μs | 133.16 μs |  1.00 | 2000 |  8000 KB |
| StringBuilderAppend |     45.2 μs |   0.50 μs |   0.44 μs |  0.00 |   10 |    40 KB |
|        StringCreate |     28.3 μs |   0.35 μs |   0.31 μs |  0.00 |    8 |    32 KB |
```

---

## 2. Attributes & Configuration

### Memory Diagnostics

```csharp
[MemoryDiagnoser]  // Shows memory allocations
public class MyBenchmark
{
    [Benchmark]
    public void Test() { }
}
```

### Parameters

```csharp
[MemoryDiagnoser]
public class CollectionBenchmark
{
    [Params(10, 100, 1000, 10000)]
    public int Size;

    [Benchmark]
    public void ListAdd()
    {
        var list = new List<int>();
        for (int i = 0; i < Size; i++)
            list.Add(i);
    }

    [Benchmark]
    public void ListAddWithCapacity()
    {
        var list = new List<int>(Size);
        for (int i = 0; i < Size; i++)
            list.Add(i);
    }
}
```

### Setup & Cleanup

```csharp
public class DatabaseBenchmark
{
    private DatabaseContext _context;

    [GlobalSetup]
    public void Setup()
    {
        _context = new DatabaseContext();
        _context.Database.EnsureCreated();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        // Runs before each iteration
    }

    [Benchmark]
    public async Task QueryData()
    {
        await _context.Users.ToListAsync();
    }
}
```

---

## 3. Manual Timing (Quick Tests)

### Stopwatch

```csharp
using System.Diagnostics;

public class ManualBenchmark
{
    public void TestPerformance()
    {
        var sw = Stopwatch.StartNew();

        // Your code here
        PerformOperation();

        sw.Stop();
        Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");
        Console.WriteLine($"Time: {sw.Elapsed.TotalMicroseconds}μs");
    }
}
```

### Multiple Runs

```csharp
public void BenchmarkWithWarmup()
{
    // Warmup
    for (int i = 0; i < 10; i++)
        PerformOperation();

    // Actual benchmark
    var times = new List<long>();
    var sw = new Stopwatch();

    for (int i = 0; i < 100; i++)
    {
        sw.Restart();
        PerformOperation();
        sw.Stop();
        times.Add(sw.ElapsedTicks);
    }

    var avg = times.Average();
    var median = times.OrderBy(x => x).ElementAt(times.Count / 2);

    Console.WriteLine($"Average: {TimeSpan.FromTicks((long)avg).TotalMicroseconds}μs");
    Console.WriteLine($"Median: {TimeSpan.FromTicks(median).TotalMicroseconds}μs");
}
```

---

## 4. Memory Profiling

### GC Statistics

```csharp
public class MemoryProfiler
{
    public void ProfileMemory(Action action)
    {
        // Force garbage collection before
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        long memBefore = GC.GetTotalMemory(false);
        int gen0Before = GC.CollectionCount(0);
        int gen1Before = GC.CollectionCount(1);
        int gen2Before = GC.CollectionCount(2);

        // Run action
        action();

        // Measure after
        long memAfter = GC.GetTotalMemory(false);
        int gen0After = GC.CollectionCount(0);
        int gen1After = GC.CollectionCount(1);
        int gen2After = GC.CollectionCount(2);

        Console.WriteLine($"Memory used: {(memAfter - memBefore) / 1024.0:F2} KB");
        Console.WriteLine($"Gen 0 collections: {gen0After - gen0Before}");
        Console.WriteLine($"Gen 1 collections: {gen1After - gen1Before}");
        Console.WriteLine($"Gen 2 collections: {gen2After - gen2Before}");
    }
}

// Usage
var profiler = new MemoryProfiler();
profiler.ProfileMemory(() =>
{
    var list = new List<int>();
    for (int i = 0; i < 100000; i++)
        list.Add(i);
});
```

---

## 5. CPU Profiling Tools

### Visual Studio Profiler

```
1. Debug → Performance Profiler
2. Select tools:
   - CPU Usage
   - Memory Usage
   - .NET Object Allocation
3. Start profiling
4. Analyze hot paths
```

### dotTrace (JetBrains)

```
- Timeline profiling
- Sampling profiling
- Call tree analysis
- SQL query profiling
```

### PerfView (Free, Advanced)

```
- ETW event collection
- CPU sampling
- Memory allocation tracking
- GC analysis
```

---

## 6. Async Benchmarking

### Async Methods

```csharp
[MemoryDiagnoser]
public class AsyncBenchmark
{
    private readonly HttpClient _client = new();

    [Benchmark]
    public async Task<string> GetDataAsync()
    {
        return await _client.GetStringAsync("https://api.example.com/data");
    }

    [Benchmark]
    public async Task<string> GetDataWithConfigureAwait()
    {
        return await _client
            .GetStringAsync("https://api.example.com/data")
            .ConfigureAwait(false);
    }
}
```

---

## 7. Common Benchmarking Mistakes

### ❌ Mistake 1: No Warmup

```csharp
// BAD: First run includes JIT compilation
var sw = Stopwatch.StartNew();
MyMethod();
sw.Stop(); // Includes JIT time!
```

### ✅ Solution: Warmup

```csharp
// GOOD: Warmup first
MyMethod(); // JIT compilation happens here

var sw = Stopwatch.StartNew();
MyMethod(); // Pure execution time
sw.Stop();
```

### ❌ Mistake 2: Dead Code Elimination

```csharp
[Benchmark]
public void Calculate()
{
    int result = HeavyCalculation();
    // result not used - compiler may optimize away!
}
```

### ✅ Solution: Consume Result

```csharp
[Benchmark]
public int Calculate()
{
    return HeavyCalculation(); // Compiler can't eliminate
}
```

### ❌ Mistake 3: Shared State

```csharp
private List<int> _sharedList = new();

[Benchmark]
public void AddItems()
{
    _sharedList.Add(1); // State carries over between runs!
}
```

### ✅ Solution: Fresh State

```csharp
[IterationSetup]
public void Setup()
{
    _sharedList = new List<int>();
}

[Benchmark]
public void AddItems()
{
    _sharedList.Add(1);
}
```

---

## 8. Real-World Benchmark Example

```csharp
[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 10)]
public class DataProcessingBenchmark
{
    private List<Order> _orders;

    [Params(100, 1000, 10000)]
    public int OrderCount;

    [GlobalSetup]
    public void Setup()
    {
        _orders = Enumerable.Range(0, OrderCount)
            .Select(i => new Order
            {
                Id = i,
                Amount = i * 10,
                CustomerId = i % 100
            })
            .ToList();
    }

    [Benchmark(Baseline = true)]
    public List<CustomerTotal> GroupWithLinq()
    {
        return _orders
            .GroupBy(o => o.CustomerId)
            .Select(g => new CustomerTotal
            {
                CustomerId = g.Key,
                Total = g.Sum(o => o.Amount)
            })
            .ToList();
    }

    [Benchmark]
    public List<CustomerTotal> GroupWithDictionary()
    {
        var totals = new Dictionary<int, decimal>();

        foreach (var order in _orders)
        {
            if (!totals.ContainsKey(order.CustomerId))
                totals[order.CustomerId] = 0;
            totals[order.CustomerId] += order.Amount;
        }

        return totals
            .Select(kvp => new CustomerTotal
            {
                CustomerId = kvp.Key,
                Total = kvp.Value
            })
            .ToList();
    }

    [Benchmark]
    public List<CustomerTotal> GroupWithTryGetValue()
    {
        var totals = new Dictionary<int, decimal>();

        foreach (var order in _orders)
        {
            if (totals.TryGetValue(order.CustomerId, out var current))
                totals[order.CustomerId] = current + order.Amount;
            else
                totals[order.CustomerId] = order.Amount;
        }

        return totals
            .Select(kvp => new CustomerTotal
            {
                CustomerId = kvp.Key,
                Total = kvp.Value
            })
            .ToList();
    }
}

public class Order
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int CustomerId { get; set; }
}

public class CustomerTotal
{
    public int CustomerId { get; set; }
    public decimal Total { get; set; }
}
```

---

## 9. Performance Counters

### Windows Performance Counters

```csharp
using System.Diagnostics;

public class PerformanceMonitor
{
    private PerformanceCounter _cpuCounter;
    private PerformanceCounter _ramCounter;

    public void Initialize()
    {
        _cpuCounter = new PerformanceCounter(
            "Processor", "% Processor Time", "_Total");

        _ramCounter = new PerformanceCounter(
            "Memory", "Available MBytes");
    }

    public void Monitor()
    {
        Console.WriteLine($"CPU: {_cpuCounter.NextValue()}%");
        Console.WriteLine($"RAM: {_ramCounter.NextValue()}MB");
    }
}
```

---

## 10. Profiling Checklist

### Before Profiling

- [ ] Build in Release mode
- [ ] Disable debugger
- [ ] Close other applications
- [ ] Run on target hardware
- [ ] Ensure representative data

### During Profiling

- [ ] Profile realistic scenarios
- [ ] Multiple runs for consistency
- [ ] Profile hot paths
- [ ] Check memory allocations
- [ ] Monitor GC collections

### After Profiling

- [ ] Analyze bottlenecks
- [ ] Prioritize optimizations
- [ ] Benchmark before/after
- [ ] Verify correctness
- [ ] Document changes

---

## 📊 Benchmark Results Interpretation

### Understanding Metrics

```
Mean      - Average execution time
Error     - Standard error
StdDev    - Standard deviation (consistency)
Ratio     - Compared to baseline
Gen0/1/2  - Garbage collections
Allocated - Memory allocated
```

### What to Look For

1. **Large Mean differences** → Performance improvement
2. **High StdDev** → Inconsistent performance
3. **Gen 2 collections** → Potential memory issue
4. **High allocations** → GC pressure

---

## 💡 Best Practices

1. **Always Baseline**
   - Compare against known implementation
   - Use `[Benchmark(Baseline = true)]`

2. **Isolate Variables**
   - Change one thing at a time
   - Control external factors

3. **Representative Data**
   - Use production-like data sizes
   - Test edge cases

4. **Multiple Iterations**
   - Warmup to eliminate JIT
   - Average multiple runs

5. **Release Mode Only**
   - Never benchmark Debug builds
   - Optimizations matter!

---

## 🎯 Interview Questions

**Q1: Tại sao phải warmup trước khi benchmark?**

- JIT compilation xảy ra ở lần chạy đầu tiên
- Skews results nếu không warmup
- BenchmarkDotNet tự động warmup

**Q2: Debug vs Release build cho benchmarking?**

- Always Release mode
- Debug có optimizations tắt
- Results khác biệt lớn (5-10x)

**Q3: Làm sao đo memory allocations?**

- GC.GetTotalMemory()
- BenchmarkDotNet [MemoryDiagnoser]
- Visual Studio Memory Profiler

**Q4: Dead code elimination là gì?**

- Compiler loại bỏ code không sử dụng
- Return results để prevent elimination
- BenchmarkDotNet có measures để prevent

---

## 🚀 Quick Reference Commands

```bash
# Run all benchmarks
dotnet run -c Release

# Run specific benchmark
dotnet run -c Release --filter *StringBenchmark*

# Export results
dotnet run -c Release --exporters html json

# Memory diagnostics
dotnet run -c Release --memory
```

---

Happy Benchmarking! 📊🚀
