import React, { useState } from 'react';
import { Box, TextField, Button, Typography, Paper } from '@mui/material';
import { useAuth } from '../contexts/AuthContext'; // 🔐 Auth context for login token storage
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const LoginPage = () => {
  // 📋 Local state for login form
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const { login } = useAuth(); // ✅ Access login function from context
  const navigate = useNavigate(); // 🔀 For redirection after login

  // 🧾 Handles form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      // 🔐 POST login credentials to backend API
      const res = await axios.post('/api/auth/login', { email, password });
      const { token, user } = res.data;

      // ✅ Store user and token in global auth context
      login({ token, user });

      // 🔀 Redirect to homepage on success
      navigate('/');
    } catch (err) {
      alert('Login failed');
    }
  };

  return (
    <Box display="flex" justifyContent="center" mt={5}>
      <Paper sx={{ p: 4, width: 400 }}>
        <Typography variant="h5" gutterBottom>Login</Typography>

        {/* 📥 Login Form */}
        <form onSubmit={handleSubmit}>
          <TextField
            label="Email"
            fullWidth
            margin="normal"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
          <TextField
            label="Password"
            type="password"
            fullWidth
            margin="normal"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <Button
            type="submit"
            variant="contained"
            color="primary"
            fullWidth
            sx={{ mt: 2 }}
          >
            Login
          </Button>
        </form>
      </Paper>
    </Box>
  );
};

export default LoginPage;
