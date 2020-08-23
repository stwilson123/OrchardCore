const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

function resolve(dir) {
  return path.join(__dirname, "../../../", dir);
}
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

module.exports = {
  context: path.resolve(__dirname, "../"),
  resolve: {
    extensions: [".js", ".ts"],
    alias: {
      interface: "@blocks-framework/vue"
    },
  },
  module: {
    rules: [
      {
        test: /\.(js|ts)$/,
        // exclude: /node_modules/,
        loader: "babel-loader"
        // options: {
        //     configFile: resolve("Pack/build/dll.babelrc")
        // }
        // include: [
        //     resolve('Modules/Blocks.LayoutModule/TemplateV2/layout/components/Sidebar/Item.vue')
        // ]
      },
      {
        test: /\.css$/,
        loaders: [MiniCssExtractPlugin.loader, "css-loader"]
      },
      {
        test: /\.(woff2?|eot|ttf|otf)(\?.*)?$/,
        loader: "url-loader"
      }
    ]
  },
  entry: {
    //	'vueVender': ["vue", "vue-router", "vue-i18n","vuex","vue-property-decorator"],
    vueVender: [
      "vue",
      "vue-router",
      "vue-i18n",
      "vuex",
      "vue-property-decorator",
      "interface",
      "vue-function-api",
      "inversify",
      "reflect-metadata",
      "@blocks-framework/core"

    ]
    // "uiComponent":[]
  },
  output: {
    path: path.join(__dirname, "../lib/vue"),
    filename: "[name]_[chunkhash].js",
    library: "[name]",
    libraryTarget: "umd"
  },
  plugins: [
    new CleanWebpackPlugin(),
    new webpack.DllPlugin({
      path: path.join(__dirname, "../lib/vue/", "[name]-manifest.json"),
      name: "[name]"
    }),
    new MiniCssExtractPlugin({
      filename: "[name]_[chunkhash].css"
      // chunkFilename: "[name]_[chunkhash].css"
    })
  ],

  stats: "errors-warnings"
};
