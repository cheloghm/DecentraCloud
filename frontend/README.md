# DecentraCloud Frontend

## Overview

The DecentraCloud Frontend is a React-based application that allows users to interact with the DecentraCloud backend services. Users can upload, view, download, share, and delete files. The frontend provides a user-friendly interface to manage files securely.

## Features

- **File Upload**: Upload multiple files simultaneously.
- **File Viewing**: View images, videos, audio files, and PDFs directly in the browser.
- **File Download**: Download files to the local machine.
- **File Deletion**: Delete files with confirmation prompts.
- **File Sharing**: Share files with other users via email.
- **Share Revocation**: Revoke access to shared files.
- **Search**: Search files by name.
- **Responsive Design**: Works on both desktop and mobile devices.

## Prerequisites

- Node.js (>= 12.x)
- npm (>= 6.x)

## Installation

1. **Clone the repository**:
    ```bash
    git clone https://github.com/your-repo/decentracloud-frontend.git
    cd decentracloud-frontend
    ```

2. **Install dependencies**:
    ```bash
    npm install
    ```

## Configuration

Create a `config.js` file in the `src` directory with the following content:

```javascript
const config = {
  apiUrl: 'http://your-backend-url/api',
};

export default config;
```

Replace `'http://your-backend-url/api'` with the actual URL of your backend API.

## Running the Application

To start the development server, run:

```bash
npm start
```

This will launch the application in your default web browser at `http://localhost:3000`.

## Usage

### File Upload

1. Click the "Upload File" button.
2. Select files to upload from your file system.
3. Click the "Upload" button in the modal to start uploading files.

### File Viewing

1. Click on a file card to view the file content in a modal.
2. The file content will be displayed based on its type (image, video, audio, PDF, or text).

### File Download

1. Click on the three-dotted menu next to a file.
2. Select "Download" to download the file to your local machine.

### File Deletion

1. Click on the three-dotted menu next to a file.
2. Select "Delete" and confirm the deletion.

### File Sharing

1. Click on the three-dotted menu next to a file.
2. Enter the email address in the provided input box.
3. Click "Share" to share the file with the specified user.

### Share Revocation

1. Click on the three-dotted menu next to a file.
2. View the list of emails the file is shared with.
3. Click the "Revoke" button next to the email to revoke access.

### Searching Files

1. Enter a search query in the search bar located at the top right of the dashboard.
2. The file list will update to show files matching the search query.

## Project Structure

- `src/components`: Contains React components.
  - `Dashboard`: Contains the FilesDashboard component.
  - `Modal`: Contains the Modal component.
- `src/config.js`: Configuration file for API endpoints.
- `src/utils`: Contains utility functions (e.g., `timeUtils.js` for time formatting).
- `src/styles`: Contains CSS files for styling components.

## Deployment

To build the application for production, run:

```bash
npm run build
```

This will create an optimized build of the application in the `build` directory.

## Contributing

We will let you know when contributions are welcomed!

## License

This project is licensed under the MIT License.