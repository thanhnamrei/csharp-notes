# 🌐 Real-World Performance Scenarios / Tối Ưu Trong Thực Tế

## 📋 Tổng Quan / Overview

Performance optimization trong các tình huống thực tế: Web APIs, Entity Framework, Microservices, và nhiều hơn nữa.

Real-world performance optimization scenarios covering Web APIs, Entity Framework, Microservices, and more.

---

## 1. ASP.NET Core Web API Performance

### ❌ Problem: Slow API Endpoint

```csharp
// BAD: Multiple database calls, no caching
[HttpGet("users/{id}")]
public async Task<IActionResult> GetUser(int id)
{
    var user = await _context.Users.FindAsync(id);
    var orders = await _context.Orders.Where(o => o.UserId == id).ToListAsync();
    var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == id);

    // Multiple queries + no caching = SLOW
    return Ok(new { user, orders, profile });
}
```

### ✅ Solution: Optimized with Caching & Single Query

```csharp
// GOOD: Single query with includes, memory caching
[HttpGet("users/{id}")]
public async Task<IActionResult> GetUser(int id)
{
    // Try cache first
    var cacheKey = $"user_{id}";
    if (_cache.TryGetValue(cacheKey, out UserDto cachedUser))
        return Ok(cachedUser);

    // Single query with eager loading
    var user = await _context.Users
        .Include(u => u.Orders)
        .Include(u => u.Profile)
        .AsNoTracking() // No change tracking overhead
        .FirstOrDefaultAsync(u => u.Id == id);

    if (user == null)
        return NotFound();

    var dto = _mapper.Map<UserDto>(user);

    // Cache for 5 minutes
    _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(5));

    return Ok(dto);
}
```

### Response Compression

```csharp
// Program.cs or Startup.cs
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});

app.UseResponseCompression();
```

### Response Caching Middleware

```csharp
// Add response caching
builder.Services.AddResponseCaching();

app.UseResponseCaching();

// In controller
[HttpGet("products")]
[ResponseCache(Duration = 60)] // Cache for 60 seconds
public async Task<IActionResult> GetProducts()
{
    return Ok(await _context.Products.ToListAsync());
}
```

### Output Caching (.NET 7+)

```csharp
// Program.cs
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(60)));
});

app.UseOutputCache();

// Controller
[HttpGet("products")]
[OutputCache(Duration = 60)]
public async Task<IActionResult> GetProducts()
{
    return Ok(await _context.Products.ToListAsync());
}
```

### Async Streams for Large Results

```csharp
// ❌ BAD: Load all in memory
[HttpGet("large-dataset")]
public async Task<IActionResult> GetLargeDataset()
{
    var data = await _context.Records.ToListAsync(); // OOM risk!
    return Ok(data);
}

// ✅ GOOD: Stream results
[HttpGet("large-dataset")]
public async IAsyncEnumerable<Record> StreamLargeDataset()
{
    await foreach (var record in _context.Records.AsAsyncEnumerable())
    {
        yield return record;
    }
}
```

---

## 2. Entity Framework Core Optimization

### N+1 Query Problem

```csharp
// ❌ BAD: N+1 queries
public async Task<List<OrderDto>> GetOrders()
{
    var orders = await _context.Orders.ToListAsync(); // 1 query

    foreach (var order in orders)
    {
        order.Customer = await _context.Customers
            .FindAsync(order.CustomerId); // N queries!
    }

    return orders;
}

// ✅ GOOD: Single query with Include
public async Task<List<OrderDto>> GetOrders()
{
    var orders = await _context.Orders
        .Include(o => o.Customer)
        .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
        .AsNoTracking()
        .ToListAsync(); // 1 query with JOINs

    return _mapper.Map<List<OrderDto>>(orders);
}
```

### Projection for Better Performance

