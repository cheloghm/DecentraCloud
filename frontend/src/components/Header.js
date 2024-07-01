// src/components/Header.js
import React from 'react';

const Header = ({ onLoginClick, onRegisterClick, isLoggedIn, onLogoutClick, username, onProfileClick }) => {
  return (
    <header style={styles.header}>
      <h1>DecentraCloud</h1>
      <nav style={styles.nav}>
        <a href="#" style={styles.link}>Home</a>
        <a href="#" style={styles.link}>About Us</a>
        <a href="#" style={styles.link}>Contact Us</a>
        {isLoggedIn ? (
          <>
            <a href="#" style={styles.link} onClick={onProfileClick}>{username || 'No Name'}</a>
            <a href="#" style={styles.link}>Dashboard</a>
            <a href="#" style={styles.link}>Settings</a>
            <button style={styles.button} onClick={onLogoutClick}>Logout</button>
          </>
        ) : (
          <>
            <button style={styles.button} onClick={onLoginClick}>Login</button>
            <button style={styles.button} onClick={onRegisterClick}>Register</button>
          </>
        )}
      </nav>
    </header>
  );
};

const styles = {
  header: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: '10px 20px',
    backgroundColor: '#282c34',
    color: 'white',
  },
  nav: {
    display: 'flex',
    alignItems: 'center',
  },
  link: {
    marginRight: '10px',
    textDecoration: 'none',
    color: 'white',
  },
  button: {
    marginLeft: '10px',
    padding: '5px 10px',
    backgroundColor: '#61dafb',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
  },
};

export default Header;
