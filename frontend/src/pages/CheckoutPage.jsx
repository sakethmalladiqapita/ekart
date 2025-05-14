/// === File: src/pages/CheckoutPage.jsx ===
import React, { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../contexts/AuthContext';

const CheckoutPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { user } = useAuth();
  const order = location.state?.order;
  const [loading, setLoading] = useState(false);
  const [details, setDetails] = useState({ cardNumber: '', name: '', expiry: '', cvv: '' });

  const handleChange = (e) => {
    setDetails({ ...details, [e.target.name]: e.target.value });
  };

  const handlePayment = async () => {
    setLoading(true);
    try {
      await axios.post('/api/payment/create', {
        orderId: order.id,
        amount: order.totalAmount
      });
      navigate('/confirmation');
    } catch (err) {
      console.error(err);
      alert('Payment processing failed.');
    } finally {
      setLoading(false);
    }
  };

  if (!order || !user) return <p style={{ textAlign: 'center', marginTop: '50px' }}>Invalid access. Please return to the cart.</p>;

  return (
    <div style={{
      maxWidth: '500px',
      margin: 'auto',
      padding: '32px 20px',
      fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
      backgroundColor: '#f9fafb',
      minHeight: '100vh'
    }}>
      <header style={{
        textAlign: 'center',
        marginBottom: '32px',
        color: '#111827',
        letterSpacing: '1px',
        textTransform: 'uppercase',
        fontWeight: '700',
        fontSize: '1.6rem',
        userSelect: 'none'
      }}>
        Checkout
      </header>

      <form style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
        <input
          type="text"
          name="cardNumber"
          placeholder="Card Number"
          value={details.cardNumber}
          onChange={handleChange}
          style={inputStyle}
        />
        <input
          type="text"
          name="name"
          placeholder="Name on Card"
          value={details.name}
          onChange={handleChange}
          style={inputStyle}
        />
        <input
          type="text"
          name="expiry"
          placeholder="Expiry Date (MM/YY)"
          value={details.expiry}
          onChange={handleChange}
          style={inputStyle}
        />
        <input
          type="password"
          name="cvv"
          placeholder="CVV"
          value={details.cvv}
          onChange={handleChange}
          style={inputStyle}
        />

        <button
          type="button"
          onClick={handlePayment}
          style={buttonStyle}
          disabled={loading}
        >
          {loading ? 'Processing...' : 'Pay Now'}
        </button>
      </form>
    </div>
  );
};

const inputStyle = {
  padding: '12px',
  fontSize: '1rem',
  border: '1px solid #d1d5db',
  borderRadius: '10px',
  outline: 'none',
  transition: 'border-color 0.3s ease-in-out',
  backgroundColor: 'white'
};

const buttonStyle = {
  backgroundColor: '#10b981',
  color: 'white',
  border: 'none',
  borderRadius: '12px',
  padding: '14px 0',
  fontSize: '1.1rem',
  fontWeight: '600',
  cursor: 'pointer',
  boxShadow: '0 4px 12px rgba(16, 185, 129, 0.4)'
};

export default CheckoutPage;
