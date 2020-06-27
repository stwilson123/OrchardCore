const path = require('path');
const { VueLoaderPlugin } = require('vue-loader')
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const FriendlyErrorsWebpackPlugin = require('friendly-errors-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const webpack = require('webpack')
const { dynamicEntries, dynamicPlug } = require('./dynamicEntry')
var VueBuilder = require('vue-builder-webpack-plugin');
// const WebpackCdnPlugin = require('webpack-cdn-plugin');

const HOST = process.env.HOST
const PORT = process.env.PORT && Number(process.env.PORT)

// dynamicEntries['commons'] = ['commons'];

function resolve(dir) {
    return path.join(__dirname, '../../../', dir)
}

var WebpackMd5Hash = require('webpack-md5-hash');
const plugins = [
    new VueBuilder({
        path: resolve('Modules/'),
        fileExtensions: "bl",
        allScoped: true,
        filePathTestRegex: /[\\/]web[\\/].*\.(js|ts|css|sass|scss|html|htm|bl)$/
    }),
    new VueLoaderPlugin(),
    new webpack.NamedModulesPlugin(),
    new webpack.HotModuleReplacementPlugin(),
    new BundleAnalyzerPlugin({
        analyzerMode: 'server',
        analyzerHost: '127.0.0.1',
        analyzerPort: 8889,
        reportFilename: 'report.html',
        defaultSizes: 'parsed',
        openAnalyzer: false,
        generateStatsFile: false,
        statsFilename: 'stats.json',
        statsOptions: null,
        logLevel: 'info'
    }),
    new MiniCssExtractPlugin({
        filename: "[name]/v2/dist.css",
        chunkFilename: "Modules/Blocks.LayoutModule/dist/[id].[chunkhash].css"
    }),
    new WebpackMd5Hash(),
    new FriendlyErrorsWebpackPlugin(),
    new webpack.DllReferencePlugin({
       // context: path.resolve(__dirname, '../lib/vue'),
        manifest: require('./../lib/vue/vueVender-manifest.json'),
        //scope: 'xyz',
       // sourceType: 'commonjs2'
    })
    // new WebpackCdnPlugin({
    //     modules: [
    //         {
    //             name: 'vue',
    //             var: 'Vue',
    //             path: 'dist/vue.runtime.min.js'
    //         },
    //         {
    //             name: 'vue-router',
    //             var: 'VueRouter',
    //             path: 'dist/vue-router.min.js'
    //         },
    //         {
    //             name: 'vuex',
    //             var: 'Vuex',
    //             path: 'dist/vuex.min.js'
    //         },
    //         {
    //             name: 'element-ui',
    //             var: 'Element-ui',
    //             path: 'lib/element-ui.common.js'
    //         }
    //     ],
    //     publicPath: '/node_modules'
    // })
];

function recursiveIssuer(m) {
    if (m.issuer) {
        return recursiveIssuer(m.issuer);
    } else if (m.name) {
        return m.name;
    } else {
        return false;
    }
}
module.exports = {
    context: path.resolve(__dirname, '../'),
    entry: dynamicEntries,
    output: {
        path: resolve(''),
        filename: '[name]/v2/bundle.js', // 表示输出的js文件名,
        chunkFilename: 'Modules/Blocks.LayoutModule/dist/[id].[chunkhash].js',
        publicPath: '/',
        // libraryTarget:'umd',
        // libraryTarget: 'umd',
        // library: 'blocks',
    },
    resolve: {
        extensions: ['.js', '.ts', '.htm', '.vue', '.bl'],
        alias: {
            // 'vue': 'vue/dist/vue.js',
            //'layer': 'layui-src/release/layer/dist/layer.js',
            //'jqGrid': 'jqGrid/js/jquery.jqGrid.js',
            //'waves': 'waves/dist/waves.min.js',
            //'blocks': path.resolve(__dirname, './../../../Modules/Blocks.ResourcesModule/FrameworkV2/blockswrap.js'),
            'node_modules': path.resolve(__dirname, '../node_modules'),//path.resolve(__dirname, './../../node_modules'),
            'moduleLoader': resolve('Modules/Blocks.ResourcesModule/FrameworkV2/moduleLoader.js'),
           // 'zTree': 'ztree',
           // 'vuex': 'vuex/dist/vuex.min.js',
            "interface": "@blocks-framework/vue",
            'gridModules':resolve('Pack/build/lib/ag-grid-dev.esm'),
            // 'BlocksModule': resolve('Modules/Blocks.ResourcesModule/FrameworkV2/moduleLoader/index.ts')
        },
        modules: [resolve('Pack/build/node_modules'), ]
    },
    externals: {
        'blocks': 'blocks',
        // 'vue': 'Vue',
        // 'vue-router': 'VueRouter',
        'highcharts': 'Highcharts',
        'qrCode': 'QRCode',
     
        //'element-ui': 'ELEMENT',
       // 'interface': 'interface'
        // 'interface': 'interface'
    },
    target: 'web',
    module: {
        rules: [
            {
                test: /\.(vue|bl)$/,
                loader: 'vue-loader',
                options: {
                    loaders: {
                        'scss': 'vue-style-loader!css-loader?!postcss-loader!sass-loader'
                    },

                }
            },
            {
                test: /\.(js|ts)$/,
                exclude: /node_modules/,
                loader: 'babel-loader',
                options: {
                    configFile: resolve("Pack/build/.babelrc")
                }
                // include: [
                //     resolve('Modules/Blocks.LayoutModule/TemplateV2/layout/components/Sidebar/Item.vue')
                // ]
            },
            {
                test: /\.html$/,
                exclude: /node_modules/,
                use: { loader: 'html-loader' }
            },
            {
                test: /\.scss$/,
                loaders: [MiniCssExtractPlugin.loader, 'css-loader', 'sass-loader']
            },
            {
                test: /\.css$/,
                oneOf: [
                    {
                        resourceQuery: /module/,
                        use: [ "vue-style-loader", { loader: 'css-loader', options: { modules: true } }]
                    },

                    {
                        use:[MiniCssExtractPlugin.loader,'css-loader']
                    }

                ]

            },
            {
                test: /\.(woff2?|eot|ttf|otf)(\?.*)?$/,
                loader: 'url-loader',
            },
            {
                test: /\.svg$/,
                loader: 'svg-sprite-loader',
                include: [resolve('Modules/Blocks.LayoutModule/TemplateV2/icons')],
                options: {
                    symbolId: 'icon-[name]'
                }
            },
            {
                test: /\.(png|jpe?g|gif|svg)(\?.*)?$/,
                loader: 'url-loader',
                // exclude:[resolve("src/icons")],
                //     options: {
                //     limit: 10000,
                //     //name: utils.assetsPath('img/[name].[hash:7].[ext]')
                //     }
                exclude: [resolve("Modules/Blocks.LayoutModule/TemplateV2/icons")],
            }
        ]
    },
    devServer: {
        clientLogLevel: 'warning',
        contentBase: resolve(''),
        historyApiFallback: true,
        disableHostCheck: true,
        hot: true,
        compress: true,
        publicPath: '/',
        host: '0.0.0.0',
        port: PORT,
        overlay: { warnings: false, errors: true },
        proxy: {
            "api": {
                target: 'http://localhost:49338',
                bypass: function (req, res, proxyOptions) {
                    let match = req.originalUrl.match(/(\.(\w+)\?)|(\.(\w+)$)/);
                    if (match)
                        return req.originalUrl;
                },
                changeOrigin: true,
                secure: false
            }

        },
        quiet: false, // necessary for FriendlyErrorsPlugin
        watchOptions: {
            poll: false
        },
        writeToDisk: false,
        stats: 'minimal'
    },
    plugins: plugins.concat(dynamicPlug),
    optimization: {
        namedChunks: true,
        //runtimeChunk: true,
        // runtimeChunk: {
        //     name: entrypoint => `${entrypoint.name}/~runtime`,
        // },
        // splitChunks: {
        //     chunks: 'all',//默认只作用于异步模块，为`all`时对所有模块生效,`initial`对同步模块有效
        //     // minSize: 30000,//合并前模块文件的体积
        //     // minChunks: 1,//最少被引用次数
        //     // maxAsyncRequests: 5,
        //     // maxInitialRequests: 3,
        //     // automaticNameDelimiter: '~',//自动命名连接符
        //     name: true,
        //     cacheGroups: {
        //         // default: {
        //         //     minChunks: 1,
        //         //     priority: -20,
        //         //     reuseExistingChunk: true,

        //         // },
        //         // //打包第三方类库
        //         // commons: {
        //         //     name: "commons",
        //         //     test: /vue/,
        //         //     chunks: "initial",
        //         //     minChunks: Infinity,
        //         //     filename: "123.bundle.js",
        //         //     priority: 10//优先级更高
        //         // },
        //         //打包重复出现的代码
        //         // vendor: {
        //         //     chunks: 'initial',
        //         //     minChunks: 1,
        //         //     maxInitialRequests: 5, // The default limit is too small to showcase the effect
        //         //     minSize: 0, // This is example is too small to create commons chunks
        //         //     name: 'vendor',
        //         //     filename: './Modules/Blocks.ResourcesModule/dist/common/[name].js',

        //         // },
        //         fooStyles: {
        //             name: 'foo',
        //             test: (m, c, entry) => { 

        //                 m.constructor.name === 'CssModule' && recursiveIssuer(m) === entry
        //             },
        //             chunks: 'all',
        //             enforce: true
        //         }

        //     },
        //     // runtimeChunk:{
        //     //     name:'manifest'
        //     // }
        // }
    },
    stats: 'errors-warnings'
}


