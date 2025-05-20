import React, { useEffect, useState } from 'react';
import axios from '../api/axios';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';

// ✅ Toast component for quick feedback
const Toast = ({ message, onClose }) => {
  useEffect(() => {
    const timer = setTimeout(() => onClose(), 3000);
    return () => clearTimeout(timer);
  }, [onClose]);

  return (
    <div style={{
      position: 'fixed',
      bottom: '20px',
      left: '50%',
      transform: 'translateX(-50%)',
      backgroundColor: '#333',
      color: 'white',
      padding: '12px 24px',
      borderRadius: '30px',
      boxShadow: '0 5px 15px rgba(0,0,0,0.3)',
      fontSize: '1rem',
      zIndex: 1000,
      opacity: 0.9
    }}>
      {message}
    </div>
  );
};

const ProductListPage = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [toastMessage, setToastMessage] = useState(null);
  const [quantities, setQuantities] = useState({});
  const [currentPage, setCurrentPage] = useState(1);
  const productsPerPage = 8;

  const { user } = useAuth();
  const navigate = useNavigate();

  // 📦 Load products from backend on mount
  useEffect(() => {
    axios.get('/api/products', {
      headers: { Authorization: undefined } // 🛡️ Ensures unauthenticated access is allowed
    })
      .then(res => {
        const data = Array.isArray(res.data) ? res.data : res.data.products;
        setProducts(data);

        // Initialize quantity for each product to 1
        const initialQuantities = {};
        data.forEach(p => initialQuantities[p.id] = 1);
        setQuantities(initialQuantities);
        setCurrentPage(1);
      })
      .catch(console.error)
      .finally(() => setLoading(false));
  }, []);

  // 🧭 Scroll to top on page change
  useEffect(() => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }, [currentPage]);

  // 🛒 Buy now redirects to checkout with product & quantity
  const handleBuyNow = (productId) => {
    const quantity = quantities[productId] || 1;
    navigate('/checkout', { state: { productId, quantity, type: 'buynow' } });
  };

  // ➕ Add to cart via API
  const handleAddToCart = async (productId, name) => {
    if (!user) return navigate('/login');
    try {
      const quantity = quantities[productId] || 1;
      await axios.post('/api/cart/add', {
        userId: user.id,
        productId,
        quantity
      });
      setToastMessage(`"${name}" added to cart!`);
    } catch (err) {
      console.error(err);
      alert('Failed to add to cart');
    }
  };

  // 🔢 Handle quantity increment/decrement
  const handleQuantityChange = (productId, delta) => {
    setQuantities(prev => {
      const newQty = Math.max(1, (prev[productId] || 1) + delta);
      return { ...prev, [productId]: newQty };
    });
  };

  // 📊 Pagination logic
  const indexOfLastProduct = currentPage * productsPerPage;
  const indexOfFirstProduct = indexOfLastProduct - productsPerPage;
  const currentProducts = products.slice(indexOfFirstProduct, indexOfLastProduct);
  const totalPages = Math.ceil(products.length / productsPerPage);

  return (
    <>
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
          Products
        </header>

        {/* 🛍️ Product grid */}
        <div style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(260px, 1fr))',
          gap: '24px'
        }}>
          {currentProducts.map(product => (
            <div
              key={product.id}
              tabIndex={0}
              aria-label={`${product.name}, ₹${product.price}`}
              style={{
                backgroundColor: '#ffffff',
                borderRadius: '16px',
                boxShadow: '0 3px 12px rgba(0, 0, 0, 0.1)',
                overflow: 'hidden',
                display: 'flex',
                flexDirection: 'column',
                transition: 'transform 0.3s cubic-bezier(.25,.8,.25,1)',
                cursor: 'pointer'
              }}
              onFocus={e => e.currentTarget.style.boxShadow = '0 6px 22px rgba(59, 130, 246, 0.5)'}
              onBlur={e => e.currentTarget.style.boxShadow = '0 3px 12px rgba(0, 0, 0, 0.1)'}
              onMouseEnter={e => e.currentTarget.style.transform = 'translateY(-6px)'}
              onMouseLeave={e => e.currentTarget.style.transform = 'translateY(0)'}
            >
              <img
                src={product.imageUrl || 'https://via.placeholder.com/260x180'}
                alt={product.name}
                style={{
                  width: '100%',
                  height: '180px',
                  objectFit: 'cover',
                  borderTopLeftRadius: '16px',
                  borderTopRightRadius: '16px',
                  backgroundColor: '#f3f4f6'
                }}
              />
              <div style={{ padding: '16px', flexGrow: 1, display: 'flex', flexDirection: 'column' }}>
                <h2 style={{
                  fontSize: '1.1rem',
                  fontWeight: '700',
                  marginBottom: '8px',
                  color: '#111827'
                }}>{product.name}</h2>
                <p style={{
                  fontWeight: '600',
                  fontSize: '1.25rem',
                  color: '#10b981',
                  marginBottom: '16px'
                }}>
                  ₹{product.price}
                </p>

                {/* ➖➕ Quantity controls */}
                <div style={{ display: 'flex', alignItems: 'center', gap: '8px', marginBottom: '12px' }}>
                  <button onClick={() => handleQuantityChange(product.id, -1)} style={qtyBtn}>-</button>
                  <span style={{ minWidth: '24px', textAlign: 'center' }}>{quantities[product.id] || 1}</span>
                  <button onClick={() => handleQuantityChange(product.id, 1)} style={qtyBtn}>+</button>
                </div>

                {/* 🛒 Add to Cart */}
                <button
                  onClick={(e) => {
                    e.stopPropagation();
                    handleAddToCart(product.id, product.name);
                  }}
                  style={{
                    backgroundColor: '#2563eb',
                    color: 'white',
                    border: 'none',
                    borderRadius: '12px',
                    padding: '12px',
                    fontSize: '1rem',
                    fontWeight: '600',
                    cursor: 'pointer',
                    marginBottom: '8px'
                  }}
                >
                  Add to Cart
                </button>

                {/* 🛍 Buy Now */}
                <button
                  onClick={(e) => {
                    e.stopPropagation();
                    handleBuyNow(product.id);
                  }}
                  style={{
                    backgroundColor: '#10b981',
                    color: 'white',
                    border: 'none',
                    borderRadius: '12px',
                    padding: '12px',
                    fontSize: '1rem',
                    fontWeight: '600',
                    cursor: 'pointer'
                  }}
                >
                  Buy Now
                </button>
              </div>
            </div>
          ))}
        </div>

        {/* 🔁 Pagination Controls */}
        <div style={{ textAlign: 'center', marginTop: '30px' }}>
          <button
            onClick={() => setCurrentPage(prev => Math.max(prev - 1, 1))}
            disabled={currentPage === 1}
            style={paginationButton}
          >
            Previous
          </button>
          <span style={{ margin: '0 16px', fontWeight: '600' }}>
            Page {currentPage} of {totalPages}
          </span>
          <button
            onClick={() => setCurrentPage(prev => Math.min(prev + 1, totalPages))}
            disabled={currentPage === totalPages}
            style={paginationButton}
          >
            Next
          </button>
        </div>
      </div>

      {/* ✅ Feedback toast */}
      {toastMessage && <Toast message={toastMessage} onClose={() => setToastMessage(null)} />}
    </>
  );
};

// 📦 Quantity button style
const qtyBtn = {
  padding: '6px 12px',
  fontSize: '1rem',
  fontWeight: '600',
  border: '1px solid #ccc',
  borderRadius: '6px',
  backgroundColor: '#f3f4f6',
  cursor: 'pointer'
};

// ⬅➡ Pagination button style
const paginationButton = {
  padding: '8px 16px',
  fontSize: '1rem',
  fontWeight: '600',
  border: '1px solid #ccc',
  borderRadius: '8px',
  backgroundColor: '#ffffff',
  cursor: 'pointer',
  margin: '0 8px'
};

export default ProductListPage;
