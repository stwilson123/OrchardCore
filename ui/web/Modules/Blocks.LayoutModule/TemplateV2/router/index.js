import Vue from 'vue'
import Router from 'vue-router'
import store from './../store'
//import { moduleRoute } from 'moduleLoader'
import Layout from './../layout/Layout'
import { Message } from 'element-ui'
import { ajaxRequest } from './../utils/ajax'
import { Bootstrapper } from "interface"
import { RouteWrapper } from "./routeComponentWrapper";
const routes = window.route || [];
// const allRoutes = moduleRoute.getRoute();
// for (const routeKey in allRoutes) {
//   const curRoute = allRoutes[routeKey];
//   if (curRoute && Array.isArray(curRoute)) {
//     routes.push({
//       path: '/' + routeKey,
//       //name: routeKey,
//       component: Layout,
//       //meta: { title: '主数据', icon: 'icon' },
//       children: curRoute
//     })
//   }
//   else {
//     routes.push(Object.assign({
//       path: '/' + routeKey,
//       //name: routeKey,
//       //meta: { title: '主数据', icon: 'icon' }
//     }, curRoute));
//   }
// }
let newRoutes = RouteWrapper;//Bootstrapper.RouteHelper.getRoute();
routes.splice(0, 0, ...newRoutes);
Vue.use(Router);
const router = new Router({
  scrollBehavior: () => ({ y: 0 }),
  routes: routes
});
const whiteList = ['/authentionmodule/login', '/layout/BussnessWebModule/MasterData/Index1', '/layout/BussnessWebModule/MasterData/LayoutDemo']
//const whiteList = ['/login']
const dashurl = store.getters.dashboardRoute.url.toLowerCase();
whiteList.push(dashurl);
router.beforeEach(async (to, from, next) => {
  var toPath = to.path.toLowerCase();
  if (toPath === "/") {
    next(dashurl)
    return;
  }
  if (whiteList.indexOf(toPath) !== -1) {
    next()
  }
  else {
    if (toPath.indexOf("redirect") != -1) {
      next();
      return;
    }
    
    var js = 0;
    let uniqueKey = to.meta.uniqueKey;
    let user_menus = store.getters.user_menus;
    if (JSON.stringify(user_menus) == "{}") {
      //let userMenu = [];
      // ajaxRequest({
      //   url: "/api/services/LayoutModule/SideBarNavigation/get",
      //   type: "post",
      //   success: function (res) {
      //     userMenu = res.content.Items;
      //   }
      // });
      let userMenu = newRoutes;
      getUserSingleRouter(userMenu);
      user_menus = store.getters.user_menus;
    }
    let hasMenu = user_menus[uniqueKey];

    let permissionRes = await Vue.prototype.$http({
      url:"/api/services/users/permission/get",
      method: "get",
    })
    let permission = permissionRes.data.content;
    let permissionHashSet = new Set(permission);
    store.dispatch('SetThisRouter', to.path);
    store.dispatch('SetThisModule', to.path.substring(1, to.path.substring(1).indexOf('/')));
    store.dispatch('SetUserPermission', permissionHashSet);
    let isHasPermission = false;

    let resourceKey = hasMenu.url.startsWith("/") ? hasMenu.url.substring(1) :  hasMenu.url;
    if(permissionHashSet.has(resourceKey + "/index"))
      isHasPermission = true;
    // ajaxRequest({
    //   url: "/api/layout/getPermission",
    //   type: "get",
    //   success: function (res) {
    //     permission = res;
    //     store.dispatch('SetThisRouter', to.path);
    //     store.dispatch('SetThisModule', to.path.substring(1, to.path.substring(1).indexOf('/')));
    //     store.dispatch('SetUserPermission', res);
    //   }
    // });
    // for (var i = 0; i < permission.length; i++) {
    //   if (("/" + permission[i].resourceKey) == (hasMenu == undefined ? "" : hasMenu.url.toLowerCase() + "/index") || ("/" + permission[i].resourceKey) == (toPath.toLowerCase() + "/index")) {
    //     js++;
    //     break;
    //   }
    // }
    if (isHasPermission) {
      next()
    }
    else {
      next('/')
    }
  }
})
export function getUserSingleRouter(userMenu) {
  for (let i in userMenu) {
    let n = userMenu[i];
    getChildren(n)
  }
}

export function getChildren(item) {
  if (item.children === undefined)
    return;
  let itemsCount = item.children.length;
  store.dispatch("SetUserMenus", { key: item.uniqueKey, url: "/" + item.path });
  if (itemsCount > 0) {
    for (let child of item.children) {
      let m = child;
      store.dispatch("SetUserMenus", { key: m.uniqueKey, url: "/" + m.path });
      getChildren(m);
    }
  }
}
export default router;
