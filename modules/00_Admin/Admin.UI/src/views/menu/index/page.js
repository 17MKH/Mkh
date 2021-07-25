const name = 'admin_menu'
export default {
  name,
  title: '菜单管理',
  icon: 'menu',
  path: '/admin/menu',
  permissions: [`${name}_tree_get`, `${name}_query_get`, `${name}_UpdateSort_post`, 'admin_menugroup_select_get'],
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
      code: `${name}_remove`,
      permissions: [`${name}_delete_delete`],
    },
    group: {
      text: '分组管理',
      code: 'admin_menugroup_manage',
      permissions: ['admin_menugroup_query_get'],
    },
    groupAdd: {
      text: '分组添加',
      code: `admin_menugroup_add`,
      permissions: ['admin_menugroup_add_post'],
    },
    groupEdit: {
      text: '分组编辑',
      code: `admin_menugroup_edit`,
      permissions: [`admin_menugroup_edit_get`, `admin_menugroup_update_post`],
    },
    groupRemove: {
      text: '分组删除',
      code: `admin_menugroup_delete`,
      permissions: [`admin_menugroup_delete_delete`],
    },
  },
  component: () => import('./index.vue'),
}
