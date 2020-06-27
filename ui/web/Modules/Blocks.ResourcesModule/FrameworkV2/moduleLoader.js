import axios from 'axios'
import {localizationManager} from './Localization/localizationManager'
const defaultOption = {
    provider: () => [],
    moduleName: 'default'
};

const moduleLoader = function (option) {

    let options = Object.assign({}, defaultOption, option);
    this.Init = function () {
        moduleRoute.addRoute(options.moduleName, this.CreateRoute());
    };

    this.providerContext = function () {
        return options.provider();;
    }

    this.CreateRoute = function () {
        let moduleRoute = [];
        let providerContextTmp = this.providerContext();
        providerContextTmp.keys().filter(k => k.indexOf('.js') > 0).forEach(fName => {
            var path = fName.substr(fName.indexOf('./') + './'.length);
            path = path.substring(0, path.indexOf('.js'));

            moduleRoute.push(
                {
                    path: path,
                    component: async function (resolve, pageContext) {
                        let module = providerContextTmp(fName);

                        let htmlView = findView(providerContextTmp, fName.substring(0, fName.indexOf('.js')));
                        let blocks = window.blocks;
                        let localization = new blocks.localization();




                        localization.dictionary =  localizationManager.getSource(localizationManager.getCurrentLanguage());
                        let isDialog = pageContext && typeof (pageContext) === "object";
                        let vueView = htmlView;
                        if (!vueView) {
                            let remoteView = await axios.get(`/${options.moduleName}/${path}?layout=`);
                            vueView = remoteView.data;
                        }
                        module.init({
                            view: vueView,
                            pageContext: isDialog ? pageContext : { moduleName: options.moduleName },
                            localization: localization
                        });

                        var viewModel = {
                            template: module._viewModelsCode.Main.el,
                            data: module._viewModelsCode.Main.data,
                            methods: module._viewModelsCode.Main.methods,
                            mounted: module._viewModelsCode.Main.mounted,
                            watch: module._viewModelsCode.Main.watch
                        };
                        resolve(!isDialog ? viewModel : { viewModel, module });
                    },
                    name: path,
                    meta: { title: '', icon: 'icon' }
                }

            );
        });


        providerContextTmp.keys().filter(k => k.indexOf('.vue') > 0).forEach(fName => {
            var path = fName.substr(fName.indexOf('./') + './'.length);
            path = path.substring(0, path.indexOf('.vue'));
            moduleRoute.push(
                {
                    path: path,
                    component: async function (resolve) {
                        let htmlView = findView(providerContextTmp, fName.substring(0, fName.indexOf('.vue')));
                        resolve(htmlView);
                    },
                    name: path,
                    meta: { title: '', icon: 'icon' }
                }

            );
        });
        return moduleRoute;
    }
}

function findView(providerContextTmp, fName) {
    let viewSuffixs = ['.html', '.htm','.vue'];
    for(let suffixIndex in viewSuffixs)
    {
        let suffix = viewSuffixs[suffixIndex];
        if (providerContextTmp.keys().includes(fName + suffix)) {
            return providerContextTmp(fName + suffix);
        }
    }
}

moduleLoader.Create = function (option) {
    return new moduleLoader(option);
}


let moduleRoute = function () { };
moduleRoute.addRoute = function (routeName, route) {
    blocksModule.current().Route = blocksModule.current().Route || {};

    blocksModule.current().Route[routeName] = route;
};

moduleRoute.getRoute = function (routeName) {
    blocksModule.current().Route = blocksModule.current().Route || {};

    return routeName ? blocksModule.current().Route[routeName] : blocksModule.current().Route;
};

let moduleDirective = function () { };
moduleDirective.addDirective = function (name, directive) {
    blocksModule.current().Directive = blocksModule.current().Directive || {};

    blocksModule.current().Directive[name] = directive;
};

moduleDirective.getDirective = function (name) {
    blocksModule.current().Directive = blocksModule.current().Directive || {};

    return name ? blocksModule.current().Directive[name] : blocksModule.current().Directive;
};


let blocksModule = function () {
    this.Init = function () {
        window.blocksV2 = window.blocksV2 || {};
    }

};
blocksModule.current = function () {
    return window.blocksV2;
}
new blocksModule().Init();
export { moduleLoader, moduleRoute, moduleDirective };