```csharp
// ❌ BAD: Select entire entity
public async Task<List<UserSummary>> GetUserSummaries()
{
    var users = await _context.Users.ToListAsync(); // All columns!

    return users.Select(u => new UserSummary
    {
        Id = u.Id,
        Name = u.Name,
        Email = u.Email
    }).ToList();
}

// ✅ GOOD: Project only needed columns
public async Task<List<UserSummary>> GetUserSummaries()
{
    return await _context.Users
        .Select(u => new UserSummary
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        })
        .ToListAsync(); // Only SELECT id, name, email
}
```

### Batch Operations

```csharp
// ❌ BAD: Individual saves
public async Task UpdatePrices(List<Product> products)
{
    foreach (var product in products)
    {
        product.Price *= 1.1m;
        await _context.SaveChangesAsync(); // N database round-trips!
    }
}

// ✅ GOOD: Batch save
public async Task UpdatePrices(List<Product> products)
{
    foreach (var product in products)
    {
        product.Price *= 1.1m;
    }

    await _context.SaveChangesAsync(); // Single transaction!
}

// ✅ BETTER: Bulk update (with library)
public async Task UpdatePrices(List<Product> products)
{
    await _context.Products
        .Where(p => products.Select(x => x.Id).Contains(p.Id))
        .ExecuteUpdateAsync(s => s.SetProperty(
            p => p.Price,
            p => p.Price * 1.1m)); // Single UPDATE statement!
}
```

### Compiled Queries

```csharp
// Compiled query - compiled once, reused many times
private static readonly Func<MyDbContext, int, Task<User>> _getUser =
    EF.CompileAsyncQuery((MyDbContext context, int id) =>
        context.Users
            .Include(u => u.Profile)
            .FirstOrDefault(u => u.Id == id));

public async Task<User> GetUser(int id)
{
    return await _getUser(_context, id); // Faster - no query compilation!
}
```

### Split Queries for Multiple Collections

```csharp
// ❌ BAD: Cartesian explosion with multiple Includes
var blogs = await _context.Blogs
    .Include(b => b.Posts)
    .Include(b => b.Comments) // Cartesian product = HUGE result set!
    .ToListAsync();

// ✅ GOOD: Split queries
var blogs = await _context.Blogs
    .Include(b => b.Posts)
    .Include(b => b.Comments)
    .AsSplitQuery() // Multiple queries instead of JOIN
    .ToListAsync();
```

### AsNoTracking for Read-Only Queries

```csharp
// ❌ Change tracking overhead
var users = await _context.Users.ToListAsync();

// ✅ No tracking - faster for read-only
var users = await _context.Users
    .AsNoTracking()
    .ToListAsync();

// ✅ Even better: No tracking by default
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(connectionString)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
```

---

## 3. Microservices Performance

### HTTP Client Best Practices

```csharp
// ❌ BAD: Creating HttpClient per request
public async Task<string> CallService()
{
    using var client = new HttpClient(); // Socket exhaustion!
    return await client.GetStringAsync("https://api.example.com");
}

// ✅ GOOD: Use IHttpClientFactory
// Program.cs
builder.Services.AddHttpClient("MyService", client =>
{
    client.BaseAddress = new Uri("https://api.example.com");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Service
public class MyService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MyService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> CallService()
    {
        var client = _httpClientFactory.CreateClient("MyService");
        return await client.GetStringAsync("/api/data");
    }
}
```

### Retry Policies with Polly

```csharp
// Add Polly for resilience
builder.Services.AddHttpClient("MyService")
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))) // Exponential backoff
    .AddTransientHttpErrorPolicy(policy =>
        policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))); // Circuit breaker
```

### Parallel Service Calls

```csharp
// ❌ BAD: Sequential calls
public async Task<CombinedData> GetData(int id)
{
    var user = await _userService.GetUserAsync(id);       // Wait...
    var orders = await _orderService.GetOrdersAsync(id);   // Wait...
    var profile = await _profileService.GetProfileAsync(id); // Wait...

    return new CombinedData { User = user, Orders = orders, Profile = profile };
}

// ✅ GOOD: Parallel calls
public async Task<CombinedData> GetData(int id)
{
    var userTask = _userService.GetUserAsync(id);
    var ordersTask = _orderService.GetOrdersAsync(id);
    var profileTask = _profileService.GetProfileAsync(id);

    await Task.WhenAll(userTask, ordersTask, profileTask);

    return new CombinedData
    {
        User = await userTask,
        Orders = await ordersTask,
        Profile = await profileTask
    };
}
```

