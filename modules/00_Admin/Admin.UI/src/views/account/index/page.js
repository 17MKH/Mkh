const name = 'admin_account'

export default {
  //路由名称，需要唯一，推荐使用模块编码+下划线开头
  name,
  //标题
  title: '账户管理',
  //图标
  icon: 'user',
  //路由路径
  path: '/admin/account',
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
  },
  //路由组件
  component: () => import('./index.vue'),
}
