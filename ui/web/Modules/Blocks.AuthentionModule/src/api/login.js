//import request from './../../../Blocks.LayoutModule/TemplateV2/utils/request'
import request from './../../../Blocks.ResourcesModule/FrameworkV2/utils/request'

export function loginByUsername(username, password) {
  const data = {
    username,
    password
  }
  return request({
    url:'/api/services/authentionModule/account/get',
    method: 'post',
    data
  })
}

export function getUserInfo(token) {
  return request({
    url: '/api/services/authentionModule/account/getdetail',
    method: 'post',
    data: { token }
  })
}

export function LogOut() {
  return request({
    url: '/api/services/authentionModule/account/logout',
    method: 'post'
  })
}