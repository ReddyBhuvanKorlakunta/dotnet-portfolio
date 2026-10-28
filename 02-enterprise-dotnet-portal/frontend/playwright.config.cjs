const { defineConfig } = require('@playwright/test');
module.exports = defineConfig({testDir:'tests',workers:1,use:{baseURL:process.env.PORTAL_URL || 'http://127.0.0.1:5080',browserName:'chromium',launchOptions:process.env.CHROME_PATH?{executablePath:process.env.CHROME_PATH}:{}},reporter:[['list'],['html',{open:'never'}]]});
