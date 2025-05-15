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
    const { cardNumber, name, expiry, cvv } = details;
  
    if (!cardNumber || !name || !expiry || !cvv) {
      alert('Please fill in all payment details.');
      return;
    }
  
    if (!/^\d{16}$/.test(cardNumber)) {
      alert('Card number must be exactly 16 digits.');
      return;
    }
  
    if (!/^\d{3,4}$/.test(cvv)) {
      alert('CVV must be 3 or 4 digits.');
      return;
    }
  
    if (!/^(0[1-9]|1[0-2])\/\d{2}$/.test(expiry)) {
      alert('Expiry must be in MM/YY format.');
      return;
    }
  
    setLoading(true);
    try {
      let finalOrder = order;
  
      // ✅ If no order (Buy Now path), create it now
      if (!finalOrder && location.state?.type === 'buynow') {
        const res = await axios.post('/api/orders/buy-now', {
          userId: user.id,
          productId: location.state.productId,
          quantity: location.state.quantity
        });
        finalOrder = res.data;
      }
  
      if (!finalOrder) {
        throw new Error('Order not available');
      }
  
      // ✅ Then call payment API
      await axios.post('/api/payment/create', {
        orderId: finalOrder.id,
        amount: finalOrder.totalAmount
      });
  
      navigate('/confirmation', { state: { orderId: finalOrder.id } });
    } catch (err) {
      console.error(err);
      alert('Payment processing failed.');
    } finally {
      setLoading(false);
    }
  };
  
  

  if (!user) {
    return <p style={{ textAlign: 'center', marginTop: '50px' }}>Please log in first.</p>;
  }
  
  if (!order && !(location.state?.type === 'buynow' && location.state?.productId && location.state?.quantity)) {
    return <p style={{ textAlign: 'center', marginTop: '50px' }}>Invalid access. Please return to the cart.</p>;
  }
  

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
  style={{
    ...buttonStyle,
    filter:
      loading ||
      !details.cardNumber ||
      !details.name ||
      !details.expiry ||
      !details.cvv
        ? 'opacity: 0.5'
        : 'none',
    cursor:
      loading ||
      !details.cardNumber ||
      !details.name ||
      !details.expiry ||
      !details.cvv
        ? 'not-allowed'
        : 'pointer',
  }}
  disabled={
    loading ||
    !details.cardNumber ||
    !details.name ||
    !details.expiry ||
    !details.cvv
  }
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
