const fs = require('fs');
const path = require('path');

const STORAGE_DIR = path.join(__dirname, '../storage');
const STORAGE_LIMIT = 1024 * 1024 * 1024; // 1GB

if (!fs.existsSync(STORAGE_DIR)) {
  fs.mkdirSync(STORAGE_DIR);
}

const getStorageStats = () => {
  const files = fs.readdirSync(STORAGE_DIR);
  let usedStorage = 0;

  files.forEach(file => {
    const filePath = path.join(STORAGE_DIR, file);
    const stats = fs.statSync(filePath);
    usedStorage += stats.size;
  });

  return {
    usedStorage,
    availableStorage: STORAGE_LIMIT - usedStorage
  };
};

const saveFile = (filename, data) => {
  const { availableStorage } = getStorageStats();
  if (data.length > availableStorage) {
    throw new Error('Not enough storage space available');
  }

  const filePath = path.join(STORAGE_DIR, filename);
  fs.writeFileSync(filePath, data);
};

const deleteFile = (filename) => {
  const filePath = path.join(STORAGE_DIR, filename);
  if (fs.existsSync(filePath)) {
    fs.unlinkSync(filePath);
  }
};

const getFile = (filename) => {
  const filePath = path.join(STORAGE_DIR, filename);
  if (fs.existsSync(filePath)) {
    return fs.readFileSync(filePath);
  } else {
    throw new Error('File not found');
  }
};

const getFilePath = (filename) => {
  const filePath = path.join(STORAGE_DIR, filename);
  if (fs.existsSync(filePath)) {
    return filePath; // Ensure the file path is absolute
  } else {
    throw new Error('File not found');
  }
};

module.exports = {
  getStorageStats,
  saveFile,
  deleteFile,
  getFile,
  getFilePath
};
