# 🗑️ Garbage Collection Optimization / Tối Ưu Garbage Collection

## 📋 Tổng Quan / Overview

Garbage Collection (GC) trong C# tự động quản lý memory, nhưng hiểu cách nó hoạt động giúp optimize performance.

Garbage Collection in C# automatically manages memory, but understanding how it works helps optimize performance.

---

## 1. GC Generations

### Generational Hypothesis

```
Gen 0: Young objects (short-lived)
       ↓ Survives collection
Gen 1: Middle-aged objects
       ↓ Survives collection
Gen 2: Old objects (long-lived) + Large Object Heap (LOH)
```

### Generation Collection Frequency

```csharp
// Gen 0: Most frequent (milliseconds)
// Gen 1: Less frequent (seconds)
// Gen 2: Least frequent (minutes)

// Check collection counts
Console.WriteLine($"Gen 0: {GC.CollectionCount(0)}");
Console.WriteLine($"Gen 1: {GC.CollectionCount(1)}");
Console.WriteLine($"Gen 2: {GC.CollectionCount(2)}");
```

---

## 2. How GC Works

### Mark & Compact

```
1. Mark Phase:
   - Find all reachable objects from roots
   - Mark them as "alive"

2. Compact Phase:
   - Move alive objects together
   - Update references
   - Free remaining memory
```

### Roots

```csharp
// GC Roots:
// 1. Static fields
private static List<object> _cache;

// 2. Local variables on stack
public void Method()
{
    var obj = new object(); // Root while in scope
}

// 3. CPU registers
// 4. GC handles
// 5. Finalization queue
```

---

## 3. GC Modes

### Workstation GC (Default)

```xml
<!-- App.config or .csproj -->
<PropertyGroup>
  <ServerGarbageCollection>false</ServerGarbageCollection>
</PropertyGroup>
```

**Characteristics:**

- Lower memory footprint
- Shorter pause times
- Better for client apps

### Server GC

```xml
<PropertyGroup>
  <ServerGarbageCollection>true</ServerGarbageCollection>
</PropertyGroup>
```

**Characteristics:**

- Higher throughput
- Multiple GC threads
- Better for server apps
- More memory usage

---

## 4. Reducing GC Pressure

### ❌ BAD: Frequent Allocations

```csharp
public void ProcessData()
{
    for (int i = 0; i < 1000000; i++)
    {
        var buffer = new byte[1024]; // 1M allocations!
        ProcessBuffer(buffer);
        // buffer becomes garbage
    }
}
```

### ✅ GOOD: Object Pooling

```csharp
public void ProcessData()
{
    var pool = ArrayPool<byte>.Shared;
    byte[] buffer = pool.Rent(1024);

    try
    {
        for (int i = 0; i < 1000000; i++)
        {
            ProcessBuffer(buffer);
            Array.Clear(buffer, 0, buffer.Length);
        }
    }
    finally
    {
        pool.Return(buffer);
    }
}
```

---

## 5. Large Object Heap (LOH)

### When Objects Go to LOH

```csharp
// Objects ≥ 85,000 bytes go to LOH
byte[] small = new byte[84999];  // Gen 0/1/2
byte[] large = new byte[85000];  // LOH (Gen 2)

// Arrays of reference types
object[] largeArray = new object[10000]; // Likely LOH
```

### LOH Issues

```csharp
// Problem: LOH is not compacted by default
// → Fragmentation over time

// Example of fragmentation
byte[] a = new byte[100_000]; // LOH
byte[] b = new byte[100_000]; // LOH
a = null; // Hole in LOH
byte[] c = new byte[150_000]; // Can't fit in hole → fragmentation
```

### Solution: Compact LOH

```csharp
// .NET Core 2.0+: Request LOH compaction
GCSettings.LargeObjectHeapCompactionMode =
    GCLargeObjectHeapCompactionMode.CompactOnce;

GC.Collect();
```

---

## 6. Weak References

### Normal Reference (Strong)

```csharp
// Strong reference - prevents GC
var data = new LargeObject();
_cache.Add(data); // Data won't be collected
```

### Weak Reference

```csharp
// Weak reference - allows GC
public class CacheManager
{
    private WeakReference<LargeObject> _cachedData;

    public void Cache(LargeObject data)
    {
        _cachedData = new WeakReference<LargeObject>(data);
    }

    public LargeObject TryGetCached()
    {
        if (_cachedData != null &&
            _cachedData.TryGetTarget(out var data))
        {
            return data; // Still alive
        }

        return null; // Was collected
    }
}

// Use case: Caching that doesn't prevent GC
```

---

## 7. Finalization & IDisposable

### Finalizers (Avoid if Possible!)

