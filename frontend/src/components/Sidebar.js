// src/components/Sidebar.js

import React from 'react';

const Sidebar = () => {
  return (
    <aside style={styles.sidebar}>
      <ul style={styles.ul}>
        <li style={styles.li}><a style={styles.a} href="#overview">Overview</a></li>
        <li style={styles.li}><a style={styles.a} href="#storage">Storage</a></li>
        <li style={styles.li}><a style={styles.a} href="#earnings">Earnings</a></li>
        <li style={styles.li}><a style={styles.a} href="#expenditures">Expenditures</a></li>
        <li style={styles.li}><a style={styles.a} href="#system-status">System Status</a></li>
        <li style={styles.li}><a style={styles.a} href="#transactions">Transactions</a></li>
      </ul>
    </aside>
  );
};

const styles = {
  sidebar: {
    width: '200px',
    backgroundColor: '#f4f4f4',
    padding: '10px',
    height: '100vh',
    position: 'fixed',
    top: 0,
    left: 0,
    overflowY: 'auto',
  },
  ul: {
    listStyleType: 'none',
    padding: 0,
  },
  li: {
    marginBottom: '10px',
  },
  a: {
    textDecoration: 'none',
    color: 'black',
  },
};

export default Sidebar;
