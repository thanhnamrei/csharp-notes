# Design Patterns / Các Mẫu Thiết Kế

## 📚 Tất Cả Design Patterns

Design patterns là các giải pháp tái sử dụng cho các vấn đề phổ biến trong lập trình hướng đối tượng. Chúng giúp tạo ra code dễ bảo trì, mở rộng, và tái sử dụng.

---

## 🏗️ Creational Patterns / Mẫu Tạo Tạo

Những pattern này giải quyết bài toán **tạo objects** theo cách linh hoạt và hiệu quả.

### 1. **[Singleton](./singleton.md)** ⭐

- **Mục đích:** Một class chỉ có một instance duy nhất
- **Khi dùng:** Logger, Database Connection, Configuration
- **Ưu điểm:** Tiết kiệm memory, global access
- **Nhược điểm:** Khó test, global state

### 2. **[Factory](./factory.md)** ⭐⭐

- **Mục đích:** Tạo objects mà không cần biết concrete class
- **Khi dùng:** Payment Gateways, Database Drivers, Document Types
- **Ưu điểm:** Loose coupling, dễ extend
- **Nhược điểm:** Nhiều classes, complexity

### 3. **[Builder](./builder.md)** ⭐⭐

- **Mục đích:** Xây dựng objects phức tạp từng bước
- **Khi dùng:** HTTP Requests, SQL Queries, Configuration Objects
- **Ưu điểm:** Flexible, clean interface, immutability
- **Nhược điểm:** Overhead, more code

---

## 🔄 Structural Patterns / Mẫu Cấu Trúc

Những pattern này giải quyết bài toán **tổ chức quan hệ giữa objects**.

### 4. **[Decorator](./decorator.md)** ⭐⭐

- **Mục đích:** Thêm responsibilities động vào objects
- **Khi dùng:** I/O Streams, UI Components, Text Formatting
- **Ưu điểm:** Dynamic behavior, composable, no inheritance needed
- **Nhược điểm:** Complexity, order dependent

### 5. **Adapter**

- **Mục đích:** Làm compatible hai interfaces không tương thích
- **Khi dùng:** Legacy code integration, API wrappers
- **Ưu điểm:** Reuse existing code

### 6. **Proxy**

- **Mục đích:** Control access tới một object khác
- **Khi dùng:** Lazy loading, access control, logging
- **Ưu điểm:** Control, security, lazy initialization

---

## 🎯 Behavioral Patterns / Mẫu Hành Động

Những pattern này giải quyết bài toán **giao tiếp giữa objects** và phân chia trách nhiệm.

### 7. **[Observer](./observer.md)** ⭐⭐

- **Mục đích:** Một-để-nhiều dependency giữa objects
- **Khi dùng:** Event Systems, Notifications, MVC Pattern
- **Ưu điểm:** Loose coupling, broadcast support
- **Nhược điểm:** Memory leaks, unpredictable order

### 8. **[Strategy](./strategy.md)** ⭐⭐

- **Mục đích:** Định nghĩa family của algorithms và làm chúng interchangeable
- **Khi dùng:** Sorting, Payment Methods, Discount Calculations
- **Ưu điểm:** Flexibility, no conditionals, testability
- **Nhược điểm:** More classes needed

### 9. **State**

- **Mục đích:** Object behavior thay đổi dựa trên internal state
- **Khi dùng:** State machines, Document states, Order statuses
- **Ưu điểm:** Clear state transitions, organized code

### 10. **Command**

- **Mục đích:** Encapsulate request như một object
- **Khi dùng:** Undo/Redo, Task scheduling, Macro recording
- **Ưu điểm:** Decouple sender and receiver, easy to queue

### 11. **Template Method**

- **Mục đích:** Định nghĩa skeleton của algorithm trong base class
- **Khi dùng:** Frameworks, common workflows
- **Ưu điểm:** Code reuse, consistent behavior

---

## 🌐 SOLID Principles Integration

| Principle                       | Relevant Patterns            |
| ------------------------------- | ---------------------------- |
| **SRP** (Single Responsibility) | Strategy, Observer           |
| **OCP** (Open/Closed)           | Decorator, Strategy, Factory |
| **LSP** (Liskov Substitution)   | Factory, Strategy            |
| **ISP** (Interface Segregation) | Strategy, Observer           |
| **DIP** (Dependency Inversion)  | Factory, Observer, Strategy  |

---

## 📊 Pattern Comparison Matrix

```
┌─────────────────┬──────────────┬──────────────┬─────────────┐
│ Pattern         │ Complexity   │ Flexibility  │ When to Use │
├─────────────────┼──────────────┼──────────────┼─────────────┤
│ Singleton       │ Low          │ Low          │ Global state|
│ Factory         │ Medium       │ High         │ Object creation
│ Builder         │ Medium       │ High         │ Complex objects
│ Decorator       │ Medium-High  │ High         │ Dynamic behavior
│ Observer        │ Medium       │ Medium       │ Event handling
│ Strategy        │ Medium       │ High         │ Algorithm swap
│ State           │ High         │ High         │ State machines
│ Command         │ Medium       │ Medium       │ Action as object
└─────────────────┴──────────────┴──────────────┴─────────────┘
```

---

## 🎓 Learning Roadmap

### **Week 1: Foundation**

1. Start with **Singleton** - Simplest pattern
2. Learn **Factory** - Creation logic
3. Understand **Builder** - Complex object creation

### **Week 2: Structural & Behavioral**

4. Master **Decorator** - Adding behavior dynamically
5. Study **Observer** - Event-driven systems
6. Get comfortable with **Strategy** - Algorithm selection

### **Week 3: Advanced**

7. Dive into **State** - Complex state management
8. Learn **Command** - Action encapsulation
9. Review **SOLID** integration with patterns

---

## 💡 General Guidelines

**✅ DO:**

- Use patterns to solve specific problems
- Start simple, add complexity only when needed
- Combine patterns for better solutions
- Test pattern implementations thoroughly

**❌ DON'T:**

- Force patterns where they're not needed
- Over-engineer simple code
- Mix too many patterns in one class
- Use patterns just because they exist

---

## 🚀 Real-World Applications

### E-Commerce Platform

- **Factory** - Product creation
- **Strategy** - Discount calculations
- **Observer** - Order notifications
- **Decorator** - Build customized products
- **Command** - Order processing queue

### Content Management System

- **Singleton** - Configuration, Cache
- **Factory** - Document types
- **Builder** - Page construction
- **Decorator** - Text formatting
- **Strategy** - Export formats

### Financial System

- **Observer** - Price updates
- **Strategy** - Payment methods
- **State** - Transaction status
- **Factory** - Payment gateways
- **Singleton** - Database connection

---

## 📚 References

- [Head First Design Patterns](https://www.oreilly.com/library/view/head-first-design/0596007124/)
- [Refactoring Guru - Design Patterns](https://refactoring.guru/design-patterns)
- [Gang of Four - Design Patterns Book](https://en.wikipedia.org/wiki/Design_Patterns)
- [Dependency Injection Patterns](../solid-principles/dip.md)

---

## 🎯 Interview Tips

1. **Know the problem** before mentioning the pattern name
2. **Provide real-world examples** with your code
3. **Discuss trade-offs** - no perfect pattern
4. **Show implementation** - not just theory
5. **Combine patterns** intelligently - show you understand when to mix them
6. **Compare alternatives** - explain why this pattern suits the problem

---

**Last Updated:** 2026
