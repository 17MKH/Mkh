import type { PageDefinition } from 'mkh-ui'
import component from './index.vue'

const page: PageDefinition = {
  name: 'admin_menu',
  icon: 'menu',
  path: '/admin/menu',
  permissions: ['admin_menu_tree_get', 'admin_menu_query_get', 'admin_menu_UpdateSort_post', 'admin_menugroup_select_get'],
  buttons: {
    add: {
      text: 'mkh.add',
      code: 'admin_menu_add',
      permissions: ['admin_menu_add_post'],
    },
    edit: {
      text: 'mkh.edit',
      code: 'admin_menu_edit',
      permissions: ['admin_menu_edit_get', 'admin_menu_update_post'],
    },
    remove: {
      text: 'mkh.delete',
      code: 'admin_menu_remove',
      permissions: ['admin_menu_delete_delete'],
    },
    group: {
      text: 'mod.admin.manage_group',
      code: 'admin_menugroup_manage',
      permissions: ['admin_menugroup_query_get'],
    },
    groupAdd: {
      text: 'mod.admin.add_group',
      code: 'admin_menugroup_add',
      permissions: ['admin_menugroup_add_post'],
    },
    groupEdit: {
      text: 'mod.admin.edit_group',
      code: 'admin_menugroup_edit',
      permissions: ['admin_menugroup_edit_get', 'admin_menugroup_update_post'],
    },
    groupRemove: {
      text: 'mod.admin.delete_group',
      code: 'admin_menugroup_delete',
      permissions: ['admin_menugroup_delete_delete'],
    },
  },
  component,
}

export default page
