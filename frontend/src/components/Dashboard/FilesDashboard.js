import React, { useState, useEffect } from 'react';
import axios from 'axios';
import config from '../../config';
import Modal from '../Modal';

const { apiUrl } = config;

const FilesDashboard = () => {
  const [files, setFiles] = useState([]);
  const [selectedFiles, setSelectedFiles] = useState([]);
  const [message, setMessage] = useState('');
  const [showUploadModal, setShowUploadModal] = useState(false);

  useEffect(() => {
    fetchFiles();
  }, []);

  const fetchFiles = async () => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      const response = await axios.get(`${apiUrl}/file`, {
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
      formData.append('file', selectedFiles[i]);
    }

    try {
      const response = await axios.post(`${apiUrl}/file/upload`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
          Authorization: `Bearer ${user.token}`,
        },
      });
      setMessage('Files uploaded successfully');
      setShowUploadModal(false);
      fetchFiles(); // Refresh files after upload
    } catch (error) {
      setMessage('Upload failed: ' + error.response.data);
    }
  };

  return (
    <div>
      <h1>Files Dashboard</h1>
      <button onClick={() => setShowUploadModal(true)} style={styles.uploadButton}>Upload File</button>
      {showUploadModal && (
        <Modal onClose={() => setShowUploadModal(false)}>
          <div>
            <h2>Upload Files</h2>
            <input type="file" multiple onChange={handleFileChange} />
            <button onClick={handleUpload}>Upload</button>
          </div>
        </Modal>
      )}
      {message && <p>{message}</p>}
      <div style={styles.fileList}>
        {files.map((file) => (
          <div key={file.id} style={styles.fileItem}>
            <p>{file.filename}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

const styles = {
  uploadButton: {
    margin: '10px',
    padding: '10px 20px',
    backgroundColor: '#61dafb',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
  },
  fileList: {
    display: 'flex',
    flexDirection: 'column',
    gap: '10px',
  },
  fileItem: {
    padding: '10px',
    border: '1px solid #ccc',
    borderRadius: '5px',
  },
};

export default FilesDashboard;
