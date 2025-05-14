import React from 'react';
import { Link } from 'react-router-dom';
import './App.css';

const Header = () => {
  return (
    <header className="header">
      <span className="brand">eKart</span>
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
