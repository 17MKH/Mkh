const name = 'admin_role'

export default {
  //路由名称，需要唯一，推荐使用模块编码+下划线开头
  name,
  //标题
  title: '角色管理',
  //图标
  icon: 'role',
  //路由路径
  path: '/admin/role',
  //绑定权限
  permissions: [`${name}_query_get`],
  //绑定按钮
  buttons: {
    add: {
      text: '添加',
      code: `${name}_add`,
      permissions: [`${name}_add_post`],
    },
    edit: {
      text: '编辑',
      code: `${name}_edit`,
      permissions: [`${name}_edit_get`, `${name}_update_post`],
    },
    remove: {
      text: '删除',
      code: `${name}_delete`,
      permissions: [`${name}_delete_delete`],
    },
    bindMenu: {
      text: '菜单绑定',
      code: `${name}_bindmenu`,
      permissions: ['admin_menu_tree_get', `${name}_QueryBindMenus_get`, `${name}_UpdateBindMenus_post`],
    },
  },
  //路由组件
  component: () => import('./index.vue'),
}
