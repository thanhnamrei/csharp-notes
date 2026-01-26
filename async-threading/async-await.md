# Async/Await / Bất Đồng Bộ

## 📖 Định Nghĩa / Definition

**Async/Await** là một cơ chế cho phép bạn viết code bất đồng bộ theo kiểu **synchronous**, giúp application **responsive** mà không cần **multi-threading**.

**Async/Await** is a mechanism that allows you to write asynchronous code in a **synchronous style**, keeping applications **responsive** without explicit **multi-threading**.

---

## 💡 Khái Niệm / Concepts

### **Async Keyword**

- Đánh dấu một method là asynchronous
- Cho phép sử dụng `await` bên trong

### **Await Keyword**

- Pause execution cho đến khi task hoàn thành
- Không block thread - thread quay lại thread pool

### **Task**

- Represent ongoing operation
- Task<T> để có return value

---

## 📝 Syntax & Examples

### 1. **Basic Async Method**

```csharp
// ❌ OLD - Synchronous (Blocking)
public string FetchData()
{
    using (var client = new HttpClient())
    {
        var response = client.GetStringAsync("https://api.example.com/data");
        return response; // Blocks until complete
    }
}

// ✅ NEW - Asynchronous (Non-blocking)
public async Task<string> FetchDataAsync()
{
    using (var client = new HttpClient())
    {
        string response = await client.GetStringAsync("https://api.example.com/data");
        return response;
    }
}
```

### 2. **Calling Async Methods**

```csharp
// Method 1: Await
public async Task Demo()
{
    string data = await FetchDataAsync();
    Console.WriteLine(data);
}

// Method 2: Wait (Blocking - avoid in production)
public void Demo()
{
    string data = FetchDataAsync().Result;  // ❌ Can deadlock
    Console.WriteLine(data);
}

// Method 3: Fire and forget (use with caution)
public void Demo()
{
    _ = FetchDataAsync();  // Don't wait for completion
}
```

### 3. **Task vs Task<T>**

```csharp
// Task - không return
public async Task ProcessAsync()
{
    await Task.Delay(1000);
    Console.WriteLine("Done");
}

// Task<T> - return value
public async Task<int> CalculateAsync()
{
    await Task.Delay(1000);
    return 42;
}

// Usage
await ProcessAsync();
int result = await CalculateAsync();
```

---

## 🎯 Real-World Examples

### Multiple Async Operations

```csharp
public class DataService
{
    private readonly HttpClient _httpClient;

    public DataService()
    {
        _httpClient = new HttpClient();
    }

    // Sequential - Chạy lần lượt (lâu)
    public async Task<(string users, string orders)> FetchDataSequentialAsync()
    {
        string users = await _httpClient.GetStringAsync("api/users");
        string orders = await _httpClient.GetStringAsync("api/orders");
        return (users, orders);
    }

    // Parallel - Chạy đồng thời (nhanh)
    public async Task<(string users, string orders)> FetchDataParallelAsync()
    {
        var usersTask = _httpClient.GetStringAsync("api/users");
        var ordersTask = _httpClient.GetStringAsync("api/orders");

        await Task.WhenAll(usersTask, ordersTask);

        return (usersTask.Result, ordersTask.Result);
    }
}
```

### Error Handling

```csharp
public async Task<Data> FetchWithRetryAsync(string url, int maxRetries = 3)
{
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            using (var response = await _httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<Data>();
                }
            }
        }
        catch (HttpRequestException ex)
        {
            if (i == maxRetries - 1)
                throw;

            await Task.Delay(1000 * (i + 1));  // Exponential backoff
        }
    }

    return null;
}
```

---

## ✅ Best Practices

### 1. **Naming Convention**

```csharp
// ✅ GOOD - Suffix with Async
public async Task<string> GetUserAsync(int id) { }
public async Task SaveUserAsync(User user) { }

// ❌ AVOID - No suffix
public async Task<string> GetUser(int id) { }
```

### 2. **Don't Block on Async**

```csharp
// ❌ WRONG - Can cause deadlock
public void ShowData()
{
    var data = FetchDataAsync().Result;
}

// ✅ CORRECT - Make caller async
public async Task ShowData()
{
    var data = await FetchDataAsync();
}
```

### 3. **Return Task, not void**

```csharp
// ❌ WRONG - void async (fire-and-forget)
public async void ProcessAsync()
{
    await Task.Delay(1000);
}

// ✅ CORRECT - return Task
public async Task ProcessAsync()
{
    await Task.Delay(1000);
}
```

### 4. **Avoid Nested Callbacks**

```csharp
// ❌ BAD - Callback Hell
FetchAsync().ContinueWith(t1 =>
{
    if (t1.IsCompletedSuccessfully)
    {
        ProcessAsync().ContinueWith(t2 =>
        {
            SaveAsync();
        });
    }
});

// ✅ GOOD - Async/Await
async Task Work()
{
    var data = await FetchAsync();
    await ProcessAsync(data);
    await SaveAsync(data);
}
```

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Deadlock risk
public string BadMethod()
{
    return FetchDataAsync().Result;
}

// ❌ WRONG - Fire and forget
public async void ButtonClick()
{
    await FetchDataAsync();
}

// ❌ WRONG - Not awaiting
public async Task BadAsync()
{
    FetchDataAsync();  // Missing await!
    return;
}

// ✅ CORRECT
public async Task<string> GoodMethod()
{
    return await FetchDataAsync();
}

public async Task ButtonClick()
{
    await FetchDataAsync();
}
```

---

## 🎯 Task.WhenAll vs Task.WhenAny

```csharp
// WhenAll - chờ tất cả
public async Task WhenAllExample()
{
    var task1 = FetchAsync("url1");
    var task2 = FetchAsync("url2");
    var task3 = FetchAsync("url3");

    await Task.WhenAll(task1, task2, task3);  // Tất cả phải xong
}

// WhenAny - chờ cái đầu tiên
public async Task WhenAnyExample()
{
    var task1 = FetchAsync("url1");
    var task2 = FetchAsync("url2");

    await Task.WhenAny(task1, task2);  // Cái nào xong trước là ok
}
```

---

## 🎓 Interview Questions

1. **Async/Await làm gì? Tại sao dùng?**
2. **async void có vấn đề gì?**
3. **Result property có vấn đề gì?**
4. **Async all the way up** - Điều này có nghĩa gì?
5. **Task.WhenAll vs Task.WhenAny khác gì?**

---

## 📚 Related Topics

- [Threading Concepts](threading.md)
- [Tasks](tasks.md)
- [Concurrency](concurrency.md)
