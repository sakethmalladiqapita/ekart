import React, { useEffect, useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import axios from '../api/axios';
import Toast from '../components/Toast';

const OrderHistoryPage = () => {
  const { user } = useAuth();
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState({ visible: false, message: '', type: 'error' });

  const [page, setPage] = useState(1);
  const pageSize = 3;

  const fetchOrders = async (pageNum) => {
    setLoading(true);
    try {
      const res = await axios.get(`/api/orders?page=${pageNum}&pageSize=${pageSize}`);
      setOrders(res.data);
    } catch (err) {
      console.error(err);
      setToast({ visible: true, message: 'Failed to load orders.', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (user) fetchOrders(page);
  }, [user, page]);

  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  const handleNext = () => setPage(prev => prev + 1);
  const handlePrev = () => setPage(prev => Math.max(prev - 1, 1));

  return (
    <div className="page-container">
      <header className="page-header">Order History</header>

      {loading ? (
        <p>Loading...</p>
      ) : orders.length === 0 ? (
        <p style={{ textAlign: 'center' }}>No past orders found.</p>
      ) : (
        <>
          <div className="card-grid">
            {orders.map((order, idx) => (
              <div key={idx} className="card">
                <div className="card-content">
                  <h2 className="card-title">Order #{order.id}</h2>
                  <p className="card-meta">Status: {order.status}</p>
                  <p className="card-meta">Date: {new Date(order.orderDate).toLocaleString()}</p>
                  <p className="card-price">₹{order.totalAmount}</p>
                </div>
              </div>
            ))}
          </div>

          <div className="pagination">
            <button onClick={handlePrev} disabled={page === 1} className="pagination-button">
              Previous
            </button>
            <span style={{ fontWeight: '600' }}>Page {page}</span>
            <button
              onClick={handleNext}
              disabled={orders.length < pageSize}
              className="pagination-button"
            >
              Next
            </button>
          </div>
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

export default OrderHistoryPage;
