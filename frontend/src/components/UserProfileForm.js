// src/components/Auth/UserProfileForm.js

import React, { useState } from 'react';

const UserProfileForm = ({ username: initialUsername, email: initialEmail, onUpdateUser, onError }) => {
  const [username, setUsername] = useState(initialUsername);
  const [email, setEmail] = useState(initialEmail);
  const [settings, setSettings] = useState({
    receiveNewsletter: false,
    theme: 'light',
  });
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await onUpdateUser({ username, email, settings });
    } catch (error) {
      setError('Failed to update profile');
      onError('Failed to update profile');
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
      {error && <p style={{ color: 'red' }}>{error}</p>}
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
    </form>
  );
};

export default UserProfileForm;
