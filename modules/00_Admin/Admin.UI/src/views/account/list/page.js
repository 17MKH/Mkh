export default {
  //标题
  title: '账户管理',
  //图标
  icon: 'user',
  //路由名称，需要唯一，推荐使用模块编码+下划线开头
  name: 'admin_account',
  //路由路径
  path: '/admin/account',
  //绑定权限
  permissions: [],
  //绑定按钮
  buttons: [],
  //路由组件
  component: () => import(/* chunkName: "error_403" */ './index.vue'),
}
