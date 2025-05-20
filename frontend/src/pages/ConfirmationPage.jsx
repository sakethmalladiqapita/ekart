import React, { useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import axios from '../api/axios';

const ConfirmationPage = () => {
  const location = useLocation();
  const orderId = location.state?.orderId; // Get order ID from navigation state
  const [deliveryStatus, setDeliveryStatus] = useState(''); // Track delivery status

  // 🔄 Fetch delivery status on load
  useEffect(() => {
    const fetchDeliveryStatus = async () => {
      try {
        const res = await axios.get(`/api/delivery/status/${orderId}`);
        setDeliveryStatus(res.data); // Expected to be a string like "Processing", "Shipped", etc.
      } catch (err) {
        console.error(err);
        setDeliveryStatus('Unknown');
      }
    };

    if (orderId) {
      fetchDeliveryStatus();
    }
  }, [orderId]);

  return (
    <div style={{
      maxWidth: '600px',
      margin: 'auto',
      padding: '32px 20px',
      fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
      backgroundColor: '#f9fafb',
      minHeight: '100vh',
      textAlign: 'center'
    }}>
      <h1 style={{
        fontSize: '2rem',
        fontWeight: '700',
        color: '#10b981',
        marginBottom: '16px'
      }}>
        Payment Successful!
      </h1>

      <p style={{ fontSize: '1.1rem', color: '#374151', marginBottom: '12px' }}>
        Your order has been placed successfully.
      </p>

      {/* 🚚 Display delivery status if orderId is available */}
      {orderId && (
        <p style={{ fontSize: '1rem', color: '#4b5563', marginBottom: '24px' }}>
          Delivery Status: <strong>{deliveryStatus}</strong>
        </p>
      )}

      {/* 🔁 CTA to redirect to home page */}
      <button
        onClick={() => window.location.href = '/'}
        style={{
          backgroundColor: '#2563eb',
          color: 'white',
          border: 'none',
          borderRadius: '12px',
          padding: '14px 28px',
          fontSize: '1.1rem',
          fontWeight: '600',
          cursor: 'pointer',
          boxShadow: '0 4px 12px rgba(37, 99, 235, 0.4)'
        }}
      >
        Continue Shopping
      </button>
    </div>
  );
};

export default ConfirmationPage;
