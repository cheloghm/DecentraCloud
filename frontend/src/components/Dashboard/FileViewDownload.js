import React, { useState } from 'react';
import axios from 'axios';

const FileViewDownload = () => {
  const [filename, setFilename] = useState('');
  const [fileContent, setFileContent] = useState('');
  const [message, setMessage] = useState('');

  const handleView = async () => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) {
      setMessage('Please log in to view files.');
      return;
    }

    try {
      const response = await axios.get(`http://localhost:3000/api/storage/view/${user.id}/${filename}`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setFileContent(response.data);
    } catch (error) {
      setMessage('View failed: ' + error.response.data);
    }
  };

  const handleDownload = async () => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) {
      setMessage('Please log in to download files.');
      return;
    }

    try {
      const response = await axios.get(`http://localhost:3000/api/storage/download/${user.id}/${filename}`, {
        responseType: 'blob',
        headers: { Authorization: `Bearer ${user.token}` },
      });
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', filename);
      document.body.appendChild(link);
      link.click();
    } catch (error) {
      setMessage('Download failed: ' + error.response.data);
    }
  };

  return (
    <div>
      <h2>View and Download File</h2>
      <input
        type="text"
        placeholder="Filename"
        value={filename}
        onChange={(e) => setFilename(e.target.value)}
      />
      <button onClick={handleView}>View</button>
      <button onClick={handleDownload}>Download</button>
      {fileContent && <pre>{fileContent}</pre>}
      {message && <p>{message}</p>}
    </div>
  );
};

export default FileViewDownload;
