import type { PageDefinition } from 'mkh-ui'
import component from './index.vue'

const page: PageDefinition = {
  name: 'admin_account',
  icon: 'user',
  path: '/admin/account',
  permissions: ['admin_account_query_get', 'admin_account_DefaultPassword_get'],
  buttons: {
    add: {
      text: 'mkh.add',
      code: 'admin_account_add',
      permissions: ['admin_account_add_post'],
    },
    edit: {
      text: 'mkh.edit',
      code: 'admin_account_edit',
      permissions: ['admin_account_edit_get', 'admin_account_update_post'],
    },
    remove: {
      text: 'mkh.delete',
      code: 'admin_account_delete',
      permissions: ['admin_account_delete_delete'],
    },
  },
  component,
}

export default page
