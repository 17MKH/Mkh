import http from '../http'

const urls = {
  //获取权限列表
  GET_PERMISSIONS: 'Module/Permissions',
}

export default {
  getPermissions: (params) => http.get(urls.GET_PERMISSIONS, params),
}