### Distributed Caching with Redis

```csharp
// Install: Microsoft.Extensions.Caching.StackExchangeRedis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "MyApp_";
});

// Usage
public class ProductService
{
    private readonly IDistributedCache _cache;
    private readonly MyDbContext _context;

    public async Task<Product> GetProductAsync(int id)
    {
        var cacheKey = $"product_{id}";

        // Try cache first
        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (cachedData != null)
            return JsonSerializer.Deserialize<Product>(cachedData);

        // Load from database
        var product = await _context.Products.FindAsync(id);

        // Cache for 10 minutes
        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(product),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

        return product;
    }
}
```

### Message Queue with Background Processing

```csharp
// Background service for processing queue
public class OrderProcessingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderProcessingService> _logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            var pendingOrders = await context.Orders
                .Where(o => o.Status == OrderStatus.Pending)
                .Take(10) // Process in batches
                .ToListAsync(stoppingToken);

            foreach (var order in pendingOrders)
            {
                await ProcessOrderAsync(order, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessOrderAsync(Order order, CancellationToken ct)
    {
        // Process order...
        _logger.LogInformation("Processing order {OrderId}", order.Id);
    }
}

// Register
builder.Services.AddHostedService<OrderProcessingService>();
```

---

## 4. Database Performance

### Connection Pooling

```csharp
// ✅ Connection pooling is enabled by default
var connectionString = "Server=localhost;Database=MyDb;User Id=sa;Password=***;Min Pool Size=5;Max Pool Size=100;";

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));
```

### Bulk Insert Operations

```csharp
// ❌ BAD: Insert one by one
public async Task ImportProducts(List<Product> products)
{
    foreach (var product in products)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync(); // Very slow for large datasets!
    }
}

// ✅ GOOD: Batch insert
public async Task ImportProducts(List<Product> products)
{
    _context.Products.AddRange(products);
    await _context.SaveChangesAsync(); // Much faster!
}

// ✅ BETTER: Use bulk insert library (EFCore.BulkExtensions)
public async Task ImportProducts(List<Product> products)
{
    await _context.BulkInsertAsync(products); // Fastest - uses SqlBulkCopy
}
```

### Indexing Strategy

```csharp
// Add indexes in OnModelCreating
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Single column index
    modelBuilder.Entity<Order>()
        .HasIndex(o => o.CustomerId);

    // Composite index
    modelBuilder.Entity<Order>()
        .HasIndex(o => new { o.CustomerId, o.OrderDate });

    // Unique index
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

    // Filtered index
    modelBuilder.Entity<Order>()
        .HasIndex(o => o.Status)
        .HasFilter("[Status] = 'Pending'");
}
```

### Raw SQL for Complex Queries

```csharp
// When EF query is too complex or slow, use raw SQL
public async Task<List<SalesReport>> GetSalesReport(DateTime startDate, DateTime endDate)
{
    return await _context.Database
        .SqlQueryRaw<SalesReport>(@"
            SELECT
                p.CategoryId,
                c.Name AS CategoryName,
                SUM(oi.Quantity * oi.Price) AS TotalSales,
                COUNT(DISTINCT o.Id) AS OrderCount
            FROM Orders o
            JOIN OrderItems oi ON o.Id = oi.OrderId
            JOIN Products p ON oi.ProductId = p.Id
            JOIN Categories c ON p.CategoryId = c.Id
            WHERE o.OrderDate BETWEEN @startDate AND @endDate
            GROUP BY p.CategoryId, c.Name
            ORDER BY TotalSales DESC",
            new SqlParameter("@startDate", startDate),
            new SqlParameter("@endDate", endDate))
        .ToListAsync();
}
```

