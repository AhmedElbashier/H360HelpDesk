# Database Migration Guide for H360 Helpdesk

## Current Setup
- **Development**: SQL Server (AHMED-VOCALCOM)
- **Production**: SQL Server (VCLLAECD2370YYT)
- **Repository**: https://github.com/AhmedElbashier/H360HelpDesk
- **Branch**: main (renamed from master)

## Render Deployment Status
✅ **Configuration Complete**: All services configured for free tier deployment
- Web API: PostgreSQL with Entity Framework
- Angular Frontend: Nginx with Docker
- Database: **Using existing database** (free tier limit reached)

## Database Integration Options

### Option A: Use Existing Render Database
If you already have a PostgreSQL database on Render:
1. Go to your Render dashboard
2. Find your existing database name
3. Update `render.yaml` with the correct database name
4. Deploy the services

### Option B: Use External Database
If you want to use a different database:
1. Use `render-external-db.yaml` configuration
2. Replace connection string with your database details
3. Deploy the services

### Option C: Use SQL Server (Current Setup)
Keep using your existing SQL Server:
1. Update connection string to point to your SQL Server
2. Remove PostgreSQL references
3. Deploy with SQL Server connection

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
1. ✅ Deploy with PostgreSQL (automatic via render.yaml) - **COMPLETED**
2. ✅ Use Entity Framework migrations to create schema - **AUTOMATIC**
3. Export data from SQL Server
4. Import data to PostgreSQL

**Current Status**: Ready for deployment on Render free tier

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

## Deployment Configuration

**render.yaml Configuration:**
- **Services**: 2 web services (API + Frontend) + 1 database
- **Plan**: Free tier for all services
- **Branch**: main
- **Docker**: Both services use Docker containers
- **Database**: PostgreSQL with automatic connection string injection

**Repository Structure:**
```
H360HelpDesk/
├── webapi/           # .NET 7 Web API
│   ├── Dockerfile
│   └── appsettings.Production.json
├── angularapp/       # Angular 16 Frontend
│   ├── Dockerfile
│   └── nginx.conf
└── render.yaml       # Render deployment config
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
