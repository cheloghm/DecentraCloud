// src/services/authService.js

import axios from 'axios';

const API_URL = 'https://localhost:7240/api/Auth'; // Ensure the path matches exactly, including case sensitivity

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
        return response.data;
    } catch (error) {
        console.error('Login Error:', error.response?.data || error.message);
        throw error;
    }
};

export { register, login };
