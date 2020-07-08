//import { moduleRoute } from 'moduleLoader'
import { ajaxRequest } from './TemplateV2/utils/ajax'
import store from './TemplateV2/store'
import { Bootstrapper, InjectCore } from 'interface'

let tsContext = require.context("../", true, /\web\/.+(((?<!bl)\.ts)|\.bl)$/);
InjectCore.Conainter = Bootstrapper.iocManager.getContainer();
//globalIocManager.register(c => c.bind(Types.IBootstrapper).toConstantValue(Bootstrapper));
Bootstrapper.PlugInSources.push(tsContext);

// moduleRoute.addRoute('redirect', [
//   {
//     path: '/redirect/:path*',
//     component: (resolve) => {
//       resolve(require('./TemplateV2/components/redirect/index'));
//     }
//   }
// ]);
var dashboardUrl = {};
ajaxRequest({
  url: '/api/services/settings/settings/get?groudId=DashboardRoute',
  type: 'get',
  success: function (res) {
    var res = res.content;
    dashboardUrl = res;
    store.dispatch('setDashboardRoute', res);
  }
});
// moduleRoute.addRoute('', {
//   redirect: dashboardUrl.url
// });

document.onreadystatechange = () => {
  if (document.readyState == 'complete') {
    Bootstrapper.initialize();
    let init = require('./init').default
    init();
  }
};