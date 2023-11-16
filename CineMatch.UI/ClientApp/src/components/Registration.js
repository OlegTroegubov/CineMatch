import React, { useState } from 'react';

const Registration = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleRegister = async () => {
        try {
            const requestData = {
                username: username,
                password: password,
            };

            const response = await fetch('/api/user/registration', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(requestData),
            });

            if (!response.ok) {
                // Обработка ошибки
                console.error(`HTTP error! Status: ${response.status}`);
                return;
            }

            console.log('Registration successful');
        } catch (error) {
            // Обработка ошибки без повторного бросания
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
