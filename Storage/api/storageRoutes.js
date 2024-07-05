const express = require('express');
const router = express.Router();
const storageService = require('../services/storageService');

// Upload a file
router.post('/upload', (req, res) => {
  const { filename, data } = req.body;

  try {
    storageService.saveFile(filename, data);
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
    res.status(200).send(fileContent);
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// Download a file
router.get('/download/:filename', (req, res) => {
  const { filename } = req.params;

  try {
    const fileContent = storageService.getFile(filename);
    res.setHeader('Content-Disposition', `attachment; filename=${filename}`);
    res.send(fileContent);
  } catch (error) {
    res.status(400).send(error.message);
  }
});

module.exports = router;
