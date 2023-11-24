// Logout.js
import React from 'react';
import axios from 'axios';

const Logout = () => {
    const handleLogout = async () => {
        try {
            const response = await axios.post('/api/user/logout', {
            });
            console.log('Logout successful', response.data);
        } catch (error) {
            console.error('Logout failed', error.response.data.message);
        }
    };

    return (
        <div>
            <h2>Logout</h2>
            <button onClick={handleLogout}>Logout</button>
        </div>
    );
};

export default Logout;
