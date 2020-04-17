'use strict'
const merge = require('webpack-merge')
const prodEnv = require('./prod.env')

module.exports = merge(prodEnv, {
  NODE_ENV: '"development"',
  VUE_APP_SIGNALR_URL:'"http://localhost:44358/signalr"'//'"http://127.0.0.1:8083/signalr"'//
})
