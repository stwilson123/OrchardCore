import axios from 'axios'
import { Message } from 'element-ui'
import blocks from 'blocks'
import httpException from "./httpException";
import "./httpExceptionHandler";
// create an axios instance 
const service = axios.create({
  //baseURL: process.env.BASE_API,
  timeout: 50000  //request timeout
})
//var ajaxHead =[];
//ajaxHead[security.antiForgery.tokenHeaderName] = security.antiForgery.getToken();
// request interceptor

var antiForgery = blocks.security.antiForgery;
service.interceptors.request.use(
  config => {
    config.headers[antiForgery.tokenHeaderName] = antiForgery.getToken()
    return config
  },
  error => {
    Promise.reject(new httpException(error))
  }
)

// response interceptor
service.interceptors.response.use(
  response => {

    if (response.data.code !== "200")
      return Promise.reject(new httpException(response))
    return response;
  },
  /**
   * 下面的注释为通过在response里，自定义code来标示请求状态
   * 当code返回如下情况则说明权限有问题，登出并返回到登录页
   * 如想通过 xmlhttprequest 来状态码标识 逻辑可写在下面error中
   * 以下代码均为样例，请结合自生需求加以修改，若不需要，则可删除
   */

  //respose => {}
  // response => {
  //   const res = response.data
  //   if (res.code !== 20000) {
  //     Message({
  //       message: res.message,
  //       type: 'error',
  //       duration: 5 * 1000
  //     })
  //     // 50008:非法的token; 50012:其他客户端登录了;  50014:Token 过期了;
  //     if (res.code === 50008 || res.code === 50012 || res.code === 50014) {
  //       // 请自行在引入 MessageBox
  //       // import { Message, MessageBox } from 'element-ui'
  //       MessageBox.confirm('你已被登出，可以取消继续留在该页面，或者重新登录', '确定登出', {
  //         confirmButtonText: '重新登录',
  //         cancelButtonText: '取消',
  //         type: 'warning'
  //       }).then(() => {
  //         store.dispatch('FedLogOut').then(() => {
  //           location.reload() // 为了重新实例化vue-router对象 避免bug
  //         })
  //       })
  //     }
  //     return Promise.reject('error')
  //   } else {
  //     return response.data
  //   }
  // },
  error => {
    //console.log('err' + error)
    // Message({
    //   message: error.message,
    //   type: 'error'
    // })
    return Promise.reject(new httpException(error))
  }
)

export default service
