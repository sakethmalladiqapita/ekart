import React, { useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import axios from '../api/axios';
import Toast from '../components/Toast';

const ConfirmationPage = () => {
  const location = useLocation();
  const orderId = location.state?.orderId;
  const [deliveryStatus, setDeliveryStatus] = useState('');
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState({ visible: false, message: '', type: 'error' });

  useEffect(() => {
    const fetchDeliveryStatus = async () => {
      try {
        const res = await axios.get(`/api/delivery/status/${orderId}`);
        setDeliveryStatus(res.data.deliveryStatus);
      } catch (err) {
        console.error(err);
        setToast({
          visible: true,
          message: 'Could not fetch delivery status.',
          type: 'error'
        });
        setDeliveryStatus('Unknown');
      } finally {
        setLoading(false);
      }
    };

    if (orderId) {
      fetchDeliveryStatus();
    } else {
      setLoading(false);
    }
  }, [orderId]);

  return (
    <div className="page-container" style={{ maxWidth: '600px', textAlign: 'center' }}>
      <h1 className="page-header text-green-500">Payment Successful!</h1>

      <p className="card-meta">Your order has been placed successfully.</p>

      {loading ? (
        <p className="card-meta">Checking delivery status...</p>
      ) : (
        orderId && (
          <p className="card-meta">
            Delivery Status: <strong>{deliveryStatus}</strong>
          </p>
        )
      )}

      <button
        className="btn btn-primary"
        onClick={() => window.location.href = '/'}
        style={{ marginTop: '30px' }}
      >
        Continue Shopping
      </button>

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

export default ConfirmationPage;
