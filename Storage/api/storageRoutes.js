const express = require('express');
const router = express.Router();
const storageService = require('../services/storageService');
const replicationService = require('../services/replicationService');

// Upload a file
router.post('/upload', async (req, res) => {
  const { userId, filename, data } = req.body;

  try {
    storageService.saveFile(userId, filename, data);
    await replicationService.replicateData(filename); // Trigger replication after upload
    res.status(200).send('File uploaded and replicated successfully');
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
  const { userId, filename } = req.body;

  try {
    storageService.deleteFile(userId, filename);
    res.status(200).send('File deleted successfully');
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// View a file
router.get('/view/:userId/:filename', (req, res) => {
  const { userId, filename } = req.params;

  try {
    const fileContent = storageService.getFile(userId, filename);
    res.status(200).send(fileContent);
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// Download a file
router.get('/download/:userId/:filename', (req, res) => {
  const { userId, filename } = req.params;

  try {
    const fileContent = storageService.getFile(userId, filename);
    res.setHeader('Content-Disposition', `attachment; filename=${filename}`);
    res.send(fileContent);
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// Search data
router.get('/search', (req, res) => {
  const { userId, query } = req.query;

  try {
    const results = storageService.searchData(userId, query);
    res.status(200).json(results);
  } catch (error) {
    res.status(400).send(error.message);
  }
});

module.exports = router;
