import React, { useEffect, useState } from 'react';
import axios from '../api/axios';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import Toast from '../components/Toast';

const ProductListPage = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [toast, setToast] = useState({ visible: false, message: '', type: 'info' });
  const [quantities, setQuantities] = useState({});
  const [currentPage, setCurrentPage] = useState(1);

  const productsPerPage = 8;
  const { user } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    axios.get('/api/products', { headers: { Authorization: undefined } })
      .then(res => {
        const data = Array.isArray(res.data) ? res.data : res.data.products;
        setProducts(data);
        const initialQuantities = {};
        data.forEach(p => (initialQuantities[p.id] = 1));
        setQuantities(initialQuantities);
        setCurrentPage(1);
      })
      .catch(err => {
        console.error(err);
        setToast({ visible: true, message: 'Failed to load products', type: 'error' });
      })
      .finally(() => setLoading(false));
  }, []);

  useEffect(() => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }, [currentPage]);

  const handleBuyNow = (productId) => {
    const quantity = quantities[productId] || 1;
    navigate('/checkout', { state: { productId, quantity, type: 'buynow' } });
  };

  const handleAddToCart = async (productId, name) => {
    if (!user) return navigate('/login');
    try {
      const quantity = quantities[productId] || 1;
      await axios.post('/api/cart/add', {
        userId: user.id,
        productId,
        quantity
      });
      setToast({ visible: true, message: `"${name}" added to cart!`, type: 'success' });
    } catch (err) {
      console.error(err);
      setToast({ visible: true, message: 'Failed to add to cart.', type: 'error' });
    }
  };

  const handleQuantityChange = (productId, delta) => {
    setQuantities(prev => {
      const newQty = Math.max(1, (prev[productId] || 1) + delta);
      return { ...prev, [productId]: newQty };
    });
  };

  const indexOfLastProduct = currentPage * productsPerPage;
  const indexOfFirstProduct = indexOfLastProduct - productsPerPage;
  const currentProducts = products.slice(indexOfFirstProduct, indexOfLastProduct);
  const totalPages = Math.ceil(products.length / productsPerPage);

  return (
    <div className="page-container">
      <header className="page-header">Products</header>

      {loading ? (
        <p>Loading...</p>
      ) : (
        <>
          <div className="card-grid">
            {currentProducts.map(product => (
              <div
                key={product.id}
                className="card"
                onMouseEnter={e => e.currentTarget.style.transform = 'translateY(-6px)'}
                onMouseLeave={e => e.currentTarget.style.transform = 'translateY(0)'}
              >
                <img
                  src={product.imageUrl || 'https://via.placeholder.com/260x180'}
                  alt={product.name}
                  className="card-image"
                />
                <div className="card-content">
                  <h2 className="card-title">{product.name}</h2>
                  <p className="card-price">₹{product.price}</p>

                  {/* Quantity Controls */}
                  <div style={{ display: 'flex', alignItems: 'center', gap: '8px', marginBottom: '12px' }}>
                    <button onClick={() => handleQuantityChange(product.id, -1)} className="qty-btn">-</button>
                    <span style={{ minWidth: '24px', textAlign: 'center' }}>{quantities[product.id] || 1}</span>
                    <button onClick={() => handleQuantityChange(product.id, 1)} className="qty-btn">+</button>
                  </div>

                  {/* Add to Cart & Buy Now */}
                  <button onClick={() => handleAddToCart(product.id, product.name)} className="btn btn-primary" style={{ marginBottom: '8px' }}>
                    Add to Cart
                  </button>
                  <button onClick={() => handleBuyNow(product.id)} className="btn btn-success">
                    Buy Now
                  </button>
                </div>
              </div>
            ))}
          </div>

          {/* Pagination */}
          <div className="pagination">
            <button
              onClick={() => setCurrentPage(prev => Math.max(prev - 1, 1))}
              disabled={currentPage === 1}
              className="pagination-button"
            >
              Previous
            </button>
            <span style={{ margin: '0 16px', fontWeight: '600' }}>
              Page {currentPage} of {totalPages}
            </span>
            <button
              onClick={() => setCurrentPage(prev => Math.min(prev + 1, totalPages))}
              disabled={currentPage === totalPages}
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

export default ProductListPage;
