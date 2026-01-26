# 📚 Lộ Trình Ôn Tập C# / C# Study Plan

Kế hoạch ôn tập toàn diện cho phỏng vấn C# theo thứ tự ưu tiên.

Comprehensive C# study plan for technical interview preparation in priority order.

---

## 📋 Giai Đoạn Ôn Tập / Study Phases

### **Phase 1: Foundation (Nền Tảng) - Tuần 1 / Week 1**

**Mục tiêu:** Nắm vững kiến thức cơ bản về OOP và code structure.

**Objectives:** Master fundamental OOP concepts and code structure.

#### 1. [oop/](oop/) - Lập Trình Hướng Đối Tượng / Object-Oriented Programming

- [ ] Inheritance / Kế thừa
- [ ] Polymorphism / Đa hình
- [ ] Encapsulation / Đóng gói
- [ ] Abstraction / Trừu tượng hóa

**Tài liệu:** Tạo file: `inheritance.md`, `polymorphism.md`, `encapsulation.md`, `abstraction.md`

#### 2. [best-practices/](best-practices/) - Các Tiêu Chuẩn Tốt Nhất / Best Practices

- [ ] Naming Conventions / Quy ước đặt tên
- [ ] Code Structure / Cấu trúc mã

**Tài liệu:** Tạo file: `naming-conventions.md`, `code-structure.md`

#### 3. [snippets/](snippets/) - Đoạn Mã Mẫu / Code Snippets

- [ ] Thực hành viết lại các ví dụ / Practice rewriting examples
- [ ] Chạy code và test / Run and test code

---

### **Phase 2: Core Concepts - Tuần 2 / Week 2**

**Mục tiêu:** Hiểu sâu về SOLID, Design Patterns, và xử lý exception.

**Objectives:** Deep dive into SOLID, Design Patterns, and Exception Handling.

#### 4. [solid-principles/](solid-principles/) - Nguyên Tắc SOLID ⭐⭐⭐

- [ ] Single Responsibility Principle / Nguyên tắc Trách Nhiệm Đơn
- [ ] Open/Closed Principle / Nguyên tắc Mở/Đóng
- [ ] Liskov Substitution Principle / Nguyên tắc Thay Thế Liskov
- [ ] Interface Segregation Principle / Nguyên tắc Phân Chia Giao Diện
- [ ] Dependency Inversion Principle / Nguyên tắc Đảo Ngược Phụ Thuộc

**Lưu ý:** Chủ đề này được hỏi **RẤT NHIỀU** trong phỏng vấn! / **HEAVILY TESTED** in interviews!

**Tài liệu:** Tạo 5 file: `srp.md`, `ocp.md`, `lsp.md`, `isp.md`, `dip.md`

#### 5. [design-patterns/](design-patterns/) - Design Patterns ⭐⭐⭐

- [ ] Singleton / Mẫu Đơn
- [ ] Factory / Mẫu Nhà Máy
- [ ] Builder / Mẫu Xây Dựng
- [ ] Observer / Mẫu Người Quan Sát
- [ ] Strategy / Mẫu Chiến Lược
- [ ] Decorator / Mẫu Trang Trí

**Lưu ý:** Thường được hỏi kết hợp với SOLID! / Often combined with SOLID questions!

**Tài liệu:** Tạo 6 file cho mỗi pattern

#### 6. [exception-handling/](exception-handling/) - Xử Lý Ngoại Lệ / Exception Handling

- [ ] try-catch-finally
- [ ] Custom Exceptions / Ngoại Lệ Tùy Chỉnh
- [ ] Best Practices / Các Thực Hành Tốt Nhất

**Tài liệu:** Tạo file: `try-catch-finally.md`, `custom-exceptions.md`, `best-practices.md`

---

### **Phase 3: Advanced Topics - Tuần 3 / Week 3**

**Mục tiêu:** Nắm vững Collections, LINQ, Memory Management, Delegates & Events.

**Objectives:** Master Collections, LINQ, Memory Management, Delegates & Events.

#### 7. [collections-linq/](collections-linq/) - Collections & LINQ

- [ ] List / Danh Sách
- [ ] Dictionary / Từ Điển
- [ ] HashSet / Tập Hợp
- [ ] LINQ Queries / Các Truy Vấn LINQ
- [ ] Performance Tips / Lời Khuyên Hiệu Năng

**Tài liệu:** Tạo file: `list.md`, `dictionary.md`, `hashset.md`, `linq-queries.md`, `performance.md`

#### 8. [memory-management/](memory-management/) - Quản Lý Bộ Nhớ / Memory Management

