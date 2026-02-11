# Observer Pattern / Mẫu Người Quan Sát

## 📖 Định Nghĩa / Definition

**Observer** là design pattern định nghĩa **một-để-nhiều dependency** giữa objects sao cho khi một object thay đổi state, tất cả dependents được **thông báo tự động**.

**Observer** is a design pattern that defines a **one-to-many dependency** between objects so that when one object changes state, all its dependents are **notified automatically**.

---

## 💡 Khi Nào Dùng / When to Use

✅ Event Handling - UI button click, form changes...
✅ Real-time Notifications - Stock prices, weather updates
✅ MVC Pattern - Model changes, Views update
✅ Pub/Sub Systems - Message queues, Event buses
✅ Reactive Programming - RxJS, RxJava equivalents

---

## 📝 Implementations / Các Cách Implement

### 1. **Classic Observer Pattern**

```csharp
// Subject (Observable)
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

// Observer (Listener)
public interface IObserver
{
    void Update(ISubject subject);
}

// Concrete Subject
public class Stock : ISubject
{
    private decimal _price;
    private List<IObserver> _observers = new();

    public decimal Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                _price = value;
                Notify();
            }
        }
    }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }
}

// Concrete Observers
public class StockDisplay : IObserver
{
    private Stock _stock;

    public StockDisplay(Stock stock)
    {
        _stock = stock;
    }

    public void Update(ISubject subject)
    {
        if (subject is Stock stock)
        {
            Console.WriteLine($"StockDisplay: Price is now ${stock.Price}");
        }
    }
}

public class PortfolioManager : IObserver
{
    public void Update(ISubject subject)
    {
        if (subject is Stock stock)
        {
            Console.WriteLine($"PortfolioManager: Recalculating portfolio with new price ${stock.Price}");
        }
    }
}

// Usage
var stock = new Stock();
var display = new StockDisplay(stock);
var manager = new PortfolioManager();

stock.Attach(display);
stock.Attach(manager);

stock.Price = 100; // Notify all observers
stock.Price = 105;
```

### 2. **C# Event-Based Observer (Modern Approach)**

```csharp
// Define custom EventArgs
public class PriceChangedEventArgs : EventArgs
{
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
}

// Subject with events
public class Stock
{
    private decimal _price;

    // Define event delegate
    public event EventHandler<PriceChangedEventArgs> PriceChanged;

    public decimal Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                var oldPrice = _price;
                _price = value;
                OnPriceChanged(oldPrice, value);
            }
        }
    }

    protected virtual void OnPriceChanged(decimal oldPrice, decimal newPrice)
    {
        PriceChanged?.Invoke(this, new PriceChangedEventArgs
        {
            OldPrice = oldPrice,
            NewPrice = newPrice
        });
    }
}

// Observers (Subscribers)
public class StockDisplay
{
    public void OnPriceChanged(object sender, PriceChangedEventArgs e)
    {
        Console.WriteLine($"Price changed from ${e.OldPrice} to ${e.NewPrice}");
    }
}

// Usage
var stock = new Stock();
var display = new StockDisplay();

stock.PriceChanged += display.OnPriceChanged;

stock.Price = 100; // Notify all subscribers
stock.Price = 105;
```

### 3. **Weak Event Pattern (Prevent Memory Leaks)**

```csharp
public interface IWeakEventListener
{
    bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e);
}

// Usage with WeakEventManager
public class WeakEventObserver : IWeakEventListener
{
    public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
        Console.WriteLine("Observer received event without strong reference");
        return true;
    }
}
```

---

## 🎯 Real-World Example / Ví Dụ Thực Tế

### **Real-time Stock Market Notification System**

