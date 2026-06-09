const express = require('express');
const pool = require('./db');

const app = express();

app.use(express.json());

app.get('/transactions/:userId', async (req, res) => {
    try {

        const { userId } = req.params;

        const result = await pool.query(
            `
            SELECT
                t.*,
                c.name AS category_name
            FROM transactions t
            LEFT JOIN categories c
                ON t.category_id = c.id
            WHERE t.user_id = $1
            ORDER BY t.transaction_date DESC
            `,
            [userId]
        );

        res.json(result.rows);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});

app.post('/transactions', async (req, res) => {

    try {

        const {
            user_id,
            category_id,
            amount,
            description,
            type
        } = req.body;

        const result = await pool.query(
            `
            INSERT INTO transactions
            (
                user_id,
                category_id,
                amount,
                description,
                type
            )
            VALUES
            ($1,$2,$3,$4,$5)
            RETURNING *
            `,
            [
                user_id,
                category_id,
                amount,
                description,
                type
            ]
        );

        res.status(201).json(result.rows[0]);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});

app.put('/transactions/:id', async (req, res) => {

    try {

        const { id } = req.params;

        const {
            category_id,
            amount,
            description,
            type
        } = req.body;

        const result = await pool.query(
            `
            UPDATE transactions
            SET
                category_id = $1,
                amount = $2,
                description = $3,
                type = $4
            WHERE id = $5
            RETURNING *
            `,
            [
                category_id,
                amount,
                description,
                type,
                id
            ]
        );

        if (result.rows.length === 0) {
            return res.status(404).json({
                message: 'Transaction not found'
            });
        }

        res.json(result.rows[0]);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});

app.delete('/transactions/:id', async (req, res) => {

    try {

        const { id } = req.params;

        const result = await pool.query(
            `
            DELETE FROM transactions
            WHERE id = $1
            RETURNING *
            `,
            [id]
        );

        if (result.rows.length === 0) {

            return res.status(404).json({
                message: 'Transaction not found'
            });

        }

        res.json({
            message: 'Transaction deleted successfully'
        });

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});

app.get('/categories/:userId', async (req, res) => {

    try {

        const { userId } = req.params;

        const result = await pool.query(
            `
            SELECT *
            FROM categories
            WHERE user_id = $1
            ORDER BY name
            `,
            [userId]
        );

        res.json(result.rows);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});
//POST کد های
app.post('/users', async (req, res) => {
    try {
        const { full_name, email, password } = req.body;

        const result = await pool.query(
            `
            INSERT INTO users
            (
                full_name,
                email,
                password_hash
            )
            VALUES
            ($1,$2,$3)
            RETURNING *
            `,
            [full_name, email, password]
        );

        res.status(201).json(result.rows[0]);
    }
    catch (err) {
        res.status(500).json({
            error: err.message
        });
    }
});
app.post('/categories', async (req, res) =>
{
    try {

        const {
            user_id,
            name,
            type
        } = req.body;

        const result = await pool.query(
            `
            INSERT INTO categories
            (
                user_id,
                name,
                type
            )
            VALUES
            ($1,$2,$3)
            RETURNING *
            `,
            [
                user_id,
                name,
                type
            ]
        );

        res.status(201).json(result.rows[0]);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});
//Get کد های
app.get('/users', async (req, res) => {
    try {

        const result =
            await pool.query(
                'SELECT * FROM users ORDER BY id'
            );

        res.json(result.rows);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});
app.get('/budgets/:userId', async (req, res) => {

    try {

        const { userId } = req.params;

        const result = await pool.query(
            `
            SELECT *
            FROM budgets
            WHERE user_id = $1
            `,
            [userId]
        );

        res.json(result.rows);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});

app.post('/budgets', async (req, res) => {

    try {

        const {
            user_id,
            category_id,
            month,
            year,
            limit_amount
        } = req.body;

        const result = await pool.query(
            `
            INSERT INTO budgets
            (
                user_id,
                category_id,
                month,
                year,
                limit_amount
            )
            VALUES
            ($1,$2,$3,$4,$5)
            RETURNING *
            `,
            [
                user_id,
                category_id,
                month,
                year,
                limit_amount
            ]
        );

        res.status(201).json(result.rows[0]);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});
// ویرایش تراکنش
app.put('/transactions/:id', async (req, res) => {
    try {
        const { id } = req.params;

        const {
            amount,
            description,
            type,
            category_id
        } = req.body;

        const result = await pool.query(
            `
            UPDATE transactions
            SET
                amount = $1,
                description = $2,
                type = $3,
                category_id = $4
            WHERE id = $5
            RETURNING *
            `,
            [
                amount,
                description,
                type,
                category_id,
                id
            ]
        );

        res.json(result.rows[0]);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});
app.get('/categories/:userId', async (req, res) => {
    try {

        const { userId } = req.params;

        const result =
            await pool.query(
                `
                SELECT *
                FROM categories
                WHERE user_id = $1
                ORDER BY name
                `,
                [userId]
            );

        res.json(result.rows);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});
// دریافت همه کاربران
app.get('/users', async (req, res) => {
    try {

        const result =
            await pool.query(
                'SELECT * FROM users ORDER BY id'
            );

        res.json(result.rows);

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});

// ثبت کاربر جدید
app.post('/users', async (req, res) => {
    try {

        const {
            full_name,
            email
        } = req.body;

        const result =
            await pool.query(
                `
                INSERT INTO users
                (
                    full_name,
                    email,
                    password_hash
                )
                VALUES
                (
                    $1,
                    $2,
                    '123456'
                )
                RETURNING *
                `,
                [
                    full_name,
                    email
                ]
            );

        res.status(201).json(
            result.rows[0]
        );

    } catch (err) {

        res.status(500).json({
            error: err.message
        });

    }
});
app.listen(3000, () => {
    console.log('Server is running on port 3000');
});
