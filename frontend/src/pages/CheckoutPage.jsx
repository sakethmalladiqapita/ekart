import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import axios from '../api/axios';
import { useAuth } from '../contexts/AuthContext';
import Toast from '../components/Toast';

const CheckoutPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { user } = useAuth();

  const order = location.state?.order;

  const [loading, setLoading] = useState(false);
  const [toast, setToast] = useState({ visible: false, message: '', type: 'info' });

  const [details, setDetails] = useState({
    cardNumber: '',
    name: '',
    expiry: '',
    cvv: ''
  });

  const handleChange = (e) => {
    setDetails({ ...details, [e.target.name]: e.target.value });
  };

  const showToast = (msg, type = 'error') => {
    setToast({ visible: true, message: msg, type });
  };

  const handlePayment = async () => {
    const { cardNumber, name, expiry, cvv } = details;

    if (!cardNumber || !name || !expiry || !cvv) {
      return showToast('Please fill in all payment details.');
    }
    if (!/^\d{16}$/.test(cardNumber)) return showToast('Card number must be 16 digits.');
    if (!/^\d{3,4}$/.test(cvv)) return showToast('CVV must be 3 or 4 digits.');
    if (!/^(0[1-9]|1[0-2])\/\d{2}$/.test(expiry)) return showToast('Expiry must be in MM/YY format.');

    setLoading(true);

    try {
      let finalOrder = order;

      if (!finalOrder) {
        if (location.state?.type === 'buynow') {
          const res = await axios.post('/api/orders/buy-now', {
            productId: location.state.productId,
            quantity: location.state.quantity
          });
          finalOrder = res.data;
        } else {
          const res = await axios.post('/api/cart/checkout');
          finalOrder = res.data;
        }
      }

      if (!finalOrder) throw new Error('Order not available');

      await axios.post('/api/payment/create', {
        OrderId: finalOrder.id,
        Amount: finalOrder.totalAmount
      });

      navigate('/confirmation', { state: { orderId: finalOrder.id } });

    } catch (err) {
      console.error(err);
      showToast('Payment processing failed.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  if (!user) return <p style={{ textAlign: 'center', marginTop: '50px' }}>Please log in first.</p>;

  if (!order &&
      !(location.state?.type === 'buynow' && location.state?.productId && location.state?.quantity)) {
    return <p style={{ textAlign: 'center', marginTop: '50px' }}>Invalid access. Please return to the cart.</p>;
  }

  return (
    <div className="page-container" style={{ maxWidth: '500px' }}>
      <header className="page-header">Checkout</header>

      <form className="form-layout">
        <input
          type="text"
          name="cardNumber"
          placeholder="Card Number"
          value={details.cardNumber}
          onChange={handleChange}
          className="input-field"
        />
        <input
          type="text"
          name="name"
          placeholder="Name on Card"
          value={details.name}
          onChange={handleChange}
          className="input-field"
        />
        <input
          type="text"
          name="expiry"
          placeholder="Expiry Date (MM/YY)"
          value={details.expiry}
          onChange={handleChange}
          className="input-field"
        />
        <input
          type="password"
          name="cvv"
          placeholder="CVV"
          value={details.cvv}
          onChange={handleChange}
          className="input-field"
        />

        <button
          type="button"
          onClick={handlePayment}
          className="btn btn-success"
          disabled={loading}
        >
          {loading ? 'Processing...' : 'Pay Now'}
        </button>
      </form>

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

export default CheckoutPage;
