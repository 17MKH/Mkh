<template>
  <m-container>
    <m-list ref="listRef" :title="title" :icon="icon" :cols="cols" :query-model="model" :query-method="query">
      <template #querybar>
        <el-form-item label="用户名：" prop="username">
          <el-input v-model="model.username" clearable />
        </el-form-item>
        <el-form-item label="姓名：" prop="name">
          <el-input v-model="model.name" clearable />
        </el-form-item>
        <el-form-item label="手机号：" prop="phone">
          <el-input v-model="model.phone" clearable />
        </el-form-item>
      </template>
      <template #buttons>
        <m-button-add :code="buttons.add.code" @click="add" />
      </template>
      <template #col-status="{ row }">
        <el-tag v-if="row.status === 0" type="info" size="small" effect="dark">未激活</el-tag>
        <el-tag v-else-if="row.status === 1" type="success" size="small" effect="dark">激活</el-tag>
        <el-tag v-if="row.status === 2" type="warning" size="small" effect="dark">禁用</el-tag>
      </template>
      <template #operation="{ row }">
        <m-button-edit :code="buttons.edit.code" @click="edit(row)" @success="refresh"></m-button-edit>
        <m-button-delete :code="buttons.remove.code" :action="remove" :data="row.id" @success="refresh"></m-button-delete>
      </template>
    </m-list>
    <save :id="selection.id" v-model="saveVisible" :mode="mode" @success="refresh" />
  </m-container>
</template>
<script>
import { useList, entityBaseCols } from 'mkh-ui'
import { reactive } from 'vue'
import page from '../index/page'
import Save from '../save/index.vue'
export default {
  components: { Save },
  setup() {
    const { title, icon, buttons } = page
    const { query, remove } = mkh.api.admin.account
    const model = reactive({ username: '', name: '', phone: '' })
    const cols = [
      { prop: 'id', label: '编号', width: '55', show: false },
      { prop: 'username', label: '用户名' },
      { prop: 'name', label: '姓名' },
      { prop: 'roleName', label: '角色' },
      { prop: 'phone', label: '手机号' },
      { prop: 'email', label: '邮箱' },
      { prop: 'status', label: '状态' },
      ...entityBaseCols,
    ]

    const list = useList()

    return {
      title,
      icon,
      buttons,
      model,
      cols,
      query,
      remove,
      ...list,
    }
  },
}
</script>
