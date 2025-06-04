const PROXY_CONFIG = [
  {
    context: [
      "/auth",
      "/hdusers",
    ],
    target: "http://call-srv-app.burujinsurance.com",
    secure: false
  },
  {
    context: [
      "/api/getPolData",  // Add the path for the first API here
    ],
    target: "https://portal.burujinsurance.com:1450",
    secure: false
  },
]

module.exports = PROXY_CONFIG;
