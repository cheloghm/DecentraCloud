import React from 'react';
import { NavLink } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

const Sidebar = () => {
  return (
    <nav className="d-flex flex-column p-3 bg-light" style={{ height: '100vh', width: '250px' }}>
      <h2 className="text-center">Dashboard</h2>
      <ul className="nav nav-pills flex-column mb-auto">
        <li className="nav-item">
          <NavLink to="/dashboard/overview" className="nav-link" activeClassName="active">
            Overview
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink to="/dashboard/storage-usage" className="nav-link" activeClassName="active">
            Storage Usage
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink to="/dashboard/earnings" className="nav-link" activeClassName="active">
            Earnings
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink to="/dashboard/expenditures" className="nav-link" activeClassName="active">
            Expenditures
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink to="/dashboard/system-status" className="nav-link" activeClassName="active">
            System Status
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink to="/dashboard/transactions" className="nav-link" activeClassName="active">
            Transactions
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink to="/dashboard/node-management" className="nav-link" activeClassName="active">
            Node Management
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink to="/dashboard/files" className="nav-link" activeClassName="active">
            Files
          </NavLink>
        </li>
      </ul>
    </nav>
  );
};

export default Sidebar;
