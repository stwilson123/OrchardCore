const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { VueLoaderPlugin } = require('vue-loader')

function resolve(dir) {
  return path.join(__dirname, "../../../", dir);
}
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

module.exports = {
  context: path.resolve(__dirname, "../"),
  resolve: {
    extensions: [".js", ".ts", ".vue"],
    modules: [resolve('Pack/build/node_modules'),],
    alias: {
      'gridModules': resolve('Pack/build/lib/ag-grid-dev.esm'),
      'interface': "@blocks-framework/vue"
    },
   
  },
  externals: {
    'blocks': 'blocks',
  },
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
        // exclude: /node_modules/,
        loader: "babel-loader",
        options: {
          configFile: resolve("Pack/build/.babelrc")
        }
        // options: {
        //     configFile: resolve("Pack/build/dll.babelrc")
        // }
        // include: [
        //     resolve('Modules/Blocks.LayoutModule/TemplateV2/layout/components/Sidebar/Item.vue')
        // ]
      },
      {
        test: /\.scss$/,
        loaders: [MiniCssExtractPlugin.loader, 'css-loader', 'sass-loader']
      },
      {
        test: /\.css$/,
        loaders: [MiniCssExtractPlugin.loader, "css-loader"]
      },
      {
        test: /\.(woff2?|eot|ttf|otf)(\?.*)?$/,
        loader: "url-loader"
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
  entry: { "webComponent": [resolve("Modules/Blocks.ResourcesModule/FrameworkV2/index"), resolve("Modules/Blocks.LayoutModule/init")] },
  output: {
    path: path.join(__dirname, "../lib/component/web"),
    filename: "[name]_[chunkhash].js",
    library: "[name]",
    libraryTarget: "umd"
  },
  plugins: [
    new CleanWebpackPlugin(),
    new VueLoaderPlugin(),
    new webpack.DllPlugin({
      path: path.join(__dirname, "../lib/component/web/", "[name]-manifest.json"),
      name: "[name]"
    }),
    new MiniCssExtractPlugin({
      filename: "[name]_[chunkhash].css"
      // chunkFilename: "[name]_[chunkhash].css"
    })
  ],

  stats: "errors-warnings"
};
