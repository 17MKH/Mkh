export default {
  //父级路由名称
  parent: 'admin_account',
  //标题
  title: '添加账户',
  //图标
  icon: 'add',
  //路由名称，需要唯一，推荐使用模块编码+下划线开头
  name: 'admin_account_add',
  //路由路径
  path: '/admin/account/add',
  //绑定权限
  permissions: [],
  //绑定按钮
  buttons: [],
  //路由组件
  component: () => import('./index.vue'),
  //面包屑
  breadcrumbs: [
    {
      label: '账户管理',
      to: '/admin/account',
    },
  ],
}
