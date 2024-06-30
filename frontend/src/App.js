// src/App.js

import React, { useState, useEffect } from 'react';
import Header from './components/Header';
import Sidebar from './components/Sidebar';
import Overview from './components/Dashboard/Overview';
import StorageUsage from './components/Dashboard/StorageUsage';
import Earnings from './components/Dashboard/Earnings';
import Expenditures from './components/Dashboard/Expenditures';
import SystemStatus from './components/Dashboard/SystemStatus';
import Transactions from './components/Dashboard/Transactions';
import LoginForm from './components/Auth/LoginForm';
import RegisterForm from './components/Auth/RegisterForm';
import Modal from './components/Modal';
import authService from './services/authService';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [showRegisterModal, setShowRegisterModal] = useState(false);
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [showProfileModal, setShowProfileModal] = useState(false);

  const handleLoginClick = () => setShowLoginModal(true);
  const handleRegisterClick = () => setShowRegisterModal(true);
  const handleCloseModal = () => {
    setShowLoginModal(false);
    setShowRegisterModal(false);
    setShowProfileModal(false);
  };

  const fetchUserDetails = async () => {
    try {
      const userDetails = await authService.getUserDetails();
      setUsername(userDetails.username);
      setEmail(userDetails.email);
    } catch (error) {
      console.error('Failed to fetch user details:', error);
    }
  };

  const handleLogin = async () => {
    setIsLoggedIn(true);
    handleCloseModal();
    await fetchUserDetails();
  };

  const handleRegister = async () => {
    setIsLoggedIn(true);
    handleCloseModal();
    await fetchUserDetails();
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
    localStorage.removeItem('user');
  };

  const handleProfileClick = () => setShowProfileModal(true);

  useEffect(() => {
    const user = localStorage.getItem('user');
    if (user) {
      setIsLoggedIn(true);
      fetchUserDetails();
    }
  }, []);

  return (
    <div>
      <Header
        onLoginClick={handleLoginClick}
        onRegisterClick={handleRegisterClick}
        isLoggedIn={isLoggedIn}
        onLogoutClick={handleLogout}
        username={username}
        onProfileClick={handleProfileClick}
      />
      {isLoggedIn ? (
        <div style={styles.container}>
          <Sidebar />
          <main style={styles.main}>
            <Overview />
            <StorageUsage />
            <Earnings />
            <Expenditures />
            <SystemStatus />
            <Transactions />
          </main>
        </div>
      ) : (
        <div style={styles.welcomeContainer}>
          <h1>DecentraCloud</h1>
          <button style={styles.button} onClick={handleLoginClick}>Login</button>
          <button style={styles.button} onClick={handleRegisterClick}>Register</button>
        </div>
      )}
      {showLoginModal && (
        <Modal onClose={handleCloseModal}>
          <LoginForm onLogin={handleLogin} />
        </Modal>
      )}
      {showRegisterModal && (
        <Modal onClose={handleCloseModal}>
          <RegisterForm onRegister={handleRegister} />
        </Modal>
      )}
      {showProfileModal && (
        <Modal onClose={handleCloseModal}>
          <div>
            <h2>Profile</h2>
            <p><strong>Username:</strong> {username}</p>
            <p><strong>Email:</strong> {email}</p>
          </div>
        </Modal>
      )}
    </div>
  );
}

const styles = {
  container: {
    display: 'flex',
  },
  main: {
    flexGrow: 1,
    padding: '20px',
  },
  welcomeContainer: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    height: '100vh',
    flexDirection: 'column',
  },
  button: {
    margin: '10px',
    padding: '10px 20px',
    backgroundColor: '#61dafb',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
  },
};

export default App;
