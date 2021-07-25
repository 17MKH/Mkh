const urls = {
  //获取权限列表
  GET_PERMISSIONS: 'Module/Permissions',
}
export default http => {
  const getPermissions = params => {
    return http.get(urls.GET_PERMISSIONS, params)
  }

  return {
    getPermissions,
  }
}