```csharp
// ❌ BAD: Finalizer adds overhead
public class ResourceHolder
{
    private IntPtr _handle;

    ~ResourceHolder() // Finalizer
    {
        CloseHandle(_handle);
    }
}

// Problems:
// 1. Objects live longer (promoted to Gen 2)
// 2. Finalizer queue processing overhead
// 3. Non-deterministic cleanup
```

### IDisposable Pattern

```csharp
// ✅ GOOD: Explicit cleanup
public class ResourceHolder : IDisposable
{
    private IntPtr _handle;
    private bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // No finalizer needed
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
            }

            // Free unmanaged resources
            CloseHandle(_handle);
            _disposed = true;
        }
    }

    ~ResourceHolder()
    {
        Dispose(false);
    }
}

// Usage
using (var resource = new ResourceHolder())
{
    // Use resource
} // Dispose called automatically
```

---

## 8. GC Notifications

### Subscribe to GC Events

```csharp
public class GCMonitor
{
    public void StartMonitoring()
    {
        GC.RegisterForFullGCNotification(10, 10);

        Task.Run(() =>
        {
            while (true)
            {
                GCNotificationStatus status = GC.WaitForFullGCApproach();

                if (status == GCNotificationStatus.Succeeded)
                {
                    Console.WriteLine("Full GC approaching!");
                    // Take action: reduce load, cache cleanup, etc.
                }

                status = GC.WaitForFullGCComplete();

                if (status == GCNotificationStatus.Succeeded)
                {
                    Console.WriteLine("Full GC completed!");
                }
            }
        });
    }
}
```

---

## 9. Manual GC Control (Use Sparingly!)

### Force Collection

```csharp
// ❌ Usually not recommended
GC.Collect(); // Collects all generations

// Specific generation
GC.Collect(0); // Gen 0 only
GC.Collect(1); // Gen 0 + Gen 1
GC.Collect(2, GCCollectionMode.Forced);

// Wait for finalizers
GC.WaitForPendingFinalizers();
```

### When Manual GC Makes Sense

```csharp
// 1. After large batch operation
public void ProcessLargeBatch()
{
    var largeData = LoadHugeDataset();
    ProcessData(largeData);
    largeData = null;

    // Clean up immediately
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();
}

// 2. Before performance-critical section
public void PrepareForCriticalPath()
{
    GC.Collect(); // Clean slate

    // Now run critical code with minimal GC interruption
    RunCriticalOperation();
}

// 3. In low-priority background tasks
public async Task CleanupBackgroundTask()
{
    await Task.Delay(60000); // Wait 1 minute
    GC.Collect(2, GCCollectionMode.Optimized);
}
```

---

## 10. GC Performance Tips

### Reduce Allocations

```csharp
// ❌ BAD: Many small allocations
public string BuildString(int count)
{
    string result = "";
    for (int i = 0; i < count; i++)
        result += i.ToString(); // Each iteration allocates
    return result;
}

// ✅ GOOD: Single allocation
public string BuildString(int count)
{
    var sb = new StringBuilder(count * 4);
    for (int i = 0; i < count; i++)
        sb.Append(i);
    return sb.ToString();
}
```

### Use Structs for Small Data

```csharp
// ✅ Value type - no GC pressure
public struct Point
{
    public int X;
    public int Y;
}

var points = new Point[1000]; // Single allocation, no individual objects
```

### Avoid Boxing

```csharp
// ❌ BAD: Boxing
int value = 42;
object boxed = value; // Allocates on heap!

// ✅ GOOD: Generics
public T Max<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) > 0 ? a : b;
}
```

---

## 11. Memory Leaks in .NET

### Common Causes

```csharp
// 1. Event handlers not unsubscribed
public class Publisher
{
    public event EventHandler DataChanged;
}

public class Subscriber
{
    public Subscriber(Publisher publisher)
    {
        publisher.DataChanged += OnDataChanged; // LEAK if not unsubscribed!
    }

    private void OnDataChanged(object sender, EventArgs e) { }
}

// ✅ Fix: Unsubscribe
public class Subscriber : IDisposable
{
    private Publisher _publisher;

    public Subscriber(Publisher publisher)
    {
        _publisher = publisher;
        _publisher.DataChanged += OnDataChanged;
    }

    public void Dispose()
    {
        _publisher.DataChanged -= OnDataChanged;
    }

    private void OnDataChanged(object sender, EventArgs e) { }
}

// 2. Static collections
private static List<object> _cache = new(); // Never collected!

// 3. Timers not disposed
var timer = new System.Timers.Timer(1000);
timer.Elapsed += OnTimerElapsed;
timer.Start();
// Must call timer.Dispose() to release resources!
```

