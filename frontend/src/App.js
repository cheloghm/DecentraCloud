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
import UserProfileForm from './components/UserProfileForm';
import Modal from './components/Modal';
import authService from './services/authService';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [showRegisterModal, setShowRegisterModal] = useState(false);
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [showProfileModal, setShowProfileModal] = useState(false);
  const [message, setMessage] = useState(null);

  const handleLoginClick = () => setShowLoginModal(true);
  const handleRegisterClick = () => setShowRegisterModal(true);
  const handleCloseModal = () => {
    setShowLoginModal(false);
    setShowRegisterModal(false);
    setShowProfileModal(false);
    setMessage(null);
  };

  const fetchUserDetails = async () => {
    try {
      const userDetails = await authService.getUserDetails();
      setUsername(userDetails.username);
      setEmail(userDetails.email);
    } catch (error) {
      setMessage('Failed to fetch user details');
      console.error('Failed to fetch user details:', error);
    }
  };

  const handleLogin = async () => {
    try {
      setIsLoggedIn(true);
      await fetchUserDetails();
      handleCloseModal();
    } catch (error) {
      setMessage('Login failed. Please try again later.');
    }
  };

  const handleRegister = async () => {
    try {
      setIsLoggedIn(true);
      await fetchUserDetails();
      handleCloseModal();
    } catch (error) {
      setMessage('Registration failed. Please try again later.');
    }
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
    localStorage.removeItem('user');
  };

  const handleProfileClick = () => setShowProfileModal(true);

  const handleUpdateUser = async (userDetails) => {
    try {
      await authService.updateUser(userDetails);
      setMessage('Profile updated successfully');
      await fetchUserDetails();
    } catch (error) {
      setMessage('Failed to update profile');
      console.error('Failed to update profile:', error);
    }
  };

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
          <LoginForm onLogin={handleLogin} onError={setMessage} />
        </Modal>
      )}
      {showRegisterModal && (
        <Modal onClose={handleCloseModal}>
          <RegisterForm onRegister={handleRegister} onError={setMessage} />
        </Modal>
      )}
      {showProfileModal && (
        <Modal onClose={handleCloseModal}>
          <UserProfileForm
            username={username}
            email={email}
            onUpdateUser={handleUpdateUser}
            onError={setMessage}
          />
        </Modal>
      )}
      {message && (
        <Modal onClose={handleCloseModal}>
          <div>
            <h2>Message</h2>
            <p>{message}</p>
            <button onClick={handleCloseModal}>Close</button>
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
