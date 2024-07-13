const express = require('express');
const router = express.Router();
const storageService = require('../services/storageService');
const replicationService = require('../services/replicationService');
const jwt = require('jsonwebtoken');

// Middleware to check authentication
const authenticate = (req, res, next) => {
  const authHeader = req.headers['authorization'];
  if (!authHeader) return res.sendStatus(401);

  const token = authHeader.split(' ')[1];
  jwt.verify(token, process.env.JWT_SECRET, (err, user) => {
    if (err) return res.sendStatus(403);
    req.user = user;
    next();
  });
};

// Upload a file
router.post('/upload', authenticate, async (req, res) => {
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
router.get('/stats', authenticate, (req, res) => {
  const stats = storageService.getStorageStats();
  res.status(200).json(stats);
});

// Delete a file
router.delete('/delete', authenticate, (req, res) => {
  const { userId, filename } = req.body;

  try {
    storageService.deleteFile(userId, filename);
    res.status(200).send('File deleted successfully');
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// View a file
router.get('/view/:userId/:filename', authenticate, (req, res) => {
  const { userId, filename } = req.params;

  try {
    const fileContent = storageService.getFile(userId, filename);
    res.status(200).send(fileContent);
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// Download a file
router.get('/download/:userId/:filename', authenticate, (req, res) => {
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
router.get('/search', authenticate, (req, res) => {
  const { userId, query } = req.query;

  try {
    const results = storageService.searchData(userId, query);
    res.status(200).json(results);
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// Rename a file
router.post('/rename', authenticate, (req, res) => {
  const { userId, oldFilename, newFilename } = req.body;

  try {
    storageService.renameFile(userId, oldFilename, newFilename);
    res.status(200).send('File renamed successfully');
  } catch (error) {
    res.status(400).send(error.message);
  }
});

// Get file size
router.get('/file-size/:filename', authenticate, (req, res) => {
  const { filename } = req.params;
  const fileSize = storageService.getFileSize(filename);
  res.status(200).send(fileSize.toString());
});

module.exports = router;
