import axios from 'axios';

// ✅ Create a pre-configured Axios instance
const instance = axios.create({
  baseURL: 'http://localhost:5105' // 🌐 Your .NET backend base URL
});

// ✅ Automatically attach JWT token to every request if available
instance.interceptors.request.use(config => {
  const token = localStorage.getItem('token'); // 🔐 Pull JWT from localStorage
  if (token) {
    config.headers.Authorization = `Bearer ${token}`; // 🛡 Add Authorization header
  }
  return config;
});

export default instance; // 📦 Use this everywhere for API calls
