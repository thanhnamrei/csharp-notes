# 📦 Collection Performance Guide / Hướng Dẫn Hiệu Năng Collection

## 📋 Tổng Quan / Overview

Chọn đúng collection type có thể cải thiện performance lên đến 1000x!

Choosing the right collection type can improve performance up to 1000x!

---

## 1. Performance Comparison Table

| Operation     | List<T> | Dictionary<K,V> | HashSet<T> | SortedSet<T> | Queue<T> | Stack<T> |
| ------------- | ------- | --------------- | ---------- | ------------ | -------- | -------- |
| Add           | O(1)\*  | O(1)            | O(1)       | O(log n)     | O(1)     | O(1)     |
| Insert        | O(n)    | -               | -          | -            | -        | -        |
| Remove        | O(n)    | O(1)            | O(1)       | O(log n)     | -        | -        |
| Find/Contains | O(n)    | O(1)            | O(1)       | O(log n)     | O(n)     | O(n)     |
| Index access  | O(1)    | O(1) by key     | -          | -            | -        | -        |
| Enumerate     | O(n)    | O(n)            | O(n)       | O(n) sorted  | O(n)     | O(n)     |

\*O(1) amortized - occasionally O(n) when resizing

---

## 2. List<T> - Array-Based List

### Best Use Cases

```csharp
// ✅ Sequential access
var numbers = new List<int>();
for (int i = 0; i < numbers.Count; i++)
{
    Process(numbers[i]); // O(1) access
}

// ✅ Append to end
numbers.Add(42); // O(1) amortized

// ✅ When you know size upfront
var optimized = new List<int>(capacity: 1000); // No resizing!
```

### Performance Tips

```csharp
// ❌ BAD: Multiple resizing
var list = new List<int>();
for (int i = 0; i < 10000; i++)
    list.Add(i); // May resize multiple times

// ✅ GOOD: Pre-allocate capacity
var list = new List<int>(10000);
for (int i = 0; i < 10000; i++)
    list.Add(i); // No resizing

// ❌ BAD: Insert at beginning
list.Insert(0, value); // O(n) - shifts all elements

// ✅ GOOD: Use LinkedList for frequent insertions
var linked = new LinkedList<int>();
linked.AddFirst(value); // O(1)
```

### When to Avoid

```csharp
// ❌ Frequent lookups
if (list.Contains(value)) // O(n) - linear search!

// ✅ Use Dictionary or HashSet instead
if (hashSet.Contains(value)) // O(1)
```

---

## 3. Dictionary<TKey, TValue> - Hash Table

### Best Use Cases

```csharp
// ✅ Fast lookups by key
var userCache = new Dictionary<int, User>();
var user = userCache[userId]; // O(1)

// ✅ Counting/grouping
var wordCount = new Dictionary<string, int>();
foreach (var word in words)
{
    if (wordCount.ContainsKey(word))
        wordCount[word]++;
    else
        wordCount[word] = 1;
}

// Better: Use TryGetValue
foreach (var word in words)
{
    if (!wordCount.TryGetValue(word, out var count))
        count = 0;
    wordCount[word] = count + 1;
}

// Even better: CollectionsMarshal (advanced)
foreach (var word in words)
{
    ref int count = ref CollectionsMarshal.GetValueRefOrAddDefault(
        wordCount, word, out _);
    count++;
}
```

### Performance Tips

```csharp
// ✅ Pre-allocate capacity
var dict = new Dictionary<string, int>(capacity: 1000);

// ✅ Custom equality comparer for performance
var caseInsensitive = new Dictionary<string, int>(
    StringComparer.OrdinalIgnoreCase); // Faster than ToLower()

// ✅ Use TryGetValue instead of ContainsKey + []
// ❌ BAD: Two lookups
if (dict.ContainsKey(key))
    value = dict[key];

// ✅ GOOD: One lookup
if (dict.TryGetValue(key, out var value))
    ProcessValue(value);
```

### Avoid Pitfalls

