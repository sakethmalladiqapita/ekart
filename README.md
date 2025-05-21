# 🛒 eKart

**eKart** is a full-stack ecommerce web application built using **React**, **.NET (C#)**, and **MongoDB**, following clean architecture principles and the **CQRS** pattern. It features product listing, cart management, user authentication, Razorpay integration, and order tracking.

---

## 📦 Tech Stack

### 🧑‍💻 Frontend
- React + Vite
- Material UI (MUI)
- Axios (with interceptor)
- React Router DOM
- Toast notifications
- Protected routes & Auth context

### 🖥 Backend (API)
- ASP.NET Core Web API (.NET 8)
- MongoDB with official C# driver
- Clean Architecture + CQRS Pattern with MediatR
- NServiceBus for messaging
- Razorpay payment integration
- JWT authentication

---

## 🚀 Features

### 🔐 Authentication
- Register, Login, Logout
- JWT-based auth with React context

### 🛍 Products
- Paginated product listing
- Product details, quantities
- Add to cart / Buy now

### 🛒 Cart
- View, update, or empty cart
- Checkout with mock card input

### 💳 Payments
- Integrated Razorpay (mock)
- Payment confirmation
- Event-driven with NServiceBus

### 📦 Orders & Delivery
- Order history view
- Delivery status tracking
- Real-time status on confirmation

---

## 🧪 Running Locally

### 📥 Prerequisites
- [.NET SDK 8.0+](https://dotnet.microsoft.com)
- [Node.js 18+](https://nodejs.org)
- [MongoDB](https://www.mongodb.com/try/download/community)

### 🔧 Backend Setup

```bash
cd backend
dotnet restore
dotnet run
```

- Runs on `http://localhost:5105`
- API: `/api/products`, `/api/cart`, `/api/orders`, `/api/payment`, etc.

### 🌐 Frontend Setup

```bash
cd frontend
npm install
npm run dev
```

- Runs on `http://localhost:5173`
- React app using Vite + MUI

---

### Frontend `.env`
```
VITE_API_URL=http://localhost:5105
```

### Backend `appsettings.json`
```json
"MongoDB": {
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "ekartdatabase"
},
"Jwt": {
  "Key": "your_jwt_secret_key"
},
"Razorpay": {
  "Key": "your_razorpay_key",
  "Secret": "your_razorpay_secret"
}
```

---

## 📌 Design Patterns Used

- ✅ **CQRS with MediatR** – Separation of queries and commands
- ✅ **Repository Pattern**
- ✅ **NServiceBus** – Asynchronous messaging for events

---

## 👨‍💻 Author

Built by [Saketh Malladi]

---
