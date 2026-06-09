const { Pool } = require('pg');

const pool = new Pool({
    user: 'postgres',
    host: 'localhost',
    database: 'Cost management',
    password: '123456789',
    port: 5432,
});

pool.connect()
    .then(() => console.log('Database Connected'))
    .catch(err => console.error('Database Error:', err));

module.exports = pool;