---

## 5. JSON Serialization Performance

### System.Text.Json Optimization

```csharp
// ✅ Use source generation for better performance (.NET 6+)
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(List<User>))]
public partial class AppJsonContext : JsonSerializerContext
{
}

// Register in Program.cs
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonContext.Default);
});

// Usage
var json = JsonSerializer.Serialize(user, AppJsonContext.Default.User);
var user = JsonSerializer.Deserialize(json, AppJsonContext.Default.User);
```

### Ignore Null Values

```csharp
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition =
        JsonIgnoreCondition.WhenWritingNull; // Smaller JSON payload
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
```

---

## 6. Async/Await Best Practices

### Avoid Async Void

```csharp
// ❌ BAD: Async void
public async void ProcessData() // Exceptions are lost!
{
    await DoWorkAsync();
}

// ✅ GOOD: Return Task
public async Task ProcessDataAsync()
{
    await DoWorkAsync();
}
```

### ConfigureAwait in Libraries

```csharp
// ✅ Use ConfigureAwait(false) in library code
public async Task<Data> LoadDataAsync()
{
    var result = await _httpClient
        .GetStringAsync(url)
        .ConfigureAwait(false); // Don't capture context

    return ProcessData(result);
}
```

### ValueTask for Hot Paths

```csharp
// ✅ Use ValueTask when often completing synchronously
public ValueTask<int> GetCachedValueAsync(string key)
{
    if (_cache.TryGetValue(key, out int value))
        return new ValueTask<int>(value); // No allocation!

    return new ValueTask<int>(LoadFromDatabaseAsync(key));
}
```

---

## 7. Logging Performance

### Use Structured Logging

```csharp
// ❌ BAD: String interpolation
_logger.LogInformation($"User {userId} placed order {orderId}");

// ✅ GOOD: Structured logging (faster, better for log aggregation)
_logger.LogInformation("User {UserId} placed order {OrderId}", userId, orderId);
```

### Log Levels & Filtering

```csharp
// appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning", // Only log warnings and errors in production
      "Microsoft.EntityFrameworkCore": "Error" // Reduce EF logging
    }
  }
}
```

### Conditional Logging

```csharp
// ✅ Check if logging is enabled before expensive operations
if (_logger.IsEnabled(LogLevel.Debug))
{
    var expensiveData = ComputeExpensiveDebugInfo();
    _logger.LogDebug("Debug info: {Data}", expensiveData);
}
```

---

## 8. Real-World Complete Example

### High-Performance E-Commerce API

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly MyDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        MyDbContext context,
        IMemoryCache cache,
        ILogger<ProductsController> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    // GET: api/products?category=electronics&page=1&pageSize=20
    [HttpGet]
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "category", "page", "pageSize" })]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts(
        [FromQuery] string category = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        pageSize = Math.Min(pageSize, 100); // Limit max page size

        var cacheKey = $"products_{category}_{page}_{pageSize}";

        // Try memory cache
        if (_cache.TryGetValue(cacheKey, out PagedResult<ProductDto> cached))
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cached;
        }

        // Build query
        var query = _context.Products.AsNoTracking();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.Category == category);
        }

        // Get total count for pagination
        var totalCount = await query.CountAsync();

        // Projection for better performance (only select needed columns)
        var products = await query
            .OrderByDescending(p => p.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                InStock = p.StockQuantity > 0
            })
            .ToListAsync();

        var result = new PagedResult<ProductDto>
        {
            Items = products,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        // Cache for 2 minutes
        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(2));

        return result;
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    [ResponseCache(Duration = 300, VaryByHeader = "Accept-Language")]
    public async Task<ActionResult<ProductDetailDto>> GetProduct(int id)
    {
        var cacheKey = $"product_detail_{id}";

        if (_cache.TryGetValue(cacheKey, out ProductDetailDto cached))
            return cached;

        // Single query with all needed data
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Reviews.OrderByDescending(r => r.CreatedDate).Take(5))
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        var dto = new ProductDetailDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryName = product.Category.Name,
            AverageRating = product.Reviews.Any()
                ? product.Reviews.Average(r => r.Rating)
                : 0,
            RecentReviews = product.Reviews.Select(r => new ReviewDto
            {
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedDate = r.CreatedDate
            }).ToList()
        };

        _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(5));

        return dto;
    }

    // POST: api/products/search
    [HttpPost("search")]
    public async Task<ActionResult<List<ProductDto>>> SearchProducts(
        [FromBody] ProductSearchRequest request)
    {
        // Use compiled query for frequently used searches
        var query = _context.Products.AsNoTracking();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(p =>
                EF.Functions.Like(p.Name, $"%{request.SearchTerm}%") ||
                EF.Functions.Like(p.Description, $"%{request.SearchTerm}%"));
        }

        if (request.MinPrice.HasValue)
            query = query.Where(p => p.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= request.MaxPrice.Value);

        var products = await query
            .Take(50) // Limit results
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category
            })
            .ToListAsync();

        return products;
    }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
