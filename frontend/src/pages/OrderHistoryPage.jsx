import React, { useEffect, useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import axios from '../api/axios';

const OrderHistoryPage = () => {
  const { user } = useAuth(); // 🔐 Access the current authenticated user
  const [orders, setOrders] = useState([]); // 📦 Store user's past orders
  const [loading, setLoading] = useState(true); // ⏳ Manage loading state

  // 📡 Fetch order history on component load
  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const res = await axios.get(`/api/orders/${user.id}`);
        setOrders(res.data);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false); // ✅ Always hide loader after fetch
      }
    };

    if (user) fetchOrders(); // Ensure user is loaded before fetching
  }, [user]);

  return (
    <div style={{
      maxWidth: '1200px',
      margin: 'auto',
      padding: '24px 20px',
      fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
      backgroundColor: '#f9fafb',
      minHeight: '100vh'
    }}>
      <header style={{
        textAlign: 'center',
        marginBottom: '32px',
        color: '#111827',
        letterSpacing: '2px',
        textTransform: 'uppercase',
        fontWeight: '700',
        fontSize: '1.8rem'
      }}>
        Order History
      </header>

      {/* 🔄 Loader + Fallback States */}
      {loading ? (
        <p>Loading...</p>
      ) : orders.length === 0 ? (
        <p style={{ textAlign: 'center' }}>No past orders found.</p>
      ) : (
        // ✅ Display order cards in a responsive grid
        <div style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(260px, 1fr))',
          gap: '24px'
        }}>
          {orders.map((order, idx) => (
            <div
              key={idx}
              style={{
                backgroundColor: '#ffffff',
                borderRadius: '16px',
                boxShadow: '0 3px 12px rgba(0, 0, 0, 0.1)',
                padding: '16px'
              }}
            >
              <h2 style={{
                fontSize: '1.1rem',
                fontWeight: '700',
                marginBottom: '8px'
              }}>
                Order #{order.id}
              </h2>
              <p style={{ fontSize: '0.95rem', marginBottom: '4px', color: '#6b7280' }}>
                Status: {order.status}
              </p>
              <p style={{ fontSize: '0.95rem', marginBottom: '4px', color: '#6b7280' }}>
                Date: {new Date(order.orderDate).toLocaleString()}
              </p>
              <p style={{ fontSize: '1.1rem', fontWeight: '600', color: '#10b981' }}>
                ₹{order.totalAmount}
              </p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default OrderHistoryPage;
