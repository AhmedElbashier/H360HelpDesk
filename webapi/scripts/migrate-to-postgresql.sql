-- PostgreSQL Migration Script for H360 Helpdesk
-- This script helps migrate data from SQL Server to PostgreSQL

-- Note: This is a template script. You'll need to:
-- 1. Export your data from SQL Server
-- 2. Transform the data format for PostgreSQL
-- 3. Import the data into PostgreSQL

-- Example table creation (adjust based on your actual schema)
-- CREATE TABLE IF NOT EXISTS hd_users (
--     id SERIAL PRIMARY KEY,
--     username VARCHAR(255) NOT NULL,
--     email VARCHAR(255) NOT NULL,
--     password_hash VARCHAR(255) NOT NULL,
--     created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
--     updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
-- );

-- Example data transformation notes:
-- 1. SQL Server IDENTITY columns -> PostgreSQL SERIAL
-- 2. SQL Server NVARCHAR -> PostgreSQL VARCHAR
-- 3. SQL Server DATETIME -> PostgreSQL TIMESTAMP
-- 4. SQL Server BIT -> PostgreSQL BOOLEAN
-- 5. SQL Server UNIQUEIDENTIFIER -> PostgreSQL UUID

-- For automated migration, consider using:
-- - Entity Framework migrations (recommended)
-- - Tools like pgloader
-- - Custom migration scripts
