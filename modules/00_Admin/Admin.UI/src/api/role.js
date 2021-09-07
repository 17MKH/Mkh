const urls = {
  /** 查询角色绑定的菜单信息 */
  QUERY_BIND_MENUS: 'Role/QueryBindMenus',
  /** 更新角色绑定的菜单信息 */
  UPDATE_BIND_MENUS: 'Role/UpdateBindMenus',
  /** 下拉列表 */
  SELECT: 'Role/Select',
}
export default http => {
  return {
    queryBindMenus: params => http.get(urls.QUERY_BIND_MENUS, params),
    updateBindMenus: params => http.post(urls.UPDATE_BIND_MENUS, params),
    select: () => http.get(urls.SELECT),
  }
}
