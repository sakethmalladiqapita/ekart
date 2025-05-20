import React, { createContext, useState, useContext, useEffect } from 'react';

// ✅ Create context for global auth state
const AuthContext = createContext();

// ✅ AuthProvider wraps the app and exposes login/logout/token/user
export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  // 🧠 On app load, try loading user from localStorage
  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
  }, []);

  // 🔐 Save token and user info after successful login
  const login = ({ token, user }) => {
    setUser(user);
    localStorage.setItem('user', JSON.stringify(user));
    localStorage.setItem('token', token);
  };

  // 🚪 Clear auth data from state and storage
  const logout = () => {
    setUser(null);
    localStorage.removeItem('user');
    localStorage.removeItem('token');
  };

  return (
    <AuthContext.Provider value={{
      user,             // 👤 Current user object
      login,            // 🔐 Login function
      logout,           // 🚪 Logout function
      token: localStorage.getItem('token') // 🔑 JWT token from storage
    }}>
      {children}
    </AuthContext.Provider>
  );
};

// ✅ Custom hook for easy access to AuthContext
export const useAuth = () => useContext(AuthContext);
