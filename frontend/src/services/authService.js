// src/services/authService.js

import axios from 'axios';

const API_URL = 'https://localhost:7240/api/Auth'; // Ensure the path matches exactly, including case sensitivity

axios.interceptors.request.use(
    config => {
        const token = localStorage.getItem('jwtToken');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    error => {
        Promise.reject(error)
    }
);

const register = async (username, email, password) => {
    try {
        const response = await axios.post(`${API_URL}/register`, {
            username,
            email,
            password
        });
        return response.data;
    } catch (error) {
        console.error('Registration Error:', error.response?.data || error.message);
        throw error;
    }
};

const login = async (email, password) => {
    try {
        const response = await axios.post(`${API_URL}/login`, {
            email,
            password
        });
        const token = response.data.Token;
        // Store the token in local storage
        localStorage.setItem('jwtToken', token);
        return response.data;
    } catch (error) {
        throw error;
    }
};

export { register, login };
