import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const Navbar = () => {
  const { user, logout } = useAuth();         // 🔐 Auth context (user info + logout)
  const navigate = useNavigate();             // 🔀 SPA navigation

  // 🔓 Logout action: clear auth and redirect
  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <AppBar
      position="static"
      sx={{
        backgroundColor: '#1f2937',            // Dark neutral background
        boxShadow: '0 2px 6px rgba(0, 0, 0, 0.2)'
      }}
    >
      <Toolbar sx={{ justifyContent: 'space-between', px: 3 }}>
        {/* 📛 Brand / Home Link */}
        <Typography
          variant="h6"
          component={Link}
          to="/"
          sx={{
            color: '#d1d5db',
            textDecoration: 'none',
            fontWeight: 700,
            letterSpacing: '1px'
          }}
        >
          eKart
        </Typography>

        {/* 🧭 Right-side navigation */}
        <Box sx={{ display: 'flex', gap: 2 }}>
          {user ? (
            // ✅ Authenticated user nav
            <>
              <NavLink to="/cart" label="Cart" />
              <NavLink to="/orders" label="Orders" />
              <Button
                onClick={handleLogout}
                sx={{
                  color: '#d1d5db',
                  fontWeight: 500,
                  textTransform: 'none'
                }}
              >
                Logout
              </Button>
            </>
          ) : (
            // 🔓 Guest nav
            <>
              <NavLink to="/login" label="Login" />
              <NavLink to="/register" label="Register" />
            </>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

// 🔧 Reusable styled navigation link
const NavLink = ({ to, label }) => (
  <Button
    component={Link}
    to={to}
    sx={{
      color: '#d1d5db',
      fontWeight: 500,
      textTransform: 'none',
      '&:hover': {
        color: '#ffffff',
        backgroundColor: 'rgba(255, 255, 255, 0.08)'
      }
    }}
  >
    {label}
  </Button>
);

export default Navbar;
