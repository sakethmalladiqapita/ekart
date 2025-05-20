import React from 'react';
import { Link } from 'react-router-dom';
import './App.css'; // 🎨 External CSS for layout and styling

// ✅ Main navigation header component
const Header = () => {
  return (
    <header className="header">
      {/* 📛 Brand Name */}
      <span className="brand">eKart</span>

      {/* 🧭 Navigation Links */}
      <nav className="nav-links">
        <Link to="/" className="nav-link">Products</Link>
        <Link to="/cart" className="nav-link">Cart</Link>
        <Link to="/orders" className="nav-link">Orders</Link>
        <Link to="/register" className="nav-link">Register</Link>
        <Link to="/logout" className="nav-link">Logout</Link>
      </nav>
    </header>
  );
};

export default Header;