```

---

## 📊 Performance Metrics to Monitor

### Application Insights

```csharp
// Add Application Insights
builder.Services.AddApplicationInsightsTelemetry();

// Track custom metrics
_telemetryClient.TrackMetric("OrderProcessingTime", elapsedMs);
_telemetryClient.TrackEvent("HighValueOrder",
    new Dictionary<string, string> { { "OrderId", orderId.ToString() } });
```

### Health Checks

```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MyDbContext>()
    .AddRedis(Configuration["Redis:ConnectionString"])
    .AddUrlGroup(new Uri("https://api.external.com/health"), "External API");

app.MapHealthChecks("/health");
```

---

## ✅ Performance Checklist

**Web API:**

- [ ] Use response caching/output caching
- [ ] Enable response compression
- [ ] Implement pagination for large datasets
- [ ] Use async/await properly
- [ ] Return only needed data (DTOs)

**Entity Framework:**

- [ ] Use AsNoTracking for read-only queries
- [ ] Avoid N+1 queries (use Include/ThenInclude)
- [ ] Use projection (Select) instead of loading full entities
- [ ] Batch SaveChanges operations
- [ ] Use compiled queries for frequently used queries
- [ ] Add appropriate indexes

**Microservices:**

- [ ] Use IHttpClientFactory
- [ ] Implement retry policies and circuit breakers
- [ ] Call services in parallel when possible
- [ ] Use distributed caching (Redis)
- [ ] Implement background processing for long tasks

**Database:**

- [ ] Connection pooling enabled
- [ ] Proper indexing strategy
- [ ] Use bulk operations for large datasets
- [ ] Monitor query performance
- [ ] Use read replicas for reporting

**General:**

- [ ] Profile before optimizing
- [ ] Monitor with Application Insights
- [ ] Set up health checks
- [ ] Use structured logging
- [ ] Implement caching strategy

---

## 🎯 Interview Questions

**Q1: N+1 problem trong EF là gì và cách fix?**

- Problem: 1 query lấy list + N queries lấy related data
- Solution: Use Include/ThenInclude để eager load

**Q2: Khi nào dùng AsNoTracking?**

- Read-only queries
- Không cần update entities
- Better performance (no change tracking overhead)

**Q3: Làm sao tránh socket exhaustion với HttpClient?**

- Use IHttpClientFactory
- Don't create new HttpClient per request
- Reuse HttpClient instances

**Q4: Caching strategy cho microservices?**

- In-memory cache: Fast, per instance
- Distributed cache (Redis): Shared across instances
- Response caching: HTTP-level caching

**Q5: Optimize bulk insert trong EF?**

- Use AddRange + single SaveChanges
- Use bulk extensions (BulkInsert)
- Consider SqlBulkCopy for very large datasets

---

Happy Coding! 🚀🌐
