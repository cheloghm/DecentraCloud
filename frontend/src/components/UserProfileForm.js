import React, { useState, useEffect } from 'react';
import authService from '../services/authService';

const UserProfileForm = ({ username: initialUsername, email: initialEmail, onUpdateUser, onDelete }) => {
  const [username, setUsername] = useState(initialUsername);
  const [email, setEmail] = useState(initialEmail);
  const [settings, setSettings] = useState({
    receiveNewsletter: false,
    theme: 'light',
  });
  const [allocatedStorage, setAllocatedStorage] = useState(0);
  const [usedStorage, setUsedStorage] = useState(0);
  const [message, setMessage] = useState('');

  useEffect(() => {
    const fetchUserDetails = async () => {
      try {
        const userDetails = await authService.getUserDetails();
        setUsername(userDetails.username);
        setEmail(userDetails.email);
        setSettings(userDetails.settings);
        setAllocatedStorage(userDetails.allocatedStorage);
        setUsedStorage(userDetails.usedStorage);
      } catch (error) {
        setMessage('Failed to fetch user details');
      }
    };

    fetchUserDetails();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await onUpdateUser({ username, email, settings });
      setMessage('Profile updated successfully');
    } catch (error) {
      setMessage('Failed to update profile');
    }
  };

  const handleDelete = async () => {
    if (window.confirm('Are you sure you want to delete your profile? This action cannot be undone.')) {
      try {
        await onDelete();
        setMessage('Profile deleted successfully');
      } catch (error) {
        setMessage('Failed to delete profile');
      }
    }
  };

  const handleCheckboxChange = (e) => {
    setSettings({
      ...settings,
      receiveNewsletter: e.target.checked,
    });
  };

  const handleSelectChange = (e) => {
    setSettings({
      ...settings,
      theme: e.target.value,
    });
  };

  const calculateStoragePercentage = () => {
    return (usedStorage / allocatedStorage) * 100;
  };

  const getStorageBarColor = () => {
    const percentage = calculateStoragePercentage();
    if (percentage <= 50) return { used: 'blue', free: 'green' };
    if (percentage > 50 && percentage < 70) return { used: 'blue', free: 'amber' };
    return { used: 'blue', free: 'red' };
  };

  const storageBarColors = getStorageBarColor();

  return (
    <form onSubmit={handleSubmit}>
      <h2>Profile</h2>
      {message && <p>{message}</p>}
      <label>
        Username:
        <input
          type="text"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
      </label>
      <label>
        Email:
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </label>
      <label>
        Receive Newsletter:
        <input
          type="checkbox"
          checked={settings.receiveNewsletter}
          onChange={handleCheckboxChange}
        />
      </label>
      <label>
        Theme:
        <select value={settings.theme} onChange={handleSelectChange}>
          <option value="light">Light</option>
          <option value="dark">Dark</option>
        </select>
      </label>
      <div style={{ marginTop: '20px' }}>
        <h3>Storage</h3>
        <div style={{ width: '100%', height: '30px', backgroundColor: storageBarColors.free, position: 'relative', borderRadius: '5px' }}>
          <div style={{ width: `${calculateStoragePercentage()}%`, height: '100%', backgroundColor: storageBarColors.used, borderRadius: '5px 0 0 5px' }}></div>
        </div>
        <p>Used: {usedStorage} / {allocatedStorage} bytes</p>
      </div>
      <button type="submit">Update</button>
      <button type="button" onClick={handleDelete} style={{ backgroundColor: 'red', color: 'white' }}>Delete Profile</button>
    </form>
  );
};

export default UserProfileForm;
