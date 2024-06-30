// src/services/authService.js

import axios from 'axios';

const apiUrl = 'https://localhost:7240/api'; // Adjust the URL as needed

const login = async (credentials) => {
  const response = await axios.post(`${apiUrl}/Auth/login`, credentials);
  localStorage.setItem('user', JSON.stringify(response.data));
  return response.data;
};

const register = async (userDetails) => {
  const response = await axios.post(`${apiUrl}/Auth/register`, userDetails);
  localStorage.setItem('user', JSON.stringify(response.data));
  return response.data;
};

const logout = () => {
  localStorage.removeItem('user');
};

const getUserDetails = async () => {
  const user = JSON.parse(localStorage.getItem('user'));
  const token = user?.token;
  if (!token) throw new Error("User not authenticated");
  const response = await axios.get(`${apiUrl}/Auth/me`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return response.data;
};

export default {
  login,
  register,
  logout,
  getUserDetails,
};
