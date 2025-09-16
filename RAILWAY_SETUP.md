# Railway Setup Guide for H360 Helpdesk

## Current Status
✅ **API Deployed**: The .NET API is deployed to Railway but needs a database
❌ **Database**: No database configured yet

## Next Steps

### 1. Add PostgreSQL Database to Railway

1. Go to your Railway project dashboard
2. Click **"+ New"** button
3. Select **"Database"** → **"Add PostgreSQL"**
4. Railway will automatically create a PostgreSQL database

### 2. Connect Database to API

1. In your Railway project, you'll see two services:
   - `h360-helpdesk-api` (your API)
   - `postgres` (new database)

2. Click on the **`h360-helpdesk-api`** service
3. Go to **"Variables"** tab
4. Railway should automatically add these environment variables:
   - `PGHOST`
   - `PGDATABASE` 
   - `PGUSER`
   - `PGPASSWORD`

### 3. Redeploy

Once the database is added and variables are set:
1. Railway will automatically redeploy your API
2. The API will now use PostgreSQL instead of trying to connect to your local SQL Server

### 4. Database Migration

The API will automatically:
- Create the database schema using Entity Framework migrations
- Set up all the required tables for the helpdesk system

## Expected Results

After setup:
- ✅ API will connect to Railway PostgreSQL
- ✅ Database schema will be created automatically
- ✅ Swagger UI will be accessible at: `https://your-api.railway.app/swagger`
- ✅ Health check will pass

## Troubleshooting

If you see connection errors:
1. Check that PostgreSQL database is added to Railway
2. Verify environment variables are set in the API service
3. Check Railway logs for any startup errors

## Cost

- **API Service**: Free tier (500 hours/month)
- **PostgreSQL Database**: Free tier (1GB storage)
- **Total**: $0/month on free tier
