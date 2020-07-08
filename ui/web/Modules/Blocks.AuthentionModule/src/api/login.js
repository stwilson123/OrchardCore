//import request from './../../../Blocks.LayoutModule/TemplateV2/utils/request'
import request from './../../../Blocks.ResourcesModule/FrameworkV2/utils/request'

export function loginByUsername(username, password) {
  const data = {
    username,
    password
  }
  return request({
    url:'/api/services/users/account/login',
    method: 'post',
    data
  })
}

export function getUserInfo(token) {
  return request({
    url: '/api/services/users/account/getdetail',
    method: 'get',
    data: { token }
  })
}

export function LogOut() {
  return request({
    url: '/api/services/users/account/logoff',
    method: 'post'
  })
}