- [ ] Garbage Collection / Dọn Dẹp Rác
- [ ] Reference vs Value Types / Kiểu Tham Chiếu vs Kiểu Giá Trị
- [ ] IDisposable Pattern / Mẫu IDisposable
- [ ] Memory Leaks / Rò Rỉ Bộ Nhớ

**Tài liệu:** Tạo file: `garbage-collection.md`, `ref-vs-value.md`, `idisposable.md`, `memory-leaks.md`

#### 9. [delegates-events/](delegates-events/) - Delegates & Events

- [ ] Delegates / Ủy Quyền
- [ ] Action & Func / Hành Động & Hàm
- [ ] Events / Sự Kiện
- [ ] EventHandler / Xử Lý Sự Kiện

**Tài liệu:** Tạo file: `delegates.md`, `action-func.md`, `events.md`

---

### **Phase 4: Advanced + Performance - Tuần 4 / Week 4**

**Mục tiêu:** Async/Await, Threading, Performance, và tránh những lỗi thường gặp.

**Objectives:** Master Async/Await, Threading, Optimization, and Common Pitfalls.

#### 10. [async-threading/](async-threading/) - Async/Await & Threading

- [ ] Async/Await / Bất Đồng Bộ/Chờ Đợi
- [ ] Tasks / Tác Vụ
- [ ] Threading Concepts / Khái Niệm Luồng
- [ ] Concurrency / Tính Đồng Thời

**Lưu ý:** Deadlocks và race conditions là những vấn đề phổ biến! / Deadlocks and race conditions are common issues!

**Tài liệu:** Tạo file: `async-await.md`, `tasks.md`, `threading.md`, `concurrency.md`

#### 11. [performance/](performance/) - Hiệu Năng / Performance

- [ ] Memory Management / Quản lý bộ nhớ
- [ ] Algorithm Optimization / Tối ưu giải thuật

**Tài liệu:** Tạo file: `memory-optimization.md`, `algorithm-optimization.md`

#### 12. [pitfalls/](pitfalls/) - Những Lỗi Thường Gặp / Common Pitfalls

- [ ] Null Handling / Xử lý Null
- [ ] Threading Issues / Các vấn đề về Thread

**Tài liệu:** Tạo file: `null-handling.md`, `threading-issues.md`

---

## 🎯 Ôn Tập Theo Tần Suất Hỏi / Interview Frequency

### **Frequently Asked (Hỏi Rất Nhiều) ⭐⭐⭐**

- [ ] SOLID Principles
- [ ] Design Patterns (Singleton, Factory, Observer)
- [ ] Async/Await & Threading
- [ ] Collections & LINQ
- [ ] Exception Handling

### **Often Asked (Hỏi Thường Xuyên) ⭐⭐**

- [ ] OOP Concepts
- [ ] Memory Management
- [ ] Delegates & Events
- [ ] Best Practices

### **Sometimes Asked (Hỏi Đôi Khi) ⭐**

- [ ] Performance Optimization
- [ ] Common Pitfalls
- [ ] Other Patterns

---

## ✅ Checklist Chuẩn Bị / Preparation Checklist

- [ ] **Tuần 1:** Hoàn thành Foundation phase (OOP + Best Practices)
- [ ] **Tuần 2:** Hoàn thành Core Concepts (SOLID + Patterns + Exception Handling)
- [ ] **Tuần 3:** Hoàn thành Advanced Topics (Collections + Memory + Delegates)
- [ ] **Tuần 4:** Hoàn thành Final Phase (Async + Performance + Pitfalls)
- [ ] **Cuối tuần 4:** Ôn tập toàn bộ lại (Review All)
- [ ] **Trước phỏng vấn:** Luyện tập coding + Mock Interview

---

## 💡 Lời Khuyên / Tips

1. **Đọc -> Hiểu -> Luyện Tập**: Read → Understand → Practice
2. **Viết lại code bằng tay**: Rewrite code by hand, không chỉ đọc
3. **Giải thích outloud**: Explain concepts out loud để nhớ lâu hơn
4. **Tạo ví dụ riêng**: Create your own examples for each concept
5. **Mock Interview**: Thực hành trả lời câu hỏi phỏng vấn
6. **Focus on SOLID & Patterns**: Tập trung vào SOLID & Design Patterns nhất

---

**Ngày Bắt Đầu / Start Date:** January 26, 2026

**Ngày Dự Kiến Hoàn Thành / Expected Completion:** February 23, 2026

**Ngày Phỏng Vấn / Interview Date:** **********\_**********

---

Happy Studying! 🚀 Chúc Bạn Ôn Tập Thành Công!
