import React, { useState } from 'react';
import axios from 'axios';

const FileDelete = () => {
  const [filename, setFilename] = useState('');
  const [message, setMessage] = useState('');

  const handleDelete = async () => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) {
      setMessage('Please log in to delete files.');
      return;
    }

    try {
      const response = await axios.delete('http://localhost:3000/api/storage/delete', {
        headers: { Authorization: `Bearer ${user.token}` },
        data: { userId: user.id, filename },
      });
      setMessage(response.data.message);
    } catch (error) {
      setMessage('Delete failed: ' + error.response.data);
    }
  };

  return (
    <div>
      <h2>Delete File</h2>
      <input
        type="text"
        placeholder="Filename"
        value={filename}
        onChange={(e) => setFilename(e.target.value)}
      />
      <button onClick={handleDelete}>Delete</button>
      {message && <p>{message}</p>}
    </div>
  );
};

export default FileDelete;
