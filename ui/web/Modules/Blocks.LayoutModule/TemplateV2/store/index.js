import Vue from 'vue'
import Vuex from 'vuex'
import app from './modules/app'
import errorLog from './modules/errorLog'
import permission from './modules/permission'
import tagsView from './modules/tagsView'
import user from './modules/user'
import menu from './modules/menu'
import getters from './getters'
//import blocks from 'blocks'
Vue.use(Vuex)

const store = new Vuex.Store({
  modules: {
    app,
    errorLog,
    permission,
    tagsView,
    user,
    menu
  },
  getters
})

export default store

// blocks.event.on('RoutePath.Set', function (route) {
//   store.dispatch('SetThisRouter', route.routePath);
//   store.dispatch('SetThisModule', route.routePath.substring(1, route.routePath.substring(1).indexOf('/')));
// });
