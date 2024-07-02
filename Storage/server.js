const express = require('express');
const bodyParser = require('body-parser');
const storageRoutes = require('./api/storageRoutes');

const app = express();
const PORT = 3000;

app.use(bodyParser.json());
app.use('/storage', storageRoutes);

app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});
