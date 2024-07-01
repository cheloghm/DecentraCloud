import React, { useState } from 'react';
import authService from '../services/authService';

const UserProfileForm = ({ username: initialUsername, email: initialEmail, onUpdateUser, onDelete }) => {
  const [username, setUsername] = useState(initialUsername);
  const [email, setEmail] = useState(initialEmail);
  const [settings, setSettings] = useState({
    receiveNewsletter: false,
    theme: 'light',
  });
  const [message, setMessage] = useState('');

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
      <button type="submit">Update</button>
      <button type="button" onClick={handleDelete} style={{ backgroundColor: 'red', color: 'white' }}>Delete Profile</button>
    </form>
  );
};

export default UserProfileForm;
