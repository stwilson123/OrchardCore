//import { asyncRouterMap, constantRouterMap } from '@/router'

/**
 * 通过meta.role判断是否与当前用户权限匹配
 * @param roles
 * @param route
 */
function hasPermission(roles, route) {
  if (route.meta && route.meta.roles) {
    return roles.some(role => route.meta.roles.includes(role))
  } else {
    return true
  }
}

/**
 * 递归过滤异步路由表，返回符合用户角色权限的路由表
 * @param routes asyncRouterMap
 * @param roles
 */
function filterAsyncRouter(routes, roles) {
  const res = []

  routes.forEach(route => {
    const tmp = { ...route }
    if (hasPermission(roles, tmp)) {
      if (tmp.children) {
        tmp.children = filterAsyncRouter(tmp.children, roles)
      }
      res.push(tmp)
    }
  })

  return res
}

const permission = {
  state: {
    routers: [],
    addRouters: [],
    thisRouter: "",
    userPermission: [],
    thisModule: "AuthentionModule",
    userMenus: {}
  },
  mutations: {
    SET_ROUTERS: (state, routers) => {
      state.addRouters = routers
      state.routers = window.route // constantRouterMap.concat(routers)
    },
    SET_THISROUTER: (state, data) => {
      state.thisRouter = data
    },
    SET_USERPERMISSION: (state, data) => {
      state.userPermission = data
    },
    SET_THISMODULE: (state, data) => {
      state.thisModule = data
    },
    SET_USERMENUS: (state, data) => {
      state.userMenus[data.key] = {};
      state.userMenus[data.key].url = data.url;
    }
  },
  actions: {
    GenerateRoutes({ commit }, data) {
      return new Promise(resolve => {
        const { roles } = data
        let accessedRouters
        if (roles.includes('admin')) {
          accessedRouters = window.route// asyncRouterMap
        } else {
          //accessedRouters = filterAsyncRouter(asyncRouterMap, roles)
        }
        commit('SET_ROUTERS', accessedRouters)
        resolve()
      })
    },
    SetThisRouter({ commit }, data) {
      return new Promise(resolve => {
        commit('SET_THISROUTER', data)
        resolve()
      })
    },
    SetUserPermission({ commit }, data) {
      return new Promise(resolve => {
        commit('SET_USERPERMISSION', data)
        resolve()
      })
    },
    SetThisModule({ commit }, data) {
      return new Promise(resolve => {
        commit('SET_THISMODULE', data)
        resolve()
      })
    },
    SetUserMenus({ commit }, data) {
      return new Promise(resolve => {
        commit('SET_USERMENUS', data)
        resolve()
      })
    }
  }
}

export default permission
