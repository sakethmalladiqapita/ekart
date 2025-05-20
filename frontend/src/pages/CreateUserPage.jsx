import React, { useState } from 'react';
import axios from 'axios';

const CreateUserPage = () => {
  // 🧾 Form state for user input, including nested address
  const [form, setForm] = useState({
    email: '',
    passwordHash: '',
    address: {
      street: '',
      city: '',
      zip: ''
    }
  });

  // 📣 Feedback message (success or error)
  const [message, setMessage] = useState('');

  // 🖊️ Handles changes for both flat and nested address fields
  const handleChange = (e) => {
    const { name, value } = e.target;

    // If it's part of the address object
    if (['street', 'city', 'zip'].includes(name)) {
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

  // 📤 Form submission logic
  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await axios.post('http://localhost:5105/api/users/create', form);
      setMessage('User created successfully!');
    } catch (err) {
      console.error(err);

      const fallback = 'Failed to create user';

      // ✅ Handle model state validation errors or raw string errors
      if (err.response?.data?.errors) {
        const allErrors = Object.values(err.response.data.errors).flat();
        setMessage(allErrors.join(' ') || fallback);
      } else if (typeof err.response?.data === 'string') {
        setMessage(err.response.data);
      } else {
        setMessage(fallback);
      }
    }
  };

  return (
    <div style={{ maxWidth: '400px', margin: 'auto', padding: '32px' }}>
      <h2 style={{ textAlign: 'center' }}>Create New Account</h2>

      {/* 📋 Registration Form */}
      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
        <input
          type="email"
          name="email"
          placeholder="Email"
          value={form.email}
          onChange={handleChange}
          required
        />
        <input
          type="password"
          name="passwordHash"
          placeholder="Password"
          value={form.passwordHash}
          onChange={handleChange}
          required
        />
        <input
          type="text"
          name="street"
          placeholder="Street"
          value={form.address.street}
          onChange={handleChange}
        />
        <input
          type="text"
          name="city"
          placeholder="City"
          value={form.address.city}
          onChange={handleChange}
        />
        <input
          type="text"
          name="zip"
          placeholder="ZIP"
          value={form.address.zip}
          onChange={handleChange}
        />

        <button
          type="submit"
          style={{
            padding: '10px',
            backgroundColor: '#2563eb',
            color: 'white',
            border: 'none',
            borderRadius: '8px'
          }}
        >
          Create User
        </button>
      </form>

      {/* 📣 Feedback message display */}
      {message && (
        <p style={{ marginTop: '20px', textAlign: 'center', color: '#4b5563' }}>
          {message}
        </p>
      )}
    </div>
  );
};

export default CreateUserPage;