---

## 12. GC Tuning (.NET Core 3.0+)

### Configuration Options

```json
// runtimeconfig.json
{
  "runtimeOptions": {
    "configProperties": {
      "System.GC.Server": true,
      "System.GC.Concurrent": true,
      "System.GC.RetainVM": true,
      "System.GC.HeapCount": 4,
      "System.GC.HeapAffinitizeMask": "0xFF"
    }
  }
}
```

### Environment Variables

```bash
# Server GC
export COMPlus_gcServer=1

# Concurrent GC
export COMPlus_gcConcurrent=1

# GC latency mode
export COMPlus_GCLatencyMode=2
```

---

## 13. Profiling GC

### Using PerfView

```bash
# Collect GC data
PerfView.exe /GCCollectOnly collect myapp.exe

# Analyze:
# - GC pause times
# - Allocation rates
# - Survival rates
# - LOH usage
```

### Visual Studio Diagnostic Tools

```
1. Debug → Performance Profiler
2. Select ".NET Object Allocation Tracking"
3. Run application
4. Analyze:
   - Allocation call tree
   - Objects by size
   - Allocation timeline
```

---

## 14. GC Latency Modes

### Set Latency Mode

```csharp
// For interactive applications
GCSettings.LatencyMode = GCLatencyMode.Interactive;

// For batch processing
GCSettings.LatencyMode = GCLatencyMode.Batch;

// For sustained low latency (no Gen 2)
GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

// No GC regions (.NET Core 2.0+)
if (GC.TryStartNoGCRegion(10_000_000)) // 10MB
{
    try
    {
        // Critical code - no GC will occur
        PerformCriticalOperation();
    }
    finally
    {
        GC.EndNoGCRegion();
    }
}
```

---

## 📊 GC Performance Metrics

### Key Metrics to Monitor

| Metric            | Good         | Bad       |
| ----------------- | ------------ | --------- |
| Gen 0 collections | High         | Very high |
| Gen 2 collections | Rare         | Frequent  |
| GC pause time     | < 10ms       | > 100ms   |
| Allocation rate   | Steady       | Spiking   |
| LOH size          | Small/stable | Growing   |
| Promoted memory   | Low          | High      |

---

## ✅ GC Optimization Checklist

- [ ] Use object pooling for frequent allocations
- [ ] Avoid LOH allocations when possible
- [ ] Implement IDisposable for unmanaged resources
- [ ] Unsubscribe from events
- [ ] Use value types for small data
- [ ] Avoid boxing in hot paths
- [ ] Pre-allocate collection capacity
- [ ] Profile allocation patterns
- [ ] Monitor Gen 2 collection frequency
- [ ] Consider Server GC for server apps
- [ ] Use `GC.SuppressFinalize()` when disposing
- [ ] Avoid manual GC.Collect() unless necessary

---

## 🎯 Interview Questions

**Q1: Giải thích 3 generations của GC?**

- Gen 0: Short-lived objects, collected frequently
- Gen 1: Medium-lived, buffer between Gen 0 and Gen 2
- Gen 2: Long-lived objects + LOH, collected rarely

**Q2: Khi nào object được promote lên generation tiếp theo?**

- Object survives một GC collection
- Gen 0 → Gen 1 → Gen 2

**Q3: Large Object Heap khác gì normal heap?**

- Objects ≥ 85KB
- Part of Gen 2
- Not compacted by default (fragmentation risk)
- Can request compaction in .NET Core 2.0+

**Q4: Tại sao finalizer làm chậm performance?**

- Object được promote lên Gen 2
- Phải chờ finalizer thread execute
- Increases GC pressure
- Use IDisposable thay vì finalizer

**Q5: Khi nào nên gọi GC.Collect()?**

- After large batch operations
- Before performance-critical sections
- In low-priority background tasks
- Almost never in production code!

**Q6: Weak reference dùng khi nào?**

- Caching mà không muốn prevent GC
- Allow GC to collect nếu memory pressure
- Framework caches, image caches

---

## 💡 Best Practices Summary

```csharp
// 1. Reduce allocations
var pool = ArrayPool<byte>.Shared;

// 2. Use structs for small data
public readonly struct Point { public int X, Y; }

// 3. Implement IDisposable properly
public void Dispose()
{
    Dispose(true);
    GC.SuppressFinalize(this);
}

// 4. Unsubscribe from events
publisher.Event -= Handler;

// 5. Avoid LOH when possible
// Use multiple small arrays instead of one large array

// 6. Profile first, optimize second
// Use profilers to find real bottlenecks
```

---

Happy Optimizing! 🗑️🚀
