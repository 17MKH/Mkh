import component from './index.vue'
const name = 'admin_dict'

export default {
  name,
  title: '字典管理',
  icon: 'dict',
  path: '/admin/dict',
  permissions: [`${name}_query_get`, 'admin_dictgroup_query_get', 'admin_dictitem_query_get'],
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
    groupAdd: {
      text: '添加分组',
      code: 'admin_dictgroup_add',
      permissions: ['admin_dictgroup_add_post'],
    },
    groupEdit: {
      text: '编辑分组',
      code: 'admin_dictgroup_edit',
      permissions: ['admin_dictgroup_edit_get', 'admin_dictgroup_update_post'],
    },
    groupRemove: {
      text: '删除分组',
      code: 'admin_dictgroup_delete',
      permissions: ['admin_dictgroup_delete_delete'],
    },
    itemAdd: {
      text: '添加字典项',
      code: 'admin_dictitem_add',
      permissions: [`admin_dictitem_add_post`],
    },
    itemEdit: {
      text: '编辑字典项',
      code: 'admin_dictitem_edit',
      permissions: [`admin_dictitem_edit_get`, `admin_dictitem_update_post`],
    },
    itemRemove: {
      text: '删除字典项',
      code: 'admin_dictitem_delete',
      permissions: [`admin_dictitem_delete_delete`],
    },
  },
  component,
}
