const path = require('path')

const IS_PROD = ['production', 'prod'].includes(process.env.NODE_ENV);
const autoprefixer = require('autoprefixer');
const pxtorem = require('postcss-pxtorem');

module.exports = {
    //部署应用包时的基本 URL
    publicPath: './',
    outputDir: 'Invoice',
    lintOnSave: false,
    runtimeCompiler: true,
    productionSourceMap: false,
    devServer: {
        proxy: {
            '/api': {
                target: 'http://client.be.sulink.cn',
                ws: false,
                changeOrigin: true
            },
        }
    },
    chainWebpack: config => {
        //压缩图片工具
        const imagesRule = config.module.rule('images');
        imagesRule.use('image-webpack-loader')
            .loader('image-webpack-loader')
            .options({
                bypassOnDebug: true
            })
            .end();
        //打包分析工具
        config
            .plugin('webpack-bundle-analyzer')
            .use(require('webpack-bundle-analyzer').BundleAnalyzerPlugin)
    },
    configureWebpack: config => {
        //cdn引入资源
        config.externals = {
            'vue': 'Vue',
            'vue-router': 'VueRouter',
            'vuex': 'Vuex',
            'axios': 'axios',
            'fastclick': 'FastClick',
            'MegaPixImage': 'MegaPixImage',
            'JPEGEncoder': 'JPEGEncoder'
        }
    },
    css: {
        loaderOptions: {
            postcss: {
                plugins: [
                    autoprefixer(),
                    pxtorem({
                        rootValue: 37.5,
                        propList: ['*'],
                        selectorBlackList: ['van-circle__layer']
                    })
                ]
            }
        }
    }
};
