import React, { useState, useEffect } from 'react';
import axios from 'axios';
import config from '../../config';
import Modal from '../Modal';
import { getTimeDifference } from '../../utils/timeUtils';

const { apiUrl } = config;

const FilesDashboard = () => {
  const [files, setFiles] = useState([]);
  const [selectedFile, setSelectedFile] = useState(null);
  const [fileContent, setFileContent] = useState(null);
  const [fileContentType, setFileContentType] = useState('');
  const [showFileModal, setShowFileModal] = useState(false);
  const [showUploadModal, setShowUploadModal] = useState(false);
  const [selectedFiles, setSelectedFiles] = useState([]);
  const [message, setMessage] = useState('');
  const [searchQuery, setSearchQuery] = useState('');
  const [shareEmail, setShareEmail] = useState('');

  useEffect(() => {
    fetchFiles();
  }, []);

  const fetchFiles = async () => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      const response = await axios.get(`${apiUrl}/file/all`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setFiles(response.data);
    } catch (error) {
      console.error('Failed to fetch files:', error);
    }
  };

  const handleFileClick = async (fileId) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      const response = await axios.get(`${apiUrl}/file/view/${fileId}`, {
        headers: { Authorization: `Bearer ${user.token}` },
        responseType: 'blob',
      });

      const contentType = response.headers['content-type'];
      setFileContentType(contentType);

      const reader = new FileReader();
      reader.onload = (e) => {
        setFileContent(e.target.result);
        setShowFileModal(true);
      };
      reader.readAsDataURL(response.data);
    } catch (error) {
      setMessage('Failed to view file: ' + error.response.data.message);
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
      setMessage('Upload failed: ' + error.response.data.message);
    }
  };

  const handleFileMenuClick = async (fileId) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      const response = await axios.get(`${apiUrl}/file/details/${fileId}`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setSelectedFile(response.data);
      setShowFileModal(true);
    } catch (error) {
      setMessage('Failed to fetch file details: ' + error.response.data.message);
    }
  };

  const handleDownload = async (fileId) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    try {
      const response = await axios.get(`${apiUrl}/file/download/${fileId}`, {
        headers: { Authorization: `Bearer ${user.token}` },
        responseType: 'blob',
      });

      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', selectedFile.filename);
      document.body.appendChild(link);
      link.click();
      link.remove();
    } catch (error) {
      setMessage('Download failed: ' + error.response.data.message);
    }
  };

  const handleDelete = async (fileId) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) return;

    const confirmDelete = window.confirm('Are you sure you want to delete this file?');
    if (!confirmDelete) return;

    try {
      await axios.delete(`${apiUrl}/file/delete/${fileId}`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setMessage('File deleted successfully');
      setShowFileModal(false);
      fetchFiles(); // Refresh files after deletion
    } catch (error) {
      setMessage('Failed to delete file: ' + error.response.data.message);
    }
  };

  const handleSearchChange = async (e) => {
    setSearchQuery(e.target.value);
    if (e.target.value === '') {
      fetchFiles();
    } else {
      const user = JSON.parse(localStorage.getItem('user'));
      if (!user) return;

      try {
        const response = await axios.get(`${apiUrl}/file/search`, {
          headers: { Authorization: `Bearer ${user.token}` },
          params: { query: e.target.value },
        });
        setFiles(response.data);
      } catch (error) {
        console.error('Failed to search files:', error);
      }
    }
  };

  const handleShare = async (fileId) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user || !shareEmail) return;

    try {
      const response = await axios.post(`${apiUrl}/file/share/${fileId}`, { email: shareEmail }, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      setMessage('File shared successfully');
      setShareEmail('');
      // Update file details to reflect shared status
      handleFileMenuClick(fileId);
    } catch (error) {
      setMessage('Failed to share file: ' + error.response.data.message);
    }
  };

  const renderFileContent = () => {
    if (fileContentType.startsWith('image/')) {
      return <img src={fileContent} alt="file content" style={styles.fileContent} />;
    }
    if (fileContentType.startsWith('video/')) {
      return <video src={fileContent} controls style={styles.fileContent}></video>;
    }
    if (fileContentType.startsWith('audio/')) {
      return <audio src={fileContent} controls></audio>;
    }
    if (fileContentType === 'application/pdf') {
      return <embed src={fileContent} type="application/pdf" width="100%" height="600px" />;
    }
    return <pre>{fileContent}</pre>;
  };

  return (
    <div>
      <h1>Files Dashboard</h1>
      <div style={styles.topBar}>
        <input
          type="text"
          placeholder="Search files..."
          value={searchQuery}
          onChange={handleSearchChange}
          style={styles.searchInput}
        />
        <button onClick={() => setShowUploadModal(true)} style={styles.uploadButton}>Upload File</button>
      </div>
      {showUploadModal && (
        <Modal onClose={() => setShowUploadModal(false)} blurBackground>
          <div>
            <h2>Upload Files</h2>
            <input type="file" multiple onChange={handleFileChange} />
            <button onClick={handleUpload}>Upload</button>
          </div>
        </Modal>
      )}
      {message && <p>{message}</p>}
      <div style={styles.fileGrid}>
        {files.map((file) => (
          <div key={file.id} style={styles.fileCard}>
            <div onClick={() => handleFileClick(file.id)}>
              <p><strong>{file.filename.length > 20 ? `${file.filename.substring(0, 20)}...` : file.filename}</strong></p>
              <p>Size: {file.size} bytes</p>
              <p>Added: {getTimeDifference(file.dateAdded)}</p>
            </div>
            <div style={styles.fileMenu}>
              <button onClick={() => handleFileMenuClick(file.id)}>⋮</button>
            </div>
          </div>
        ))}
      </div>
      {showFileModal && (
        <Modal onClose={() => setShowFileModal(false)} blurBackground>
          {selectedFile ? (
            <div>
              <h2>File Details</h2>
              <p><strong>Filename:</strong> {selectedFile.filename.length > 20 ? `${selectedFile.filename.substring(0, 20)}...` : selectedFile.filename}</p>
              <p><strong>Size:</strong> {selectedFile.size} bytes</p>
              <p><strong>Added:</strong> {getTimeDifference(selectedFile.dateAdded)}</p>
              <p><strong>Shared With:</strong></p>
              <ul>
                {selectedFile.sharedWithEmails.map((email, index) => (
                  <li key={index}>{email}</li>
                ))}
              </ul>
              <input
                type="email"
                placeholder="Enter email to share"
                value={shareEmail}
                onChange={(e) => setShareEmail(e.target.value)}
                style={styles.shareInput}
              />
              <button onClick={() => handleShare(selectedFile.id)} style={styles.shareButton}>Share</button>
              <button onClick={() => handleDownload(selectedFile.id)}>Download</button>
              <button onClick={() => handleFileClick(selectedFile.id)}>View</button>
              <button onClick={() => handleDelete(selectedFile.id)} style={styles.deleteButton}>Delete</button>
            </div>
          ) : (
            renderFileContent()
          )}
        </Modal>
      )}
    </div>
  );
};

