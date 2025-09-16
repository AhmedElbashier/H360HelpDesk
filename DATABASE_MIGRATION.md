# Database Migration Guide for H360 Helpdesk

## Current Setup
- **Development**: SQL Server (AHMED-VOCALCOM)
- **Production**: SQL Server (VCLLAECD2370YYT)

## Render Deployment Options

### Option 1: PostgreSQL (Recommended)
**Pros:**
- Native Render support
- Automatic backups
- Easy scaling
- Cost-effective

**Cons:**
- Need to migrate data from SQL Server

**Steps:**
1. Deploy with PostgreSQL (automatic via render.yaml)
2. Use Entity Framework migrations to create schema
3. Export data from SQL Server
4. Import data to PostgreSQL

### Option 2: External SQL Server
**Pros:**
- No data migration needed
- Keep existing setup

**Cons:**
- Additional cost (~$50-100/month)
- More complex configuration
- Need to manage backups separately

## Migration Steps (PostgreSQL)

### 1. Schema Migration
Entity Framework will automatically create the PostgreSQL schema when the app starts.

### 2. Data Migration
```bash
# Export from SQL Server
sqlcmd -S VCLLAECD2370YYT -d H360_Helpdesk -E -Q "SELECT * FROM YourTable" -o data.csv

# Transform data (if needed)
# Import to PostgreSQL
psql -h your-postgres-host -d your-database -c "\COPY your_table FROM 'data.csv' WITH CSV HEADER"
```

### 3. Verify Migration
- Check table counts
- Verify data integrity
- Test application functionality

## Environment Variables for Render

The following environment variables will be automatically set by Render:

```env
DB_HOST=your-postgres-host
DB_NAME=your-database-name
DB_USER=your-username
DB_PASSWORD=your-password
```

## Backup Strategy

### Before Migration
1. Full backup of SQL Server database
2. Export all data to CSV/JSON files
3. Document current schema

### After Migration
1. Regular PostgreSQL backups (automatic with Render)
2. Test restore procedures
3. Monitor application performance

## Rollback Plan

If issues occur:
1. Keep SQL Server running during initial deployment
2. Have data export ready
3. Can quickly switch back to SQL Server connection string
4. Test rollback procedure before going live