```csharp
// ❌ BAD: Poor hash function
public class BadKey
{
    public override int GetHashCode() => 1; // All collisions!
}

// ✅ GOOD: Good distribution
public class GoodKey
{
    public int Id { get; set; }
    public override int GetHashCode() => Id.GetHashCode();
    public override bool Equals(object obj) =>
        obj is GoodKey other && Id == other.Id;
}
```

---

## 4. HashSet<T> - Unique Elements

### Best Use Cases

```csharp
// ✅ Remove duplicates
var uniqueItems = new HashSet<int>(listWithDuplicates);

// ✅ Fast membership test
var allowedIds = new HashSet<int> { 1, 2, 3, 5, 8, 13 };
if (allowedIds.Contains(id)) // O(1)

// ✅ Set operations
var set1 = new HashSet<int> { 1, 2, 3, 4 };
var set2 = new HashSet<int> { 3, 4, 5, 6 };

set1.UnionWith(set2);        // Union
set1.IntersectWith(set2);    // Intersection
set1.ExceptWith(set2);       // Difference
```

### Performance Comparison

```csharp
// Benchmark: Check if item exists

// ❌ List.Contains - O(n)
var list = Enumerable.Range(0, 10000).ToList();
bool exists = list.Contains(9999); // ~5000 comparisons on average

// ✅ HashSet.Contains - O(1)
var set = Enumerable.Range(0, 10000).ToHashSet();
bool exists = set.Contains(9999); // ~1 comparison

// Speed difference: 1000-5000x faster!
```

---

## 5. SortedSet<T> / SortedDictionary<K,V>

### Best Use Cases

```csharp
// ✅ Need sorted data
var sortedScores = new SortedSet<int> { 95, 87, 92, 78 };
// Always sorted: [78, 87, 92, 95]

// ✅ Range queries
var scores = new SortedSet<int> { 10, 20, 30, 40, 50 };
var range = scores.GetViewBetween(20, 40); // [20, 30, 40]

// ✅ Min/Max access
int min = sortedScores.Min; // O(1)
int max = sortedScores.Max; // O(1)
```

### When to Avoid

```csharp
// ❌ If you don't need sorting - use HashSet
// SortedSet is slower: O(log n) vs O(1)

// ❌ Frequent additions/removals
var sorted = new SortedSet<int>();
for (int i = 0; i < 10000; i++)
    sorted.Add(i); // O(log n) each - slower than HashSet

// ✅ Better: Add all, then sort once
var list = new List<int>();
for (int i = 0; i < 10000; i++)
    list.Add(i);
list.Sort(); // O(n log n) once
```

---

## 6. Queue<T> - FIFO

### Best Use Cases

```csharp
// ✅ First-In-First-Out processing
var taskQueue = new Queue<Task>();

taskQueue.Enqueue(task1);
taskQueue.Enqueue(task2);

while (taskQueue.Count > 0)
{
    var task = taskQueue.Dequeue(); // O(1)
    ProcessTask(task);
}

// ✅ Breadth-First Search
public void BFS(Node root)
{
    var queue = new Queue<Node>();
    queue.Enqueue(root);

    while (queue.Count > 0)
    {
        var node = queue.Dequeue();
        ProcessNode(node);

        foreach (var child in node.Children)
            queue.Enqueue(child);
    }
}
```

---

## 7. Stack<T> - LIFO

### Best Use Cases

```csharp
// ✅ Last-In-First-Out processing
var stack = new Stack<int>();
stack.Push(1);
stack.Push(2);
stack.Push(3);
Console.WriteLine(stack.Pop()); // 3

// ✅ Depth-First Search
public void DFS(Node root)
{
    var stack = new Stack<Node>();
    stack.Push(root);

    while (stack.Count > 0)
    {
        var node = stack.Pop();
        ProcessNode(node);

        foreach (var child in node.Children)
            stack.Push(child);
    }
}

// ✅ Undo/Redo functionality
var undoStack = new Stack<Action>();
undoStack.Push(() => RestoreState(previousState));
```

---

## 8. LinkedList<T> - Doubly Linked List

