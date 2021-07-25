const urls = {
  QUERY_BIND_MENUS: 'Role/QueryBindMenus',
  UPDATE_BIND_MENUS: 'Role/UpdateBindMenus',
  SELECT: 'Role/Select',
}
export default http => {
  /** 查询角色绑定的菜单信息 */
  const queryBindMenus = params => {
    return http.get(urls.QUERY_BIND_MENUS, params)
  }

  /** 更新角色绑定的菜单信息 */
  const updateBindMenus = params => {
    return http.post(urls.UPDATE_BIND_MENUS, params)
  }

  /** 下拉列表 */
  const select = () => {
    return http.get(urls.SELECT)
  }

  return {
    queryBindMenus,
    updateBindMenus,
    select,
  }
}
