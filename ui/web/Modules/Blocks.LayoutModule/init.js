import Vue from 'vue'
import Cookies from 'js-cookie'
import Element from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import './TemplateV2/styles/index.scss'
import App from "./App.vue";
import store from './TemplateV2/store'
import i18n from './TemplateV2/lang'
import './TemplateV2/icons'
//import { moduleDirective } from 'moduleLoader'
import httpRequest from './../Blocks.ResourcesModule/FrameworkV2/utils/request'
//import { ajaxRequest } from './TemplateV2/utils/ajax'
import dnd from 'awe-dnd'

Vue.use(dnd)

import { Plugin } from 'vue-fragment'
Vue.use(Plugin)

Vue.use(Element, {
  size: Cookies.get('size') || 'medium', // set element-ui default size
  i18n: (key, value) => i18n.t(key, value)
})

Vue.prototype.$http = httpRequest;

import { Loading, MessageBox, Notification, Message } from 'element-ui'
Vue.prototype.$loading = Loading.service;
Vue.prototype.$msgbox = MessageBox;
Vue.prototype.$alert = MessageBox.alert;
Vue.prototype.$confirm = MessageBox.confirm;
Vue.prototype.$prompt = MessageBox.prompt;
Vue.prototype.$notify = Notification;
//Vue.prototype.$message = Message;
Vue.prototype.$message = CreateMessage;

import './TemplateV2/permission/permission'

// for (const key in window.component) {
//   if (window.component.hasOwnProperty(key)) {
//     Vue.component(key, window.component[key]);
//   }
// }
// for (const key in moduleDirective.getDirective()) {
//   Vue.directive(key, moduleDirective.getDirective()[key]);
// }

// Vue.directive('action', {
//   inserted: function (el, binding, vnode, oldVnode) {
//     var action = binding.arg;
//     var thisRouter = store.getters.thisRouter.toLowerCase();
//     var userPermission = store.getters.userPermission;
//     var actionRouter = thisRouter + "/" + action;
//     var js = 0;
//     for (let i = 0; i < userPermission.length; i++) {
//       let resourceKey = "/" + userPermission[i].resourceKey;
//       let layResourceKey = "/layout/" + userPermission[i].resourceKey;
//       if (resourceKey == actionRouter || layResourceKey == actionRouter) {
//         js++;
//         break;
//       }
//     }
//     if (js === 0) {
//       el.parentNode.removeChild(el);
//     }
//   },
//   bind: function (el, binding, vnode, oldVnode) {
//     var action = binding.arg;
//     var thisRouter = store.getters.thisRouter.toLowerCase();
//     var userPermission = store.getters.userPermission;
//     var actionRouter = thisRouter + "/" + action;
//     var js = 0;
//     for (let i = 0; i < userPermission.length; i++) {
//       let resourceKey = "/" + userPermission[i].resourceKey;
//       let layResourceKey = "/layout/" + userPermission[i].resourceKey;
//       if (resourceKey == actionRouter || layResourceKey == actionRouter) {
//         js++;
//         break;
//       }
//     }
//     if (js === 0) {
//       el.style.display = "none";
//     }
//   }
// });
let checkPermission = (el, binding, vnode, ) => {
  let menus = store.getters.user_menus;
  let context = vnode.context;
  let uid = "";
  if (context.$route && context.$route.meta && context.$route.meta.uniqueKey) {
    uid = context.$route.meta.uniqueKey;
  }
  else {
    uid = context.$options.propsData.UniqueKey;
  }

  let contextPath = menus[uid];
  if (!contextPath)
    throw new Error(`action register error, uid [${uid}]'s ContextPath not found`);
  var action = binding.arg;
  if (!action)
    return true;
  var userPermission = store.getters.userPermission;
  contextPath = contextPath.url;
  contextPath = contextPath.startsWith("/") ? contextPath.substr(1) : contextPath;
  let resourceKey = contextPath.toLowerCase() + "/" + action.toLowerCase();
  return userPermission.has(resourceKey);

}
Vue.directive("permission", {
  inserted: function (el, binding, vnode, oldVnode) {
    if (!checkPermission(el, binding, vnode))
      el.parentNode && el.parentNode.removeChild(el);
  },
  bind: function (el, binding, vnode, oldVnode) {

    if (!checkPermission(el, binding, vnode))
      el.style.display = "none";
  }
})

// import './TemplatePack/Tradition/css/style.css'
// import './TemplatePack/Tradition/_Layout.css'
// import './TemplatePack/Tradition/css/materialize.css'
// import './TemplatePack/Tradition/css/font-awesome/css/font-awesome.css'
// import 'ag-grid-community/dist/styles/ag-grid.css'
// import 'ag-grid-community/dist/styles/ag-theme-balham.css'
export default function () {
  let router = require("./TemplateV2/router").default;
  var v = new Vue({
    el: '#app',
    router,
    store,
    i18n,
    render: h => h(App)
  })
  Vue.prototype.$l = function (key) {
    let thisModule = this.getModuleName();
    key = thisModule + "." + key;
    var values = [], len = arguments.length - 1;
    while (len-- > 0) values[len] = arguments[len + 1];
    var i18n = v.$i18n;
    return i18n._t.apply(i18n, [key, i18n.locale, i18n._getMessages(), this].concat(values));
  };
  window.v = v;
}

function CreateMessage(options) {
  // let defaultOption = {
  //   type: "info"
  // };
  // let newOptions = Object.assign({}, defaultOption, options)
  // if (newOptions.type == "error") {
  //   if (!newOptions.duration) {
  //     newOptions.duration = 0;
  //   }
  //   newOptions.showClose = true;
  // }
  // Message(newOptions);
  showMessage(options);
}
CreateMessage.success = (message) => {
  if (typeof message != "string") {
    throw new error("message must be string")
  }
  showMessage({ type: "success", message: message })
  //Message.success(message)
}
CreateMessage.error = (message) => {
  if (typeof message != "string") {
    throw new error("message must be string")
  }
  showMessage({ type: "error", message: message })
  //Message.error(message)
}
CreateMessage.info = (message) => {
  if (typeof message != "string") {
    throw new error("message must be string")
  }
  showMessage({ type: "info", message: message })
  //Message.info(message)
}
CreateMessage.warning = (message) => {
  if (typeof message != "string") {
    throw new error("message must be string")
  }
  showMessage({ type: "warning", message: message })
  //Message.warning(message)
}

function showMessage(options) {
  let defaultOption = {
    type: "info"
  };
  let newOptions = Object.assign({}, defaultOption, options)
  // if (newOptions.type == "error") {
  //   if (!newOptions.duration) {
  //     newOptions.duration = 0;
  //   }
  //   newOptions.showClose = true;
  // }
  Message(newOptions);
}