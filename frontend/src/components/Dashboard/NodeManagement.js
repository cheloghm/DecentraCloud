// src/components/Dashboard/NodeManagement.js

import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Bar, Line } from 'react-chartjs-2';

const NodeManagement = () => {
  const [nodes, setNodes] = useState([]);
  const [selectedNode, setSelectedNode] = useState(null);
  const [formData, setFormData] = useState({
    storage: '',
    // Add other properties
  });

  useEffect(() => {
    const fetchNodes = async () => {
      const user = JSON.parse(localStorage.getItem('user'));
      const token = user?.token;
      if (!token) return;

      const response = await axios.get(`http://localhost:5000/api/NodeManagement/${user.id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setNodes(response.data);
    };

    fetchNodes();
  }, []);

  const handleNodeSelect = (node) => {
    setSelectedNode(node);
    setFormData({
      storage: node.storage,
      // Add other properties
    });
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    const user = JSON.parse(localStorage.getItem('user'));
    const token = user?.token;
    if (!token) return;

    await axios.put(`http://localhost:5000/api/NodeManagement/${selectedNode.id}`, formData, {
      headers: { Authorization: `Bearer ${token}` },
    });

    alert('Node updated successfully');
  };

  const handleDeleteNode = async (nodeId) => {
    const user = JSON.parse(localStorage.getItem('user'));
    const token = user?.token;
    if (!token) return;

    await axios.delete(`http://localhost:5000/api/NodeManagement/${nodeId}`, {
      headers: { Authorization: `Bearer ${token}` },
    });

    setNodes(nodes.filter((node) => node.id !== nodeId));
  };

  return (
    <div>
      <h1>Node Management Dashboard</h1>
      <div>
        {nodes.map((node) => (
          <div key={node.id} onClick={() => handleNodeSelect(node)}>
            <h2>Node ID: {node.id}</h2>
            <p>Storage: {node.storage} GB</p>
            <button onClick={() => handleDeleteNode(node.id)}>Delete Node</button>
          </div>
        ))}
      </div>
      {selectedNode && (
        <form onSubmit={handleFormSubmit}>
          <h2>Update Node</h2>
          <div>
            <label>Storage</label>
            <input
              type="number"
              name="storage"
              value={formData.storage}
              onChange={handleInputChange}
            />
          </div>
          {/* Add other input fields */}
          <button type="submit">Update Node</button>
        </form>
      )}
      <div>
        <h2>Node Performance Metrics</h2>
        {/* Add charts and graphs here */}
      </div>
    </div>
  );
};

export default NodeManagement;
