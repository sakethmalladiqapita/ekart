import React, { useEffect, useState, useCallback } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import axios from '../api/axios';
import Toast from '../components/Toast';

const CartPage = () => {
  const { user, loading: authLoading } = useAuth(); // ✅ includes loading state
  const [cart, setCart] = useState([]);
  const [loading, setLoading] = useState(true);
  const [actionLoading, setActionLoading] = useState(false);
  const [toast, setToast] = useState({ visible: false, message: '', type: 'info' });

  const navigate = useNavigate();

  const fetchCart = useCallback(async () => {
    try {
      const res = await axios.get(`/api/cart`);
      setCart(res.data);
    } catch (err) {
      console.error(err);
      setToast({ visible: true, message: 'Failed to load cart items', type: 'error' });
    } finally {
      setLoading(false);
    }
  }, []);

  // ✅ Properly delay redirect/fetch until auth check is complete
  useEffect(() => {
    if (!authLoading) {
      if (!user) {
        navigate('/login');
      } else {
        fetchCart();
      }
    }
  }, [user, authLoading, navigate, fetchCart]);

  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  const updateQuantity = async (productId, newQty) => {
    if (newQty < 1 || actionLoading) return;
    const item = cart.find(c => c.productId === productId);
    const delta = newQty - item.quantity;

    try {
      setActionLoading(true);
      await axios.post('/api/cart/add', { productId, quantity: delta });

      setCart(prev =>
        prev.map(i => (i.productId === productId ? { ...i, quantity: newQty } : i))
      );
    } catch (err) {
      console.error(err);
      setToast({ visible: true, message: 'Failed to update quantity', type: 'error' });
    } finally {
      setActionLoading(false);
    }
  };

  const emptyCart = async () => {
    if (actionLoading) return;

    try {
      setActionLoading(true);
      for (const item of cart) {
        await axios.post('/api/cart/add', {
          productId: item.productId,
          quantity: -item.quantity
        });
      }
      setCart([]);
      setToast({ visible: true, message: 'Cart emptied successfully', type: 'success' });
    } catch (err) {
      console.error(err);
      setToast({ visible: true, message: 'Failed to empty cart', type: 'error' });
    } finally {
      setActionLoading(false);
    }
  };

  const handleCheckout = async () => {
    if (actionLoading) return;

    try {
      setActionLoading(true);
      const res = await axios.post('/api/cart/checkout');
      navigate('/checkout', { state: { order: res.data } });
    } catch (err) {
      console.error(err);
      setToast({ visible: true, message: 'Checkout failed. Please try again.', type: 'error' });
    } finally {
      setActionLoading(false);
    }
  };

  const cartTotal = cart
    .filter(item => item.quantity > 0)
    .reduce((total, item) => total + item.unitPrice * item.quantity, 0);

  return (
    <div className="page-container">
      <header className="page-header">Your Cart</header>

      {loading ? (
        <p>Loading...</p>
      ) : !cart.length ? (
        <p style={{ textAlign: 'center' }}>Your cart is empty.</p>
      ) : (
        <>
          <div className="card-grid">
            {cart.map((item, idx) =>
              item.quantity > 0 ? (
                <div key={idx} className="card">
                  <img
                    src={item.imageUrl}
                    alt={item.productName}
                    className="card-image"
                  />
                  <div className="card-content">
                    <h2 className="card-title">{item.name}</h2>
                    <p className="card-price">
                      ₹{(item.unitPrice * item.quantity).toFixed(2)}
                    </p>
                    <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                      <button
                        onClick={() => updateQuantity(item.productId, item.quantity - 1)}
                        className="qty-btn"
                      >
                        -
                      </button>
                      <span style={{ minWidth: '24px', textAlign: 'center' }}>
                        {item.quantity}
                      </span>
                      <button
                        onClick={() => updateQuantity(item.productId, item.quantity + 1)}
                        className="qty-btn"
                      >
                        +
                      </button>
                    </div>
                  </div>
                </div>
              ) : null
            )}
          </div>

          {cart.some(item => item.quantity > 0) && (
            <>
              <div className="total-display">
                Total Amount: ₹{cartTotal.toFixed(2)}
              </div>

              <div style={{ textAlign: 'center', marginTop: '40px' }}>
                <button
                  onClick={handleCheckout}
                  className="btn btn-primary"
                  style={{ marginRight: '20px' }}
                >
                  Proceed to Checkout
                </button>
                <button onClick={emptyCart} className="btn btn-danger">
                  Empty Cart
                </button>
              </div>
            </>
          )}
        </>
      )}

      {toast.visible && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast({ ...toast, visible: false })}
        />
      )}
    </div>
  );
};

export default CartPage;
