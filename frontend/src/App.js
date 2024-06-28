// src/App.js

import React from 'react';
import RegisterForm from './components/Auth/RegisterForm';
import LoginForm from './components/Auth/LoginForm';

function App() {
    return (
        <div>
            <h1>DecentraCloud</h1>
            <RegisterForm />
            <LoginForm />
        </div>
    );
}

export default App;
