import React, { useState, useEffect } from 'react';
import axios from 'axios';

const FilesDashboard = () => {
  const [files, setFiles] = useState([]);
  const [selectedFiles, setSelectedFiles] = useState([]);
  const [message, setMessage] = useState('');
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => {
    fetchFiles();
  }, []);

  const fetchFiles = async () => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      const response = await axios.get(`http://localhost:5000/api/file`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setFiles(response.data);
    } catch (error) {
      console.error('Failed to fetch files:', error);
    }
  };

  const handleFileChange = (e) => {
    setSelectedFiles(e.target.files);
  };

  const handleUpload = async () => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!selectedFiles.length || !user) {
      setMessage('Please provide files and ensure you are logged in.');
      return;
    }
  
    const formData = new FormData();
    for (let i = 0; i < selectedFiles.length; i++) {
      formData.append('files', selectedFiles[i]);
    }
    formData.append('userId', user.id);
  
    try {
      const response = await axios.post(`http://localhost:5000/api/file/upload`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
          Authorization: `Bearer ${user.token}`,
        },
      });
      setMessage('Files uploaded successfully');
      fetchFiles(); // Refresh files after upload
    } catch (error) {
      setMessage('Upload failed: ' + error.response.data);
    }
  };  

  const handleView = async (filename) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      const response = await axios.get(`http://localhost:5000/api/file/view/${filename}`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      alert(response.data); // Display file content
    } catch (error) {
      setMessage('View failed: ' + error.response.data);
    }
  };

  const handleDownload = async (filename) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      const response = await axios.get(`http://localhost:5000/api/file/download/${filename}`, {
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

  const handleDelete = async (filename) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      await axios.delete(`http://localhost:5000/api/file/delete`, {
        headers: { Authorization: `Bearer ${user.token}` },
        data: { userId: user.id, filename },
      });
      setMessage('File deleted successfully');
      fetchFiles(); // Refresh files after deletion
    } catch (error) {
      setMessage('Delete failed: ' + error.response.data);
    }
  };

  const handleRename = async (filename, newFilename) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      await axios.post(`http://localhost:5000/api/file/rename`, {
        userId: user.id,
        oldFilename: filename,
        newFilename: newFilename,
      }, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setMessage('File renamed successfully');
      fetchFiles(); // Refresh files after renaming
    } catch (error) {
      setMessage('Rename failed: ' + error.response.data);
    }
  };

  const handleSearch = (e) => {
    setSearchQuery(e.target.value);
  };

  const filteredFiles = files.filter(file => file.filename.includes(searchQuery));

  return (
    <div>
      <h1>Files Dashboard</h1>
      <div style={{ display: 'flex', justifyContent: 'space-between' }}>
        <div>
          <input type="file" multiple onChange={handleFileChange} style={{ display: 'none' }} id="file-upload" />
          <label htmlFor="file-upload" className="btn">Choose Files</label>
          <button onClick={handleUpload}>Upload</button>
        </div>
        <input
          type="text"
          placeholder="Search Files"
          value={searchQuery}
          onChange={handleSearch}
        />
      </div>
      {message && <p>{message}</p>}
      <div style={{ display: 'flex', flexWrap: 'wrap' }}>
        {filteredFiles.map((file) => (
          <div key={file.filename} style={{ margin: '10px' }}>
            <img
              src={`http://localhost:5000/api/file/thumbnail/${file.filename}`}
              alt={file.filename}
              style={{ width: '100px', height: '100px' }}
              onClick={() => handleView(file.filename)}
            />
            <div>
              <button onClick={() => handleView(file.filename)}>View</button>
              <button onClick={() => handleDownload(file.filename)}>Download</button>
              <button onClick={() => handleDelete(file.filename)}>Delete</button>
              <button onClick={() => handleRename(file.filename, prompt("Enter new filename:"))}>Rename</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default FilesDashboard;
