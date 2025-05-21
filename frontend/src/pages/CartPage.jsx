import React, { useEffect, useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import axios from '../api/axios';

const CartPage = () => {
  const { user } = useAuth(); // Access authenticated user context
  const [cart, setCart] = useState([]); // Local state for cart items
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  // 🔄 Fetch user's cart from backend
  const fetchCart = async () => {
    try {
      const res = await axios.get(`/api/cart`);
      setCart(res.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  // 🔐 Redirect unauthenticated users to login and fetch cart on load
  useEffect(() => {
    if (!user) return navigate('/login');
    fetchCart();
  }, [user, navigate]);
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);
  
  // ➕➖ Update quantity of a specific cart item
  const updateQuantity = async (productId, newQty) => {
    if (newQty < 1) return;
    const item = cart.find(c => c.productId === productId);
    const delta = newQty - item.quantity;
    try {
      axios.post('/api/cart/add', {
        productId,
        quantity:delta
      });
      
      await fetchCart();
    } catch (err) {
      console.error(err);
    }
  };

  // 🗑️ Remove all items from cart
  const emptyCart = async () => {
    try {
      for (const item of cart) {
        await axios.post('/api/cart/add', {
          productId: item.productId,
          quantity: -item.quantity
        });
      }
      setCart([]);
    } catch (err) {
      console.error(err);
    }
  };

  // 💳 Initiate checkout and redirect with order data
  const handleCheckout = async () => {
    try {
      const res = await axios.post('/api/cart/checkout', { userId: user.id });
      navigate('/checkout', { state: { order: res.data } });
    } catch (err) {
      console.error(err);
      alert('Checkout failed');
    }
  };

  // 💰 Calculate total amount for display
  const cartTotal = cart
    .filter(item => item.quantity > 0)
    .reduce((total, item) => total + item.unitPrice * item.quantity, 0);

  return (
    <div style={{
      maxWidth: '1200px',
      margin: 'auto',
      padding: '24px 20px',
      fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
      backgroundColor: '#f9fafb',
      minHeight: '100vh',
      boxSizing: 'border-box'
    }}>
      <header style={{
        textAlign: 'center',
        marginBottom: '32px',
        color: '#111827',
        letterSpacing: '2px',
        textTransform: 'uppercase',
        fontWeight: '700',
        fontSize: '1.8rem',
        userSelect: 'none'
      }}>
        Your Cart
      </header>

      {/* ⏳ Loading & Empty States */}
      {loading ? (
        <p>Loading...</p>
      ) : !cart.length ? (
        <p style={{ textAlign: 'center' }}>Your cart is empty.</p>
      ) : (
        <>
          {/* 🛒 Cart Item Grid */}
          <div style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fit, minmax(260px, 1fr))',
            gap: '24px'
          }}>
            {cart.map((item, idx) => (
              item.quantity > 0 && (
                <div
                  key={idx}
                  style={{
                    backgroundColor: '#ffffff',
                    borderRadius: '16px',
                    boxShadow: '0 3px 12px rgba(0, 0, 0, 0.1)',
                    overflow: 'hidden',
                    display: 'flex',
                    flexDirection: 'column'
                  }}
                >
                  <img
                    src={item.imageUrl}
                    alt={item.productName}
                    style={{ height: '180px', objectFit: 'cover', width: '100%' }}
                  />
                  <div style={{ padding: '16px' }}>
                    <h2 style={{
                      fontSize: '1.1rem',
                      fontWeight: '700',
                      color: '#111827',
                      marginBottom: '8px'
                    }}>{item.name}</h2>
                    <p style={{
                      fontSize: '1rem',
                      color: '#6b7280',
                      marginBottom: '8px'
                    }}>
                      Price: ₹{(item.unitPrice * item.quantity).toFixed(2)}
                    </p>
                    <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                      <button onClick={() => updateQuantity(item.productId, item.quantity - 1)} style={qtyBtn}>-</button>
                      <span style={{ minWidth: '24px', textAlign: 'center' }}>{item.quantity}</span>
                      <button onClick={() => updateQuantity(item.productId, item.quantity + 1)} style={qtyBtn}>+</button>
                    </div>
                  </div>
                </div>
              )
            ))}
          </div>

          {/* 💰 Cart Total */}
          {cart.some(item => item.quantity > 0) && (
            <div style={{
              textAlign: 'center',
              marginTop: '30px',
              fontSize: '1.2rem',
              fontWeight: '600',
              color: '#111827'
            }}>
              Total Amount: ₹{cartTotal.toFixed(2)}
            </div>
          )}

          {/* 🛍️ Checkout + Empty Actions */}
          {cart.some(item => item.quantity > 0) && (
            <div style={{ textAlign: 'center', marginTop: '40px' }}>
              <button
                onClick={handleCheckout}
                style={{
                  backgroundColor: '#2563eb',
                  color: 'white',
                  border: 'none',
                  borderRadius: '12px',
                  padding: '14px 28px',
                  fontSize: '1.1rem',
                  fontWeight: '600',
                  cursor: 'pointer',
                  marginRight: '20px',
                  boxShadow: '0 4px 12px rgba(37, 99, 235, 0.4)'
                }}
              >
                Proceed to Checkout
              </button>
              <button
                onClick={emptyCart}
                style={{
                  backgroundColor: '#ef4444',
                  color: 'white',
                  border: 'none',
                  borderRadius: '12px',
                  padding: '14px 28px',
                  fontSize: '1.1rem',
                  fontWeight: '600',
                  cursor: 'pointer',
                  boxShadow: '0 4px 12px rgba(239, 68, 68, 0.4)'
                }}
              >
                Empty Cart
              </button>
            </div>
          )}
        </>
      )}
    </div>
  );
};

// 📦 Button style reused for +/– quantity actions
const qtyBtn = {
  padding: '6px 12px',
  fontSize: '1rem',
  fontWeight: '600',
  border: '1px solid #ccc',
  borderRadius: '6px',
  backgroundColor: '#f3f4f6',
  cursor: 'pointer'
};

export default CartPage;
