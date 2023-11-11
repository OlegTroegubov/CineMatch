import React, {useState} from 'react';
import axios from 'axios';

const Registration = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleRegister = async () => {
        try {
            const requestData = {
                username: username,
                password: password
            };

            await axios.post('/api/user/registration', requestData, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            console.log('Registration successful');
        } catch (error) {
            console.error('Registration failed', error);
        }
    };


    return (
        <div>
            <h2>Registration</h2>
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
            <button onClick={handleRegister}>Register</button>
        </div>
    );
};

export default Registration;
