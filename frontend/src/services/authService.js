import axios from 'axios';
import config from '../config';

const { apiUrl } = config;

const login = async (credentials) => {
  try {
    const response = await axios.post(`${apiUrl}/Auth/login`, credentials);
    localStorage.setItem('user', JSON.stringify(response.data));
    return response.data;
  } catch (error) {
    throw error;
  }
};

const register = async (userDetails) => {
  try {
    const response = await axios.post(`${apiUrl}/Auth/register`, userDetails);
    localStorage.setItem('user', JSON.stringify(response.data));
    return response.data;
  } catch (error) {
    throw error;
  }
};

const logout = () => {
  localStorage.removeItem('user');
};

const getUserDetails = async () => {
  const user = JSON.parse(localStorage.getItem('user'));
  const token = user?.token;
  if (!token) throw new Error("User not authenticated");
  try {
    const response = await axios.get(`${apiUrl}/Auth/me`, {
      headers: { Authorization: `Bearer ${token}` }
    });
    return response.data;
  } catch (error) {
    throw error;
  }
};

const updateUser = async (userDetails) => {
  const user = JSON.parse(localStorage.getItem('user'));
  const token = user?.token;
  if (!token) throw new Error("User not authenticated");
  try {
    const response = await axios.put(`${apiUrl}/Auth`, userDetails, {
      headers: { Authorization: `Bearer ${token}` }
    });
    return response.data;
  } catch (error) {
    throw error;
  }
};

const deleteUser = async () => {
  const user = JSON.parse(localStorage.getItem('user'));
  const token = user?.token;
  if (!token) throw new Error("User not authenticated");
  try {
    await axios.delete(`${apiUrl}/Auth`, {
      headers: { Authorization: `Bearer ${token}` }
    });
  } catch (error) {
    throw error;
  }
};

export default {
  login,
  register,
  logout,
  getUserDetails,
  updateUser,
  deleteUser,
};