```csharp
public class StockPriceChangedEventArgs : EventArgs
{
    public string Symbol { get; set; }
    public decimal PreviousPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime ChangedAt { get; set; }
}

// Subject
public class StockMarket
{
    private Dictionary<string, decimal> _stocks = new();

    public event EventHandler<StockPriceChangedEventArgs> StockPriceChanged;

    public void UpdateStockPrice(string symbol, decimal newPrice)
    {
        if (_stocks.TryGetValue(symbol, out var oldPrice))
        {
            if (oldPrice != newPrice)
            {
                _stocks[symbol] = newPrice;

                StockPriceChanged?.Invoke(this, new StockPriceChangedEventArgs
                {
                    Symbol = symbol,
                    PreviousPrice = oldPrice,
                    CurrentPrice = newPrice,
                    ChangedAt = DateTime.Now
                });
            }
        }
        else
        {
            _stocks[symbol] = newPrice;
        }
    }
}

// Observer 1: Email Notifier
public class EmailNotifier
{
    public void OnStockPriceChanged(object sender, StockPriceChangedEventArgs e)
    {
        Console.WriteLine($"📧 Email: Stock {e.Symbol} changed from ${e.PreviousPrice} to ${e.CurrentPrice}");
        // Send email in real application
    }
}

// Observer 2: SMS Notifier
public class SmsNotifier
{
    private decimal _threshold;

    public SmsNotifier(decimal threshold)
    {
        _threshold = threshold;
    }

    public void OnStockPriceChanged(object sender, StockPriceChangedEventArgs e)
    {
        var percentChange = Math.Abs((e.CurrentPrice - e.PreviousPrice) / e.PreviousPrice * 100);

        if (percentChange > _threshold)
        {
            Console.WriteLine($"📱 SMS: ALERT! {e.Symbol} changed by {percentChange:F2}%");
        }
    }
}

// Observer 3: Database Logger
public class DatabaseLogger
{
    public void OnStockPriceChanged(object sender, StockPriceChangedEventArgs e)
    {
        Console.WriteLine($"💾 DB: Logging price change for {e.Symbol} at {e.ChangedAt}");
        // Log to database
    }
}

// Observer 4: UI Update
public class UIUpdater
{
    public void OnStockPriceChanged(object sender, StockPriceChangedEventArgs e)
    {
        var change = e.CurrentPrice - e.PreviousPrice;
        var emoji = change > 0 ? "📈" : "📉";
        Console.WriteLine($"{emoji} UI: {e.Symbol} = ${e.CurrentPrice}");
    }
}

// Usage
public class Application
{
    public static void Main()
    {
        var market = new StockMarket();

        // Subscribe observers
        var emailNotifier = new EmailNotifier();
        var smsNotifier = new SmsNotifier(threshold: 2.0m);
        var dbLogger = new DatabaseLogger();
        var uiUpdater = new UIUpdater();

        market.StockPriceChanged += emailNotifier.OnStockPriceChanged;
        market.StockPriceChanged += smsNotifier.OnStockPriceChanged;
        market.StockPriceChanged += dbLogger.OnStockPriceChanged;
        market.StockPriceChanged += uiUpdater.OnStockPriceChanged;

        // Stock prices change
        market.UpdateStockPrice("AAPL", 150.00m);
        market.UpdateStockPrice("AAPL", 153.50m); // 2.33% change - triggers SMS
        market.UpdateStockPrice("GOOGL", 2800.00m);

        Console.WriteLine("\n--- Unsubscribing SMS Notifier ---\n");
        market.StockPriceChanged -= smsNotifier.OnStockPriceChanged;

        market.UpdateStockPrice("AAPL", 160.00m); // Only 4 notifications instead of 5
    }
}
```

---

## ✅ Advantages / Lợi Ích

| Lợi Ích                   | Mô Tả                                    |
| ------------------------- | ---------------------------------------- |
| **Loose Coupling**        | Subject không cần biết chi tiết Observer |
| **Dynamic Subscription**  | Subscribe/Unsubscribe lúc runtime        |
| **Broadcast Support**     | Một subject có nhiều observers           |
| **Separation of Concern** | Mỗi observer xử lý logic riêng           |
| **Easy to Extend**        | Thêm observer mới dễ dàng                |

---

## ❌ Disadvantages / Nhược Điểm

| Nhược Điểm              | Mô Tả                              |
| ----------------------- | ---------------------------------- |
| **Memory Leaks**        | Forgotten unsubscribe              |
| **Performance**         | Nhiều observers = chậm             |
| **Difficult to Debug**  | Order of notification không jelas  |
| **Unpredictable Order** | Không biết observer nào chạy trước |

---

## 🔴 Common Mistakes

```csharp
// ❌ WRONG - Memory leak when unsubscribing
public class MyApp
{
    private Stock _stock = new Stock();

    public MyApp()
    {
        _stock.PriceChanged += OnPriceChanged; // Forgot to unsubscribe!
    }

    private void OnPriceChanged(object sender, EventArgs e)
    {
        // Handle
    }

    public void Cleanup()
    {
        // Need to unsubscribe
        _stock.PriceChanged -= OnPriceChanged;
    }
}

// ✅ CORRECT
public class MyApp : IDisposable
{
    private Stock _stock = new Stock();

    public MyApp()
    {
        _stock.PriceChanged += OnPriceChanged;
    }

    public void Dispose()
    {
        _stock.PriceChanged -= OnPriceChanged;
    }

    private void OnPriceChanged(object sender, EventArgs e)
    {
        // Handle
    }
}
```

---

## 📚 Related Topics

- [Pub/Sub Pattern](../async-threading/)
- [Events in C#](../delegates-events/)
- [Reactive Programming](../async-threading/)

---

## 🎓 Interview Questions

1. **Observer pattern là gì? Sử dụng ở đâu?**
2. **Observer vs Event trong C#?**
3. **Cách ngăn memory leak trong Observer?**
4. **Weak Event Pattern là gì?**
5. **Observer vs Pub/Sub - Khác gì?**
6. **Nếu có 1000 observers, có vấn đề gì?**
