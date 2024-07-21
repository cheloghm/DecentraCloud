import React, { useEffect, useState } from 'react';
import axios from 'axios';
import config from '../../config';
import Modal from '../Modal';

const { apiUrl } = config;

const NodeManagement = () => {
  const [nodes, setNodes] = useState([]);
  const [selectedNode, setSelectedNode] = useState(null);
  const [formData, setFormData] = useState({
    storage: '',
  });
  const [message, setMessage] = useState('');

  useEffect(() => {
    fetchNodes();
  }, []);

  const fetchNodes = async () => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      const response = await axios.get(`${apiUrl}/nodes/all`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setNodes(response.data);
    } catch (error) {
      console.error('Failed to fetch nodes:', error);
    }
  };

  const handleNodeSelect = async (nodeId) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      const response = await axios.get(`${apiUrl}/nodemanagement/node/${nodeId}`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setSelectedNode(response.data);
      setFormData({ storage: response.data.storage });
    } catch (error) {
      console.error('Failed to fetch node details:', error);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user || !selectedNode) return;

    try {
      await axios.put(`${apiUrl}/nodemanagement/node/${selectedNode.id}`, formData, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setMessage('Node updated successfully');
      fetchNodes(); // Refresh the node list
      setSelectedNode(null); // Close modal
    } catch (error) {
      console.error('Failed to update node:', error);
      setMessage('Failed to update node');
    }
  };

  const handleDeleteNode = async (nodeId) => {
    if (!window.confirm('Are you sure you want to delete this node? This action cannot be undone.')) return;

    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      await axios.delete(`${apiUrl}/nodemanagement/node/${nodeId}`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setMessage('Node deleted successfully');
      fetchNodes(); // Refresh the node list
    } catch (error) {
      console.error('Failed to delete node:', error);
      setMessage('Failed to delete node');
    }
  };

  return (
    <div>
      <h1>Node Management Dashboard</h1>
      <div>
        {nodes.map((node) => (
          <div key={node.id} onClick={() => handleNodeSelect(node.id)} style={styles.nodeCard}>
            <h2>{node.nodeName}</h2>
            <p>Online: {node.isOnline ? 'Yes' : 'No'}</p>
            <p>Endpoint: {node.endpoint}</p>
          </div>
        ))}
      </div>
      {selectedNode && (
        <Modal onClose={() => setSelectedNode(null)}>
          <form onSubmit={handleFormSubmit}>
            <h2>{selectedNode.nodeName}</h2>
            <p>Node ID: {selectedNode.id}</p>
            <p>Online: {selectedNode.isOnline ? 'Yes' : 'No'}</p>
            <p>Endpoint: {selectedNode.endpoint}</p>
            <label>
              Storage:
              <input
                type="number"
                name="storage"
                value={formData.storage}
                onChange={handleInputChange}
              />
            </label>
            <button type="submit">Update</button>
            <button type="button" onClick={() => handleDeleteNode(selectedNode.id)} style={styles.deleteButton}>Delete</button>
          </form>
          {message && <p>{message}</p>}
        </Modal>
      )}
    </div>
  );
};

const styles = {
  nodeCard: {
    border: '1px solid #ccc',
    padding: '10px',
    margin: '10px',
    borderRadius: '5px',
    cursor: 'pointer',
  },
  deleteButton: {
    backgroundColor: 'red',
    color: 'white',
    marginTop: '10px',
  },
};

export default NodeManagement;
