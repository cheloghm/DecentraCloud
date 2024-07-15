const express = require('express');
const router = express.Router();
const storageService = require('../services/storageService');
const replicationService = require('../services/replicationService');
const axios = require('axios');

// Middleware to check authentication
const authenticate = async (req, res, next) => {
  const authHeader = req.headers['authorization'];
  if (!authHeader) return res.sendStatus(401);

  const token = authHeader.split(' ')[1];

  try {
    const response = await axios.post('https://localhost:5001/api/token/verify', { token }); // Central server URL
    req.user = response.data;
    next();
  } catch (error) {
    res.sendStatus(403);
  }
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


module.exports = router;
