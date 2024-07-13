const fs = require('fs');
const path = require('path');
const crypto = require('crypto');

const STORAGE_DIR = path.join(__dirname, '../storage');
const STORAGE_LIMIT = process.env.STORAGE_LIMIT || 1024 * 1024 * 1024; // 1GB

const ENCRYPTION_KEY = crypto.createHash('sha256').update(String(process.env.ENCRYPTION_KEY)).digest();
const IV_LENGTH = 16; // AES block size

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

const encrypt = (buffer) => {
  const iv = crypto.randomBytes(IV_LENGTH);
  const cipher = crypto.createCipheriv('aes-256-cbc', ENCRYPTION_KEY, iv);
  let encrypted = cipher.update(buffer);
  encrypted = Buffer.concat([encrypted, cipher.final()]);
  return iv.toString('hex') + ':' + encrypted.toString('hex');
};

const decrypt = (text) => {
  const textParts = text.split(':');
  const iv = Buffer.from(textParts.shift(), 'hex');
  const encryptedText = Buffer.from(textParts.join(':'), 'hex');
  const decipher = crypto.createDecipheriv('aes-256-cbc', ENCRYPTION_KEY, iv);
  let decrypted = decipher.update(encryptedText);
  decrypted = Buffer.concat([decrypted, decipher.final()]);
  return decrypted;
};

const saveFile = (userId, filename, data) => {
  try {
    const { availableStorage } = getStorageStats();
    if (Buffer.byteLength(data, 'utf8') > availableStorage) {
      throw new Error('Not enough storage space available');
    }

    const userDir = path.join(STORAGE_DIR, userId);
    if (!fs.existsSync(userDir)) {
      fs.mkdirSync(userDir);
    }

    const encryptedData = encrypt(Buffer.from(data, 'utf8'));
    const filePath = path.join(userDir, filename);
    fs.writeFileSync(filePath, encryptedData, 'utf8');
  } catch (error) {
    console.error('Error saving file:', error.message);
    throw error;
  }
};

const deleteFile = (userId, filename) => {
  const filePath = path.join(STORAGE_DIR, userId, filename);
  if (fs.existsSync(filePath)) {
    fs.unlinkSync(filePath);
  }
};

const getFile = (userId, filename) => {
  const filePath = path.join(STORAGE_DIR, userId, filename);
  if (fs.existsSync(filePath)) {
    const encryptedData = fs.readFileSync(filePath, 'utf8');
    return decrypt(encryptedData).toString('utf8');
  } else {
    throw new Error('File not found');
  }
};

const getFileSize = (filename) => {
  const filePath = path.join(STORAGE_DIR, filename);
  if (fs.existsSync(filePath)) {
    const stats = fs.statSync(filePath);
    return stats.size;
  } else {
    throw new Error('File not found');
  }
};

const getFilePath = (userId, filename) => {
  const filePath = path.join(STORAGE_DIR, userId, filename);
  if (fs.existsSync(filePath)) {
    return filePath;
  } else {
    throw new Error('File not found');
  }
};

const searchData = (userId, query) => {
  const userDir = path.join(STORAGE_DIR, userId);
  const results = [];

  if (fs.existsSync(userDir)) {
    const files = fs.readdirSync(userDir);
    files.forEach(file => {
      const filePath = path.join(userDir, file);
      const data = fs.readFileSync(filePath, 'utf8');

      if (data.includes(query)) {
        results.push({
          filename: file,
          snippet: data.substring(0, 100) // Include a snippet of the data
        });
      }
    });
  }

  return results;
};

const renameFile = (userId, oldFilename, newFilename) => {
  const userDir = path.join(STORAGE_DIR, userId);
  const oldFilePath = path.join(userDir, oldFilename);
  const newFilePath = path.join(userDir, newFilename);

  if (fs.existsSync(oldFilePath)) {
    fs.renameSync(oldFilePath, newFilePath);
  } else {
    throw new Error('File not found');
  }
};

module.exports = {
  getStorageStats,
  saveFile,
  deleteFile,
  getFile,
  getFilePath,
  searchData,
  renameFile,
  getFileSize
};
