// src/components/Sidebar.js
import React from 'react';
import { Link } from 'react-router-dom';

const Sidebar = () => {
  return (
    <div>
      <nav>
        <ul>
          <li><Link to="/dashboard/overview">Overview</Link></li>
          <li><Link to="/dashboard/storage-usage">Storage</Link></li>
          <li><Link to="/dashboard/earnings">Earnings</Link></li>
          <li><Link to="/dashboard/expenditures">Expenditures</Link></li>
          <li><Link to="/dashboard/system-status">System Status</Link></li>
          <li><Link to="/dashboard/transactions">Transactions</Link></li>
          <li><Link to="/dashboard/node-management">Node Management</Link></li>
          <li><Link to="/dashboard/upload">Upload File</Link></li>
          <li><Link to="/dashboard/view-download">View/Download File</Link></li>
          <li><Link to="/dashboard/delete">Delete File</Link></li>
          <li><Link to="/dashboard/search">Search Files</Link></li>
        </ul>
      </nav>
    </div>
  );
};

export default Sidebar;
