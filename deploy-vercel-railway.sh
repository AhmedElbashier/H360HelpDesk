#!/bin/bash

echo "🚀 Deploying H360 Helpdesk to Vercel + Railway"
echo "=============================================="

# Check if required tools are installed
if ! command -v vercel &> /dev/null; then
    echo "❌ Vercel CLI not found. Installing..."
    npm install -g vercel
fi

if ! command -v railway &> /dev/null; then
    echo "❌ Railway CLI not found. Installing..."
    npm install -g @railway/cli
fi

echo ""
echo "📦 Building Angular Frontend..."
cd angularapp
npm install
npm run build
echo "✅ Angular build complete"

echo ""
echo "🌐 Deploying Frontend to Vercel..."
vercel --prod --yes
echo "✅ Frontend deployed to Vercel"

echo ""
echo "🔧 Deploying Backend to Railway..."
cd ../webapi
railway login
railway init
railway up
echo "✅ Backend deployed to Railway"

echo ""
echo "🎉 Deployment Complete!"
echo "Frontend: Check Vercel dashboard for URL"
echo "Backend: Check Railway dashboard for URL"
echo ""
echo "Next steps:"
echo "1. Update API_URL in Angular environment"
echo "2. Configure database connection in Railway"
echo "3. Test the complete application"
