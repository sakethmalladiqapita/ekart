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
- Clean Architecture + CQRS Pattern
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

## 🧱 Project Structure

\`\`\`
ekart/
│
├── backend/                 # .NET Web API (Controllers, Services, CQRS, Models)
├── frontend/                # React + Vite app
│   ├── src/
│   │   ├── pages/           # Page components (Cart, Checkout, Orders, etc.)
│   │   ├── components/      # Shared UI components (Header, Footer, Navbar)
│   │   ├── contexts/        # AuthContext provider
│   │   ├── api/axios.js     # Axios instance with token interceptor
│   ├── public/
│   ├── index.html
│
├── database/                # MongoDB collections (Products, Orders, Users, etc.)
├── README.md
\`\`\`

---

## 🧪 Running Locally

### 📥 Prerequisites
- [.NET SDK 8.0+](https://dotnet.microsoft.com)
- [Node.js 18+](https://nodejs.org)
- [MongoDB](https://www.mongodb.com/try/download/community)

### 🔧 Backend Setup

\`\`\`bash
cd backend
dotnet restore
dotnet run
\`\`\`

- Runs on `http://localhost:5105`
- API: `/api/products`, `/api/cart`, `/api/orders`, `/api/payment`, etc.

### 🌐 Frontend Setup

\`\`\`bash
cd frontend
npm install
npm run dev
\`\`\`

- Runs on `http://localhost:5173`
- React app using Vite + MUI

---

## 📸 Screenshots

| Product List | Cart Page | Checkout |
|--------------|-----------|----------|
| ✅ Paginated UI | ✅ Quantity controls | ✅ Card details + pay now |
| ✅ Add to cart  | ✅ Empty cart        | ✅ Buy now support         |

---

## ⚙ Environment Variables

### Frontend `.env`
\`\`\`
VITE_API_URL=http://localhost:5105
\`\`\`

### Backend `appsettings.json`
\`\`\`json
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
\`\`\`

---

## 📌 Design Patterns Used

- ✅ **CQRS** – Separation of queries and commands
- ✅ **Repository Pattern**
- ✅ **Factory Pattern**
- ✅ **Anti-Corruption Layer** – Razorpay translator
- ✅ **NServiceBus** – Asynchronous messaging for events

---

## 📬 API Overview

| Endpoint                    | Method | Auth | Description                     |
|----------------------------|--------|------|---------------------------------|
| `/api/auth/login`          | POST   | ❌   | User login                      |
| `/api/users/create`        | POST   | ❌   | User registration               |
| `/api/products`            | GET    | ❌   | Fetch all products              |
| `/api/cart/add`            | POST   | ✅   | Add/update/remove cart item     |
| `/api/cart/checkout`       | POST   | ✅   | Checkout and place order        |
| `/api/orders/{userId}`     | GET    | ✅   | Get all orders for a user       |
| `/api/payment/create`      | POST   | ✅   | Trigger Razorpay order creation |
| `/api/delivery/status/:id` | GET    | ✅   | Get delivery status by order    |

---

## ✨ Future Enhancements

- 📱 Mobile-responsive UI
- 🔔 Real-time status updates via SignalR
- 🧾 Admin dashboard
- 🛠 Improved error boundaries
- 🌍 i18n support

---

## 👨‍💻 Author

Built with ❤️ by [Saketh Malladi]  
[GitHub](https://github.com/sakethmalladiqapita)

---
