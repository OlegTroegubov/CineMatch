// Login.js
import React, { useState } from 'react';
import axios from 'axios';
import {NavMenu} from "../NavMenu";

const Login = ({ onLogin }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleLogin = async () => {
        try {
            const response = await axios.post('/api/user/login', {
                Username: username,
                Password: password,
            });
            console.log('Login successful', response.data);
        } catch (error) {
            console.error('Login failed', error.response.data.message);
        }
    };

    return (
        <div>
            <h2>Login</h2>
            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button onClick={handleLogin}>Login</button>
        </div>
    );
};

export default Login;
