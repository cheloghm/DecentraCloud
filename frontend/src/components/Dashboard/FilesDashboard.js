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
      const response = await axios.get(`${apiUrl}/file/${fileId}`, {
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
      <button onClick={() => setShowUploadModal(true)} style={styles.uploadButton}>Upload File</button>
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
        <Modal onClose={() => setShowFileModal(false)}>
          {selectedFile ? (
            <div>
              <h2>File Details</h2>
              <p><strong>Filename:</strong> {selectedFile.filename.length > 20 ? `${selectedFile.filename.substring(0, 20)}...` : selectedFile.filename}</p>
              <p><strong>Size:</strong> {selectedFile.size} bytes</p>
              <p><strong>Added:</strong> {getTimeDifference(selectedFile.dateAdded)}</p>
              <button onClick={() => handleDownload(selectedFile.id)}>Download</button>
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
};

export default FilesDashboard;
