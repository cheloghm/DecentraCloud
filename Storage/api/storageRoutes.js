const express = require('express');
const router = express.Router();
const storageService = require('../services/storageService');
const mime = require('mime-types');

// Upload a file
router.post('/upload', (req, res) => {
  const { filename, data } = req.body;

  try {
    storageService.saveFile(filename, Buffer.from(data, 'base64'));
    res.status(200).send('File uploaded successfully');
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// Get storage stats
router.get('/stats', (req, res) => {
  const stats = storageService.getStorageStats();
  res.status(200).json(stats);
});

// Delete a file
router.delete('/delete', (req, res) => {
  const { filename } = req.body;

  try {
    storageService.deleteFile(filename);
    res.status(200).send('File deleted successfully');
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// View a file
router.get('/view/:filename', (req, res) => {
  const { filename } = req.params;

  try {
    const fileContent = storageService.getFile(filename);
    const mimeType = mime.lookup(filename);

    // Set appropriate content type based on the file extension
    res.setHeader('Content-Type', mimeType || 'application/octet-stream');
    res.status(200).send(fileContent);
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// Download a file
router.get('/download/:filename', (req, res) => {
  const { filename } = req.params;

  try {
    const filePath = storageService.getFilePath(filename);
    const mimeType = mime.lookup(filename);

    res.setHeader('Content-Disposition', `attachment; filename="${filename}"`);
    res.setHeader('Content-Type', mimeType || 'application/octet-stream');
    res.sendFile(filePath);
  } catch (error) {
    res.status(400).send(error.message);
  }
});

module.exports = router;