const styles = {
  topBar: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: '20px',
  },
  searchInput: {
    padding: '10px',
    width: '200px',
    borderRadius: '5px',
    border: '1px solid #ccc',
  },
  uploadButton: {
    margin: '10px',
    padding: '10px 20px',
    backgroundColor: '#61dafb',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
    zIndex: 1000,
  },
  fileGrid: {
    display: 'flex',
    flexWrap: 'wrap',
    gap: '10px',
  },
  fileCard: {
    padding: '10px',
    border: '1px solid #ccc',
    borderRadius: '5px',
    width: '200px',
    position: 'relative',
    cursor: 'pointer',
  },
  fileMenu: {
    position: 'absolute',
    top: '10px',
    right: '10px',
  },
  fileContent: {
    maxWidth: '100%',
    maxHeight: '600px',
  },
  shareInput: {
    padding: '10px',
    width: '200px',
    borderRadius: '5px',
    border: '1px solid #ccc',
    margin: '10px 0',
  },
  shareButton: {
    backgroundColor: '#4CAF50',
    color: 'white',
    padding: '10px',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
    marginBottom: '10px',
  },
  deleteButton: {
    backgroundColor: 'red',
    color: 'white',
    padding: '10px',
    border: 'none',
    borderRadius: '5px',
    cursor: 'pointer',
    marginTop: '10px',
  },
};

export default FilesDashboard;
