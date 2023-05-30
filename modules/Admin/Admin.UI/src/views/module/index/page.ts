import type { PageDefinition } from 'mkh-ui'
import component from './index.vue'

const page: PageDefinition = {
  name: 'admin_module',
  icon: 'module',
  path: '/admin/module',
  permissions: ['admin_Module_Permissions_get'],
  buttons: {},
  component,
}

export default page
