const path = require('path');
const { VueLoaderPlugin } = require('vue-loader')
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin
const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const webpack = require('webpack')
const commonResourcesEntry = require('./commonResourcesEntry')

const HOST = process.env.HOST
const PORT = process.env.PORT && Number(process.env.PORT)

function resolve(dir) {
    return path.join(__dirname, '../../../', dir)
}

var WebpackMd5Hash = require('webpack-md5-hash');
const plugins = [new VueLoaderPlugin(),
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
    filename: "[name].css",
    chunkFilename: "[id].css"
}),
new WebpackMd5Hash()
];

module.exports = {
    context:path.resolve(__dirname, '../'),
    entry: commonResourcesEntry,
    output: {
        path:  resolve(''),
        filename: '[name]/bundle.js', // 表示输出的js文件名,
        publicPath: '/',
        // libraryTarget:'umd',
        libraryTarget: 'umd',
        library: 'blocks',
    },
    resolve: {
        extensions: ['.js', '.htm', '.vue'],
        alias: {
            'vue': 'vue/dist/vue.js',
            'layer': 'layui-src/release/layer/dist/layer.js',
            'jqGrid': 'jqGrid/js/jquery.jqGrid.js',
            'waves': 'waves/dist/waves.min.js',
           // 'blocks': path.resolve(__dirname, './../../Modules/Blocks.ResourcesModule/FrameworkPack/blocks.js'),
            'node_modules': path.resolve(__dirname,'../node_modules'),
            'moduleLoader': path.resolve(__dirname, '../../../Modules/Blocks.ResourcesModule/FrameworkV2/moduleLoader.js'),
            'zTree': 'ztree',
            'moment': 'moment/min/moment.min.js'
        },
        modules:[resolve('Pack/build/node_modules')]
    },
    externals: {
        'vue': 'Vue',
    },
    target: 'web',
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader',
                options: {
                    loaders: {
                        'scss': 'vue-style-loader!css-loader?!postcss-loader!sass-loader',
                    }
                }
            },
            {
                test: /\.js$/,
                loader: 'babel-loader',
              
               
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
                use: [MiniCssExtractPlugin.loader, 'css-loader'],
                
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
    plugins: plugins,
    // optimization: {
    //     namedChunks: true,
    // }
}


