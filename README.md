# DecentraCloud

## Overview

DecentraCloud is a decentralized cloud platform designed to provide secure, affordable, and decentralized storage, application deployment, and Kubernetes cluster management. Additionally, it integrates cryptocurrency mining, allowing users to earn rewards based on their contributions to the network. The platform supports storage and deployment both on-premises and in the cloud.

## Key Features

- **Decentralized Storage**: Convert your PC into a storage node, securely store files across multiple nodes.
- **Application Deployment**: Manage and deploy applications using Kubernetes clusters.
- **CI/CD Integration**: Automate your development workflows with continuous integration and deployment.
- **Cryptocurrency Mining**: Earn rewards based on your system's specifications and storage contributions.
- **User-Friendly Interface**: Easy-to-use web application for managing all aspects of the platform.

## Project Structure

The project is organized into the following directories:

1. **Backend**: Manages server-side operations, APIs, business logic, and database interactions.
2. **Blockchain**: Handles blockchain integration, including smart contracts and wallet management.
3. **Frontend**: Contains the client-side code for user interfaces and interactions.
4. **Kubernetes**: Manages Kubernetes cluster setup and orchestration.
5. **Storage**: Handles decentralized storage functionality, including storage node software and encryption.
6. **.venv**: Virtual environment for managing Python dependencies.
7. **node_modules**: Node.js dependencies for the frontend and backend.

### Directory Breakdown

#### 1. Backend

The backend is built using .NET 6.0 and provides various APIs for user authentication, file management, node management, and more.

**Features:**
- User Authentication
- File Management (Upload, Download, Delete, View)
- File Sharing and Revocation
- Node Management

**Setup Instructions:**
- Install .NET 6.0 and MongoDB.
- Configure the application by updating `appsettings.json` with your MongoDB connection string and JWT settings.
- Run the application using Visual Studio 2022 or the command line.

#### 2. Blockchain

The blockchain component manages the integration with blockchain technology, including smart contracts and wallet functionality.

**Setup Instructions:**
- Develop and deploy smart contracts using Solidity.
- Integrate wallet functionality using a library like Nethereum.
- Set up and run a blockchain node.

#### 3. Frontend

The frontend is built with React.js and provides a user-friendly interface for interacting with DecentraCloud services.

**Features:**
- File Upload, View, Download, and Delete
- File Sharing and Revocation
- Search Functionality
- Responsive Design

**Setup Instructions:**
- Install Node.js and npm.
- Configure the API endpoint in `src/config.js`.
- Run the development server using `npm start`.

#### 4. Kubernetes

Manages the setup and orchestration of Kubernetes clusters, including Helm integration for simplified deployment.

**Setup Instructions:**
- Create Kubernetes manifests and Helm charts.
- Automate cluster management using scripts.

#### 5. Storage

The storage component handles decentralized storage, including the storage node software and data encryption.

**Features:**
- Decentralized Storage Nodes
- Secure Data Storage with Encryption
- API Endpoints for Managing Files
- Monitoring and Statistics

**Setup Instructions:**
- Install Node.js and Python 3.x.
- Configure environment variables in `.env`.
- Start the storage node server using `npm start`.
- Register and authenticate the storage node using provided CLI commands.

## Prerequisites

- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- [MongoDB](https://www.mongodb.com/try/download/community)
- [Node.js](https://nodejs.org/) (v14.x or later)
- [Python 3.x](https://www.python.org/downloads/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [Git](https://git-scm.com/)
- [OpenSSL](https://www.openssl.org/)

## Installation

### Clone the Repository

```bash
git clone https://github.com/cheloghm/DecentraCloud.git
cd DecentraCloud


Setup Backend
bash
Copy code
cd Backend/DecentraCloud
dotnet run --project DecentraCloud.API
Setup Frontend
bash
Copy code
cd ../frontend
npm install
npm start
Setup Storage Node
bash
Copy code
cd ../Storage
npm install
pip install -r requirements.txt
openssl req -nodes -new -x509 -keyout certs/privatekey.pem -out certs/certificate.pem
npm start
Usage
User Authentication
Register, login, and manage user details via the provided API endpoints.

File Management
Upload, view, download, and delete files through the user-friendly frontend or RESTful API.

Node Management
Register and manage your storage nodes, monitor their status, and earn cryptocurrency rewards.

API Documentation
Access the API documentation at https://localhost:7240/swagger once the backend server is running.

Contributing
Contributions are currently not open. We will provide updates when the project is ready for external contributions.

License
This project is licensed under the MIT License.
