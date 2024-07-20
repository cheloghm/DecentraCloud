import React, { useState } from 'react';
import axios from 'axios';

const FileSearch = () => {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);
  const [message, setMessage] = useState('');

  const handleSearch = async () => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (!user) {
      setMessage('Please log in to search files.');
      return;
    }

    try {
      const response = await axios.get('http://localhost:3000/api/storage/search', {
        headers: { Authorization: `Bearer ${user.token}` },
        params: { userId: user.id, query },
      });
      setResults(response.data);
    } catch (error) {
      setMessage('Search failed: ' + error.response.data);
    }
  };

  return (
    <div>
      <h2>Search Files</h2>
      <input
        type="text"
        placeholder="Search Query"
        value={query}
        onChange={(e) => setQuery(e.target.value)}
      />
      <button onClick={handleSearch}>Search</button>
      {results.length > 0 && (
        <ul>
          {results.map((result, index) => (
            <li key={index}>
              {result.filename}: {result.snippet}
            </li>
          ))}
        </ul>
      )}
      {message && <p>{message}</p>}
    </div>
  );
};

export default FileSearch;
