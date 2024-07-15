require('dotenv').config();
const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');
const storageRoutes = require('./api/storageRoutes');
const fs = require('fs');
const https = require('https');
const path = require('path');

const app = express();
const PORT = process.env.PORT || 3000;

app.use(cors());
app.use(bodyParser.json({ limit: '50mb' }));
app.use(bodyParser.urlencoded({ limit: '50mb', extended: true }));
app.use('/storage', storageRoutes);

const httpsOptions = {
  key: fs.readFileSync(path.join(__dirname, 'certs', 'privatekey.pem')),
  cert: fs.readFileSync(path.join(__dirname, 'certs', 'certificate.pem'))
};

https.createServer(httpsOptions, app).listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});