### Best Use Cases

```csharp
// ✅ Frequent insertions/deletions in middle
var list = new LinkedList<int>();
var node = list.AddLast(2);
list.AddBefore(node, 1); // O(1)
list.AddAfter(node, 3);  // O(1)

// ✅ LRU Cache implementation
public class LRUCache<TKey, TValue>
{
    private readonly Dictionary<TKey, LinkedListNode<(TKey, TValue)>> _cache;
    private readonly LinkedList<(TKey Key, TValue Value)> _lru;
    private readonly int _capacity;

    public LRUCache(int capacity)
    {
        _capacity = capacity;
        _cache = new Dictionary<TKey, LinkedListNode<(TKey, TValue)>>(capacity);
        _lru = new LinkedList<(TKey, TValue)>();
    }

    public TValue Get(TKey key)
    {
        if (!_cache.TryGetValue(key, out var node))
            throw new KeyNotFoundException();

        // Move to front (most recently used)
        _lru.Remove(node);
        _lru.AddFirst(node);

        return node.Value.Value;
    }

    public void Put(TKey key, TValue value)
    {
        if (_cache.TryGetValue(key, out var node))
        {
            _lru.Remove(node);
        }
        else if (_cache.Count >= _capacity)
        {
            // Remove least recently used
            var lru = _lru.Last;
            _lru.RemoveLast();
            _cache.Remove(lru.Value.Key);
        }

        var newNode = _lru.AddFirst((key, value));
        _cache[key] = newNode;
    }
}
```

### When to Avoid

```csharp
// ❌ Random access by index
for (int i = 0; i < linkedList.Count; i++)
{
    var item = linkedList.ElementAt(i); // O(n) each time!
}

// ✅ Use List<T> instead for index access
for (int i = 0; i < list.Count; i++)
{
    var item = list[i]; // O(1)
}
```

---

## 9. ConcurrentDictionary<K,V> - Thread-Safe

### Best Use Cases

```csharp
// ✅ Multi-threaded scenarios
var cache = new ConcurrentDictionary<string, Data>();

Parallel.ForEach(items, item =>
{
    cache.TryAdd(item.Key, item.Data); // Thread-safe
});

// ✅ GetOrAdd pattern
var value = cache.GetOrAdd(key, k => ExpensiveOperation(k));

// ✅ Update with function
cache.AddOrUpdate(
    key,
    addValue: new Data(),
    updateValueFactory: (k, old) => UpdateData(old));
```

### Performance Considerations

```csharp
// ❌ Don't use for single-threaded code
// ConcurrentDictionary has overhead

// Single-threaded
var dict = new Dictionary<string, int>(); // Faster

// Multi-threaded
var concurrent = new ConcurrentDictionary<string, int>(); // Necessary
```

---

## 10. ImmutableCollections - Immutable Data

### Best Use Cases

```csharp
using System.Collections.Immutable;

// ✅ Functional programming
var original = ImmutableList.Create(1, 2, 3);
var modified = original.Add(4); // Returns new list
// original is unchanged!

// ✅ Thread-safe sharing
public class SharedState
{
    private ImmutableDictionary<string, int> _data =
        ImmutableDictionary<string, int>.Empty;

    public void Update(string key, int value)
    {
        // Atomic update
        ImmutableInterlocked.AddOrUpdate(
            ref _data, key, value, (k, old) => value);
    }
}

// ✅ Snapshots
var snapshot = _dictionary.ToImmutableDictionary();
```

### Performance Notes

```csharp
// ⚠️ Each modification creates new structure
// Use Builder for multiple operations

// ❌ Slow: Multiple modifications
var list = ImmutableList<int>.Empty;
for (int i = 0; i < 1000; i++)
    list = list.Add(i); // Creates 1000 lists!

// ✅ Fast: Use Builder
var builder = ImmutableList.CreateBuilder<int>();
for (int i = 0; i < 1000; i++)
    builder.Add(i);
var list = builder.ToImmutable();
```

---

## 11. Frozen Collections (.NET 8+)

