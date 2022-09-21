import type { PageDefinition } from 'mkh-ui'
import component from './index.vue'

const page: PageDefinition = {
  name: 'admin_role',
  icon: 'role',
  path: '/admin/role',
  permissions: ['admin_role_query_get'],
  buttons: {
    add: {
      text: 'mkh.add',
      code: 'admin_role_add',
      permissions: ['admin_role_add_post'],
    },
    edit: {
      text: 'mkh.edit',
      code: 'admin_role_edit',
      permissions: ['admin_role_edit_get', 'admin_role_update_post'],
    },
    remove: {
      text: 'mkh.delete',
      code: 'admin_role_delete',
      permissions: ['admin_role_delete_delete'],
    },
    bindMenu: {
      text: 'mod.admin.menu_bind',
      code: 'admin_role_bindmenu',
      permissions: ['admin_menu_tree_get', 'admin_role_QueryBindMenus_get', 'admin_role_UpdateBindMenus_post'],
    },
  },
  component,
}
export default page
