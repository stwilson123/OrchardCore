import { getToken, setToken, removeToken } from './../../utils/auth'
import { loginByUsername, getUserInfo, LogOut } from './../../../../../Modules/Blocks.AuthentionModule/src/api/login'

const user = {
  state: {
    user: '',
    status: '',
    code: '',
    token: getToken(),
    name: '',
    avatar: '',
    introduction: '',
    roles: [],
    setting: {
      articlePlatform: []
    },
    dashboardRoute: {},
    userInfo: {}
  },

  mutations: {
    SET_CODE: (state, code) => {
      state.code = code
    },
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_INTRODUCTION: (state, introduction) => {
      state.introduction = introduction
    },
    SET_SETTING: (state, setting) => {
      state.setting = setting
    },
    SET_STATUS: (state, status) => {
      state.status = status
    },
    SET_NAME: (state, name) => {
      state.name = name
    },
    SET_AVATAR: (state, avatar) => {
      state.avatar = avatar
    },
    SET_ROLES: (state, roles) => {
      state.roles = roles
    },
    SET_DASHBOARDROUTE: (state, dashboardRoute) => {
      state.dashboardRoute = dashboardRoute
    },
    SET_USERINFO: (state, info) => {
      state.userInfo = info;
    }
  },
  actions: {
    // 用户名登录
    LoginByUsername({ commit }, userInfo) {
      const username = userInfo.username.trim()
      return new Promise((resolve, reject) => {
        loginByUsername(username, userInfo.password).then(response => {
          debugger
          const data = response.data;
          if (data.content) {
            localStorage.setItem("bilinUserID", username);
            commit('SET_TOKEN', data.content.token.access_token)
            setToken(data.content.token.access_token);
            resolve(data.content)
          }
          else {
            reject(data.msg)
          }
        }).catch(error => {
          reject(error)
        })
      })

      // return new Promise((resolve, reject) => {
      //   commit('SET_TOKEN', "10211218")
      //   setToken("10211218")
      //   resolve()
      // })
    },
    // 获取用户信息
    GetUserInfo({ commit, state }) {
      return new Promise((resolve, reject) => {
        getUserInfo(state.token).then(response => {
          if (!response.data) {
            reject('Verification failed, please login again.')
          }
          const data = response.data;
          if (data.content.success) {

            commit('SET_ROLES', ['admin']);
            commit('SET_NAME', data.content.data.trueName)
            commit('SET_AVATAR', '')
            commit('SET_INTRODUCTION', '')
            localStorage.setItem("blr2ewmsuserid", data.content.data.id);
            commit('SET_USERINFO', data.content.data)
            resolve(response)
          }
          else {
            reject(data.content.msg)
          }
        }).catch(error => {
          reject(error)
        })
      })
      // return new Promise((resolve, reject) => {
      //   commit('SET_ROLES', ['admin'])
      //   commit('SET_NAME', "唐阜敏")
      //   commit('SET_AVATAR', '')
      //   commit('SET_INTRODUCTION', '')
      //   resolve({success:true})
      // })
    },
    // 登出
    LogOut({ commit, state }) {
      return new Promise((resolve, reject) => {
        LogOut().then(() => {
          commit('SET_TOKEN', '')
          commit('SET_ROLES', [])
          removeToken()
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },
    // 前端 登出
    FedLogOut({ commit }) {
      return new Promise(resolve => {
        commit('SET_TOKEN', '')
        commit('SET_ROLES', [])
        removeToken()
        resolve()
      })
    },
    setDashboardRoute({ commit }, dashboardRoute) {
      commit('SET_DASHBOARDROUTE', dashboardRoute)
    }
  }
}

export default user