### Ultimate Read Performance

```csharp
using System.Collections.Frozen;

// Build once, read many times
var config = new Dictionary<string, string>
{
    ["Host"] = "localhost",
    ["Port"] = "8080",
    ["Timeout"] = "30"
}.ToFrozenDictionary();

// Lookups are 2-4x faster than regular Dictionary!
var host = config["Host"];

// Also available:
var frozenSet = data.ToFrozenSet();
```

### When to Use

```csharp
// ✅ Configuration data
// ✅ Lookup tables
// ✅ Read-heavy workloads
// ✅ No modifications after creation

// ❌ Don't use if data changes
// ❌ Small collections (overhead not worth it)
```

---

## 12. Real-World Performance Comparison

### Scenario: Find Items in Collection

```csharp
[MemoryDiagnoser]
public class LookupBenchmark
{
    private List<int> _list;
    private HashSet<int> _hashSet;
    private Dictionary<int, int> _dict;

    [Params(100, 1000, 10000)]
    public int Size;

    [GlobalSetup]
    public void Setup()
    {
        var data = Enumerable.Range(0, Size).ToArray();
        _list = new List<int>(data);
        _hashSet = new HashSet<int>(data);
        _dict = data.ToDictionary(x => x);
    }

    [Benchmark]
    public bool List_Contains()
    {
        return _list.Contains(Size - 1); // Worst case
    }

    [Benchmark]
    public bool HashSet_Contains()
    {
        return _hashSet.Contains(Size - 1);
    }

    [Benchmark]
    public bool Dictionary_ContainsKey()
    {
        return _dict.ContainsKey(Size - 1);
    }
}

// Results (Size = 10,000):
// List_Contains:          ~50 μs
// HashSet_Contains:       ~0.05 μs  (1000x faster!)
// Dictionary_ContainsKey: ~0.05 μs  (1000x faster!)
```

---

## 📊 Collection Selection Guide

```
Need to...
├─ Store unique items?
│  ├─ Need sorting? → SortedSet<T>
│  └─ No sorting? → HashSet<T>
│
├─ Look up by key?
│  ├─ Need sorting? → SortedDictionary<K,V>
│  ├─ Thread-safe? → ConcurrentDictionary<K,V>
│  ├─ Read-only? → FrozenDictionary<K,V> (.NET 8+)
│  └─ Default → Dictionary<K,V>
│
├─ Sequential access?
│  ├─ Frequent insert/delete middle? → LinkedList<T>
│  └─ Default → List<T>
│
├─ FIFO? → Queue<T>
├─ LIFO? → Stack<T>
└─ Immutable? → ImmutableList/Dictionary/Set<T>
```

---

## ✅ Performance Best Practices

- [ ] Choose Dictionary/HashSet cho O(1) lookups
- [ ] Pre-allocate capacity khi biết size
- [ ] Use TryGetValue thay vì ContainsKey + []
- [ ] Tránh List.Contains trong large collections
- [ ] Use appropriate equality comparer
- [ ] Consider Frozen collections cho read-only data
- [ ] Use struct keys trong Dictionary khi possible
- [ ] Avoid LINQ trong hot paths với collections
- [ ] Use ConcurrentCollection chỉ khi cần thread-safety
- [ ] Profile trước khi optimize!

---

## 🎯 Interview Questions

**Q1: Khi nào dùng List vs HashSet?**

- List: Sequential access, index access, order matters
- HashSet: Fast lookups, unique items, O(1) contains

**Q2: Dictionary vs SortedDictionary?**

- Dictionary: O(1) lookup, unsorted
- SortedDictionary: O(log n) lookup, always sorted

**Q3: Tại sao Dictionary nhanh hơn List.Find()?**

- Dictionary: Hash table, O(1) lookup
- List.Find: Linear search, O(n)

**Q4: Khi nào dùng Queue vs Stack?**

- Queue: FIFO (BFS, task queue)
- Stack: LIFO (DFS, undo/redo)

---

Happy Coding! 📦🚀
