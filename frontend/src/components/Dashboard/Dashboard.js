// src/components/Dashboard/Dashboard.js
import React from 'react';
import { Link } from 'react-router-dom';
import FileUpload from './FileUpload';
import FileViewDownload from './FileViewDownload';
import FileDelete from './FileDelete';
import FileSearch from './FileSearch';

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
        </ul>
      </nav>
      <FileUpload />
      <FileViewDownload />
      <FileDelete />
      <FileSearch />
    </div>
  );
};

export default Dashboard;
