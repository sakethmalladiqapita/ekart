import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from '../api/axios';
import Toast from '../components/Toast';

const CreateUserPage = () => {
  const [form, setForm] = useState({
    email: '',
    passwordHash: '',
    address: {
      street: '',
      city: '',
      zip: '',
      state: '',
      country: ''
    }
  });

  const [loading, setLoading] = useState(false);
  const [toast, setToast] = useState({ visible: false, message: '', type: 'info' });
  const navigate = useNavigate();


  const handleChange = (e) => {
    const { name, value } = e.target;

    if (['street', 'city', 'zip', 'state', 'country'].includes(name)) {
      setForm(prev => ({
        ...prev,
        address: {
          ...prev.address,
          [name]: value
        }
      }));
    } else {
      setForm(prev => ({ ...prev, [name]: value }));
    }
  };

  const validate = () => {
    const { email, passwordHash, address } = form;
    const errors = [];

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const zipRegex = /^\d{6}$/;

    if (!email.trim() || !emailRegex.test(email)) {
      errors.push('Valid email is required.');
    }

    if (!passwordHash.trim() || passwordHash.length < 6) {
      errors.push('Password must be at least 6 characters.');
    }

    if (!address.street.trim()) errors.push('Street is required.');
    if (!address.city.trim()) errors.push('City is required.');
    if (!address.state.trim()) errors.push('State is required.');
    if (!address.country.trim()) errors.push('Country is required.');
    if (!zipRegex.test(address.zip)) {
      errors.push('ZIP must be a 6-digit number.');
    }

    if (errors.length > 0) {
      setToast({ visible: true, message: errors.join(' '), type: 'error' });
      return false;
    }

    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validate()) return;

    setLoading(true);
    setToast({ visible: false, message: '', type: 'info' });

    try {
      await axios.post('/api/users/create', form);

      setToast({ visible: true, message: 'User created successfully! Redirecting to login...', type: 'success' });
      
      setTimeout(() => {
        navigate('/login');
      }, 1500);
      
    } catch (err) {
      console.error(err);

      const fallback = 'Failed to create user';
      let message = fallback;

      if (err.response?.data?.errors) {
        const allErrors = Object.values(err.response.data.errors).flat();
        message = allErrors.join(' ') || fallback;
      } else if (err.response?.data?.detail) {
        message = err.response.data.detail;
      } else if (typeof err.response?.data === 'string') {
        message = err.response.data;
      }

      setToast({ visible: true, message, type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="page-container" style={{ maxWidth: '400px' }}>
      <h2 className="page-header">Create New Account</h2>

      <form onSubmit={handleSubmit} className="form-layout">
        <input
          type="email"
          name="email"
          placeholder="Email"
          value={form.email}
          onChange={handleChange}
          className="input-field"
        />
        <input
          type="password"
          name="passwordHash"
          placeholder="Password"
          value={form.passwordHash}
          onChange={handleChange}
          className="input-field"
        />
        <input
          type="text"
          name="street"
          placeholder="Street"
          value={form.address.street}
          onChange={handleChange}
          className="input-field"
        />
        <input
          type="text"
          name="city"
          placeholder="City"
          value={form.address.city}
          onChange={handleChange}
          className="input-field"
        />
        <input
          type="text"
          name="zip"
          placeholder="ZIP"
          value={form.address.zip}
          onChange={handleChange}
          className="input-field"
        />
        <input
          type="text"
          name="state"
          placeholder="State"
          value={form.address.state}
          onChange={handleChange}
          className="input-field"
        />
        <input
          type="text"
          name="country"
          placeholder="Country"
          value={form.address.country}
          onChange={handleChange}
          className="input-field"
        />

        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? 'Creating...' : 'Create User'}
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

export default CreateUserPage;
