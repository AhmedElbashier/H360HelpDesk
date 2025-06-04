
const express = require('express');
const { createProxyMiddleware } = require('http-proxy-middleware');
const cors = require('cors');

const app = express();
const PORT = 7000;

// Use cors middleware
app.use(cors());

const apiProxy = createProxyMiddleware({
    target: 'https://portal.burujinsurance.com:1450',
    changeOrigin: true,
    pathRewrite: { '^/': '' },
    logLevel: 'debug', // This will log more detailed information
    onProxyRes: function (proxyRes, req, res) {
        // Log the response from the target server
        console.log('Response from target server:', proxyRes.statusCode);
    },
    onError: function (err, req, res) {
        console.log('Error in proxy:', err);
    }
});

app.use('/api', apiProxy);

app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});

