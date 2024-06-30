// src/components/Dashboard/Overview.js

import React from 'react';

const Overview = () => {
  return (
    <div style={styles.container}>
      <h2>Overview</h2>
      <div style={styles.cards}>
        <div style={styles.card}>Total Storage Used</div>
        <div style={styles.card}>Total Earnings</div>
        <div style={styles.card}>Total Expenditures</div>
        <div style={styles.card}>System Status</div>
      </div>
    </div>
  );
};

const styles = {
  container: {
    padding: '20px',
    marginLeft: '220px',
  },
  cards: {
    display: 'flex',
    justifyContent: 'space-around',
  },
  card: {
    padding: '20px',
    backgroundColor: '#e0e0e0',
    width: '200px',
    textAlign: 'center',
    borderRadius: '8px',
  },
};

export default Overview;
