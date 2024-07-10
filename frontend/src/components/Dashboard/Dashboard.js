// src/components/Dashboard/Dashboard.js
import React from 'react';
import { Link } from 'react-router-dom';

const Dashboard = () => {
  return (
    <div>
      <h1>Dashboard</h1>
      <nav>
        <ul>
          <li><Link to="/dashboard/upload">Upload File</Link></li>
          <li><Link to="/dashboard/view-download">View/Download File</Link></li>
          <li><Link to="/dashboard/delete">Delete File</Link></li>
          <li><Link to="/dashboard/search">Search Files</Link></li>
          <li><Link to="/dashboard/node-management">Node Management</Link></li>
        </ul>
      </nav>
    </div>
  );
};

export default Dashboard;
