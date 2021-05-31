export default {
  //标题
  title: '角色管理',
  //图标
  icon: 'role',
  //路由名称，需要唯一，推荐使用模块编码+下划线开头
  name: 'admin_role',
  //路由路径
  path: '/admin/role',
  //绑定权限
  permissions: [],
  //绑定按钮
  buttons: [],
  //路由组件
  component: () => import('./index.vue'),
}
