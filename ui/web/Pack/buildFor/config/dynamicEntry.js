const { glob } = require("glob");
const path = require('path');
var dynamicEntries = {};
var dynamicPlug = [];
const HtmlWebpackPlugin = require('html-webpack-plugin')
const entryFileName = 'index';
const resolve = (dir) => path.join(__dirname, '../../../', dir);
const files = glob.sync(resolve('Modules/*/index.ts'));
const WebpackCdnPlugin = require('./module');



files.sort(function (a, b) { return b.localeCompare(a) });
files.forEach(function (f) {
    var name = /.*\/(Modules\/.*?\/index)\.ts/.exec(f)[1];
    name = name.substr(0, name.length - entryFileName.length) + 'dist';
    dynamicEntries[name] = f;
    // var plug = new HtmlWebpackPlugin({
    //     filename: path.resolve(__dirname, '../../' + name + '/index.html'),
    //     chunks: [name],
    //     template: path.resolve(__dirname, '../template.html'),
    //     inject: true
    // });
    // dynamicPlug.push(plug);
});
const manifestEntries = [];

for (let entryPoint in dynamicEntries) {
    manifestEntries.push(entryPoint + "/~runtime");
}

var htmlPlug = new HtmlWebpackPlugin({
    filename: resolve('Modules/Blocks.LayoutModule/dist/index.html'),
    chunks: Object.keys(dynamicEntries).concat(manifestEntries), //.concat(['vendor']),
    template: path.resolve(__dirname, '../template/template.html'),
    inject: true,
    cdnModule: 'thirdComponent',
    title: 'vue-element-admin',
    chunksSortMode: function (chunk1, chunk2) {
        // var order = ['vendor','/Blocks.ResourcesModule/', '/Blocks.LayoutModule/'];

        // var order1 = order.findIndex((el) => chunk1.names[0].indexOf(el) > 0);
        // var order2 = order.findIndex((el) => chunk2.names[0].indexOf(el) > 0);
        // return order1 > -1 ||  order2 > -1 ? order1 - order2 : 
        //     (chunk1.names[0] < chunk2.names[0] ? 1 : 0)  ;  
        var chunk1Name = chunk1.names[0];
        var chunk2Name = chunk2.names[0];
        var orderTop = ['vendor', '/Blocks.ResourcesModule/'];
        var orderBottom = ['/Blocks.LayoutModule/'];
        var orderTop1 = orderTop.findIndex((el) => chunk1Name.indexOf(el) > -1);
        var orderTop2 = orderTop.findIndex((el) => chunk2Name.indexOf(el) > -1);
        var orderBottom1 = orderBottom.findIndex((el) => chunk1Name.indexOf(el) > -1);
        var orderBottom2 = orderBottom.findIndex((el) => chunk2Name.indexOf(el) > -1);

        if (((orderTop1 === -1 && orderTop2 === -1 && orderBottom1 === -1 && orderBottom2 === -1) ||
            (orderBottom1 > -1 && orderBottom2 > -1)))
            return (chunk1Name > chunk2Name ? -1 : 1);

        if ((orderTop1 > -1 && orderTop2 > -1 && orderTop1 == orderTop2))
            return (chunk1Name > chunk2Name ? -1 : 1);

        if ((orderTop1 > -1 && orderTop2 > -1))
            return orderTop1 - orderTop2;
        if (orderTop1 > -1 || orderTop2 > -1)
            return 1;
        if (orderBottom1 > -1 || orderBottom2 > -1)
            return -1;
    }
});
debugger;
var cdnPlug = new WebpackCdnPlugin({
    modules: {
        'thirdComponent': [
            // { name: 'vue', var: 'Vue', path: 'dist/vue.min.js' },
            // { name: 'vue-router', var: 'VueRouter', path: 'dist/vue-router.min.js' },
            {
                name: 'blocks', var: 'blocks', version: '0.1',
                prodUrl: '/Pack/build/lib/bundle.js',
                devUrl: '/Pack/build/lib/bundle.js',
                path: '/Pack/build/lib/bundle.js',
            },
           // { name: 'element-ui', var: 'ELEMENT', path: 'lib/index.js' },
           
            // {
            //     name: 'interface',
            //     var: 'interface',
            //     version: '0.1',
            //     prodUrl: '/pack/build/lib/interface.js',
            //     devUrl: '/pack/build/lib/interface.js',
            //     path: '/pack/build/lib/interface.js',
            // },
            { name: 'vueVender', var: 'vueVender',  version: '0.1', prodUrl: '/pack/build/lib/vue/vueVender_4a2076df08f3ffa40124.js',
                devUrl: '/pack/build/lib/vue/vueVender_4a2076df08f3ffa40124.js',
                path: '/pack/build/lib/vue/vueVender_4a2076df08f3ffa40124.js', },
        ],

    },
    publicPath: '/node_modules',
    // prod: (process.env.NODE_ENV === 'production')
})
dynamicPlug.push(htmlPlug);
dynamicPlug.push(cdnPlug);
module.exports = { dynamicEntries, dynamicPlug }