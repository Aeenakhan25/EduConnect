# 🎓 EduConnect – Academic Web Portal

---

## 🌐 Overview 

EduConnect is a full-featured, role-based academic management system built using **Blazor (.NET 8), C#, and Bootstrap 5**, designed to simulate the internal workings of a real-world university portal. It brings together students, faculty, and administrators into a unified digital ecosystem where academic operations such as enrollment, course management, grading, and notifications are handled in a structured and scalable way.

EduConnect goes beyond basic CRUD operations by introducing **real-world academic constraints and rules**, such as course capacity limitations, semester-based enrollment restrictions, and active enrollment checks. These rules are enforced strictly in the service layer to mimic enterprise-level system behavior.

---

## ✨ Key Highlights
- 🎯 Role-Based Access (Admin / Faculty / Student)
- ⚡ Event-Driven Notification System
- 🧠 Clean Modular Architecture (Service-Based)
- 🔐 Mock Authentication with Role Management
- 📊 Dynamic CGPA & Grading Engine
- 🎨 Responsive UI using Bootstrap 5
- 🧩 Strict Code-Behind Pattern Implementation

---

## 📚 Features / Core Modules

### 🎓 Student Management
- Add, Edit, Delete students
- Live search filtering
- Student detail view with enrollments & grades
- Prevent deletion if active enrollments exist

### 📖 Course Management
- Course catalog with card UI
- Enrollment & drop functionality
- Capacity tracking (Open / AlmostFull / Full)
- Same-semester re-enrollment restriction

### 🧾 Grading System
- Faculty enters marks (0–100)
- Automatic letter grade generation
- CGPA calculation (credit-hour weighted)
- Student grade visualization

### 🔔 Notification System
- Event-driven notifications
- Real-time UI updates
- Notification bell with unread count
- Mark notifications as read

### 🔐 Authentication Module
- Mock login system
- Role-based navigation
- Route protection and unauthorized handling

---

## 🎯 Objectives
- Simulate a real-world academic management system
- Apply SOLID principles in a practical project
- Implement Blazor architecture with clean separation of concerns
- Demonstrate event-driven programming in UI systems
- Build scalable, maintainable, and modular code

---

## 🏗️ Architecture Overview

```text
Presentation Layer (Blazor UI)
        ↓
Service Layer (Business Logic)
        ↓
Models Layer (Entities + Enums)
        ↓
Interfaces Layer (Contracts)
```

---

## 🧠 Concepts Used
- Object-Oriented Programming (OOP)
- Inheritance (Person Hierarchy)
- Abstraction (Interfaces)
- Encapsulation (Service Layer)
- Event-Driven Programming
- Dependency Injection
- Component-Based Architecture
- Separation of Concerns

---

## 🛠️ Technologies
- Blazor (.NET 8)
- C#
- Bootstrap 5
- In-Memory Data Storage
- Dependency Injection (Built-in .NET)

---

## 🌍 Scope
- Academic institutions (simulation level)
- Learning advanced Blazor architecture
- Demonstrating enterprise-level system design
- Extendable for real database integration and APIs

---

## 🔄 System Flow

```text
User Login
   ↓
Role Identification (Admin / Faculty / Student)
   ↓
Dashboard Navigation
   ↓
Perform Actions (Enroll / Manage / Grade)
   ↓
Service Layer Processes Logic
   ↓
Events Triggered (if applicable)
   ↓
Notification System Updates UI
```

---

## 🔮 Future Improvements
- 🗄️ SQL Server / Database Integration
- 🔐 JWT Authentication & Authorization
- 🌐 REST API Layer
- ⚡ SignalR for Real-Time Updates
- 📅 Timetable & Attendance Module
- 📊 Advanced Analytics Dashboard
- 📱 Mobile App Version (Flutter)
