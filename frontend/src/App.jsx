/// === File: src/App.jsx ===
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { CssBaseline } from '@mui/material';
import LoginPage from './pages/LoginPage';
import ProductListPage from './pages/ProductListPage';
import CartPage from './pages/CartPage';
import CheckoutPage from './pages/CheckoutPage';
import ConfirmationPage from './pages/ConfirmationPage';
import OrderHistoryPage from './pages/OrderHistoryPage';
import Navbar from './components/NavBar';
import { AuthProvider } from './contexts/AuthContext';
import CreateUserPage from './pages/CreateUserPage';
import Footer from './components/Footer';
import './App.css'; // make sure this exists
import './styles/AppStyles.css';


function App() {
  return (
    <AuthProvider>
      <Router>
        <CssBaseline />
        <div className="app-container">
          <Navbar />
          <main className="main-content">
            <Routes>
              <Route path="/" element={<ProductListPage />} />
              <Route path="/login" element={<LoginPage />} />
              <Route path="/cart" element={<CartPage />} />
              <Route path="/checkout" element={<CheckoutPage />} />
              <Route path="/confirmation" element={<ConfirmationPage />} />
              <Route path="/orders" element={<OrderHistoryPage />} />
              <Route path="/register" element={<CreateUserPage />} />
            </Routes>
          </main>
          <Footer />
        </div>
      </Router>
    </AuthProvider>
  );
}

export default App;
