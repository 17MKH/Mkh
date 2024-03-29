import type { PageDefinition } from 'mkh-ui'
import component from './index.vue'

const page: PageDefinition = {
  name: 'admin_dict',
  icon: 'dict',
  path: '/admin/dict',
  permissions: ['admin_dict_query_get', 'admin_dictgroup_query_get', 'admin_dictitem_query_get'],
  buttons: {
    add: {
      text: 'mkh.add',
      code: 'admin_dict_add',
      permissions: ['admin_dict_add_post'],
    },
    edit: {
      text: 'mkh.edit',
      code: 'admin_dict_edit',
      permissions: ['admin_dict_edit_get', 'admin_dict_update_post'],
    },
    remove: {
      text: 'mkh.delete',
      code: 'admin_dict_delete',
      permissions: ['admin_dict_delete_delete'],
    },
    groupAdd: {
      text: 'mod.admin.add_group',
      code: 'admin_dictgroup_add',
      permissions: ['admin_dictgroup_add_post'],
    },
    groupEdit: {
      text: 'mod.admin.edit_group',
      code: 'admin_dictgroup_edit',
      permissions: ['admin_dictgroup_edit_get', 'admin_dictgroup_update_post'],
    },
    groupRemove: {
      text: 'mod.admin.delete_group',
      code: 'admin_dictgroup_delete',
      permissions: ['admin_dictgroup_delete_delete'],
    },
    itemAdd: {
      text: 'mod.admin.add_dict',
      code: 'admin_dictitem_add',
      permissions: ['admin_dictitem_add_post'],
    },
    itemEdit: {
      text: 'mod.admin.edit_dict',
      code: 'admin_dictitem_edit',
      permissions: ['admin_dictitem_edit_get', 'admin_dictitem_update_post'],
    },
    itemRemove: {
      text: 'mod.admin.delete_dict',
      code: 'admin_dictitem_delete',
      permissions: ['admin_dictitem_delete_delete'],
    },
  },
  component,
}

export default page
