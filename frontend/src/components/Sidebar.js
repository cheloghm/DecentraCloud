import React from 'react';
import { Link } from 'react-router-dom';

const Sidebar = () => {
  return (
    <nav>
      <ul>
        <li><Link to="/dashboard/overview">Overview</Link></li>
        <li><Link to="/dashboard/storage-usage">Storage Usage</Link></li>
        <li><Link to="/dashboard/earnings">Earnings</Link></li>
        <li><Link to="/dashboard/expenditures">Expenditures</Link></li>
        <li><Link to="/dashboard/system-status">System Status</Link></li>
        <li><Link to="/dashboard/transactions">Transactions</Link></li>
        <li><Link to="/dashboard/node-management">Node Management</Link></li>
        <li><Link to="/dashboard/files">Files</Link></li>
      </ul>
    </nav>
  );
};

export default Sidebar;
